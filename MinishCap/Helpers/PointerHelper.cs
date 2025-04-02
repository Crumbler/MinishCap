using System.Runtime.CompilerServices;

namespace MinishCap.Helpers;

public static unsafe class PointerHelper {
    public static Span<T> ToSpan<T>(ref T val, int count)
        where T : unmanaged {
        var ptr = Unsafe.AsPointer(ref val);
        return new Span<T>(ptr, count);
    }
}