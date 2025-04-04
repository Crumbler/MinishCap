using System.Runtime.InteropServices;

namespace MinishCap.Extensions;
public static class ArrayExtensions {
    public static Span<TOut> AsSpanOf<T, TOut>(this T[] arr)
        where T : struct
        where TOut : struct {
        var span = arr.AsSpan();

        return MemoryMarshal.Cast<T, TOut>(span);
    }
}