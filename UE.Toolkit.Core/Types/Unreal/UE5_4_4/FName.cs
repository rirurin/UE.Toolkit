// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable CommentTypo

using System.Runtime.InteropServices;
using System.Text;

namespace UE.Toolkit.Core.Types.Unreal.UE5_4_4;

[StructLayout(LayoutKind.Sequential, Size = 8)]
public unsafe struct FName
{
    /// <summary>
    /// INTERNAL USE ONLY, DO NOT TOUCH!!!<br/>
    /// Holds a pointer to the global name pool, allowing for
    /// FNames to be properly viewed as strings.
    /// </summary>
    public static FNamePool* GFNamePool = null;

    /// <summary>
    /// INTERNAL USE ONLY, DO NOT TOUCH!!!<br/>
    /// Holds function wrapper for creating FNames, allowing it
    /// to be done with constructor.
    /// </summary>
    public static FNameHelper_FindOrStoreString? FNameHelper_FindOrStoreString;
    
    public FNameEntryId ComparisonIndex;
    public FNameEntryId Number; // #if !UE_FNAME_OUTLINE_NUMBER

    public FName(string content, EFindName findType = EFindName.FNAME_Add)
    {
        if (FNameHelper_FindOrStoreString == null)
        {
            throw new($"{nameof(FNameHelper_FindOrStoreString)} wrapper is null.");
        }
        
        var view = (FNameStringView*)Marshal.AllocHGlobal(sizeof(FNameStringView));
        view->Len = (uint)content.Length;
        view->bIsWide = false;
        view->Union.Data = (byte*)Marshal.StringToHGlobalAnsi(content);
        
        var fname = FNameHelper_FindOrStoreString(view, findType);
        ComparisonIndex = fname.ComparisonIndex;
        Number = fname.Number;
        
        Marshal.FreeHGlobal((nint)view->Union.Data);
        Marshal.FreeHGlobal((nint)view);
    }

    /// <summary>
    /// Set a new value to this <see cref="FName"/>. (Experimental)
    /// </summary>
    /// <param name="newValue">The new value. Must be less than 1024 characters.</param>
    public void SetValue(string newValue) => GetFNameEntry()->SetValue(newValue);

    public ReadOnlySpan<byte> ToSpanAnsi()
    {
        if (GFNamePool == null)
        {
            //Log.Warning($"{nameof(FName)} global pool is not set, defaulting to base {nameof(base.ToString)}");
            return new();
        }

        return GetFNameEntry()->ToSpanAnsi();
    }
    
    public ReadOnlySpan<char> ToSpanWide()
    {
        if (GFNamePool == null)
        {
            //Log.Warning($"{nameof(FName)} global pool is not set, defaulting to base {nameof(base.ToString)}");
            return new();
        }

        return GetFNameEntry()->ToSpanWide();
    }

    public override string ToString()
    {
        if (GFNamePool == null)
        {
            //Log.Warning($"{nameof(FName)} global pool is not set, defaulting to base {nameof(base.ToString)}");
            return base.ToString()!;
        }

        return GetFNameEntry()->ToString();
    }

    private FNameEntry* GetFNameEntry()
    {
        // Get appropriate pool
        var poolIdx = GetPool(ComparisonIndex.Value >> 0x10); // 0xABB2B - pool 0xA
        
        // Go to name entry in pool.
        return (FNameEntry*)((ComparisonIndex.Value & 0xFFFF) * 2 + poolIdx);
    }
    
    private static nint GetPool(uint poolIdx) => *((nint*)(GFNamePool + 1) + poolIdx);
}

public unsafe delegate FName FNameHelper_FindOrStoreString(FNameStringView* view, EFindName findType);

public enum EFindName
{
    FNAME_Find,
        
    /** Find a name or add it if it doesn't exist. */
    FNAME_Add
}

[StructLayout(LayoutKind.Sequential, Size = 0x4)]
public struct FNameEntryId
{
    public uint Value;
}

[StructLayout(LayoutKind.Sequential, Size = 0x2)]
public struct FNameEntryHeader
{
    // Flags:
    // bIsWide : 1;
    // ProbeHashBits : 5;
    // Len : 10;
    private ushort _data;
    
    // Bit 0: bIsWide (1 bit)
    public bool bIsWide
    {
        get => (_data & 0x0001) != 0;
        set => _data = (ushort)(value ? (_data | 0x0001) : (_data & ~0x0001));
    }

    // Bits 1-5: ProbeHashBits (5 bits)
    public byte ProbeHashBits
    {
        get => (byte)((_data >> 1) & 0x1F); // 0x1F = 5 bits
        set => _data = (ushort)((_data & ~0x003E) | ((value & 0x1F) << 1));
    }

    // Bits 6-15: Len (10 bits)
    public ushort Len
    {
        get => (ushort)((_data >> 6) & 0x03FF); // 0x03FF = 10 bits
        set => _data = (ushort)((_data & ~0xFFC0) | ((value & 0x03FF) << 6));
    }
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct FNameEntry
{
    private const int NAME_SIZE = 1024;
    
    [FieldOffset(0x0)] private FNameEntryHeader _header;
    [FieldOffset(0x2)] private fixed byte _ansiName[NAME_SIZE];
    [FieldOffset(0x2)] private fixed char _wideName[NAME_SIZE];

    public void SetValue(string newValue)
    {
        const int maxStrLen = NAME_SIZE - 1;
        if (newValue.Length > maxStrLen)
        {
            throw new ArgumentException($"{nameof(SetValue)} || {nameof(newValue)} cannot be longer than {maxStrLen} characters.");
        }

        if (_header.bIsWide)
        {
            fixed (char* str = _wideName)
            {
                var strBytes = Encoding.Unicode.GetBytes(newValue + '\0');
                Marshal.Copy(strBytes, 0, (nint)str, strBytes.Length);
            }
        }
        else
        {
            fixed (byte* str = _ansiName)
            {
                var strBytes = Encoding.Default.GetBytes(newValue + '\0');
                Marshal.Copy(strBytes, 0, (nint)str, strBytes.Length);
            }
        }
    }

    public Span<char> ToSpanWide()
    {
        fixed (FNameEntry* self = &this)
        {
            return new(self->_wideName, _header.Len);
        }
    }
    
    public Span<byte> ToSpanAnsi()
    {
        fixed (FNameEntry* self = &this)
        {
            return new(self->_ansiName, _header.Len);
        }
    }

    public override string ToString()
    {
        if (_header.bIsWide)
        {
            fixed (char* str = _wideName)
            {
                return new(str, 0, _header.Len);
            }
        }

        fixed (byte* str = _ansiName)
        {
            return Encoding.Default.GetString(str, _header.Len);
        }
    }
}

[StructLayout(LayoutKind.Sequential)] // Might be Size = 8
public struct FNameStringView
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct FNameStringViewUnion
    {
        [FieldOffset(0x0)] public void* Data;
        [FieldOffset(0x0)] public byte* Ansi;
        [FieldOffset(0x0)] public char* Wide;
    }

    public FNameStringViewUnion Union;
    public uint Len;
    public bool bIsWide;
}