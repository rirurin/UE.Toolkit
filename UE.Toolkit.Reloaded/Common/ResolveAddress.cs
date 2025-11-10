using System.Diagnostics;

namespace UE.Toolkit.Reloaded.Common;

public class ResolveAddress
{
    
    public nuint GetDirectAddress(int offset) => (nuint)(BaseAddress + offset);
    public unsafe static nuint GetGlobalAddress(nint ptrAddress) => (nuint)(*(int*)ptrAddress + ptrAddress + 4);
    private unsafe nuint TryDerefInstructionPointer(nuint ptr)
    {
        if (ptr < (nuint)BaseAddress)
        {
            return 0;
        }

        return *(byte*)ptr switch
        {
            235 => DerefInstructionPointerShort(ptr),
            233 => DerefInstructionPointerNear(ptr),
            _ => ptr,
        };
    }
    private unsafe nuint DerefInstructionPointerShort(nuint ptr)
    {
        nuint ptr2 = ptr + (nuint)(*(sbyte*)(ptr + 1) + 2);
        return TryDerefInstructionPointer(ptr2);
    }

    private unsafe nuint DerefInstructionPointerNear(nuint ptr)
    {
        nuint ptr2 = ptr + (nuint)(*(int*)(ptr + 1) + 5);
        return TryDerefInstructionPointer(ptr2);
    }
    public nuint GetAddressMayThunk(int offset) => TryDerefInstructionPointer(GetDirectAddress(offset));
    public nuint GetIndirectAddressShort(int offset) => GetGlobalAddress((nint)BaseAddress + offset + 1);
    public nuint GetIndirectAddressShort2(int offset) => GetGlobalAddress((nint)BaseAddress + offset + 2);
    public nuint GetIndirectAddressLong(int offset) => GetGlobalAddress((nint)BaseAddress + offset + 3);
    public nuint GetIndirectAddressLong4(int offset) => GetGlobalAddress((nint)BaseAddress + offset + 4);
    
    private nint BaseAddress;
    public ResolveAddress()
    {
        var Process = System.Diagnostics.Process.GetCurrentProcess();
        BaseAddress = Process.MainModule!.BaseAddress;
    }
}