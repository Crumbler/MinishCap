using System.Runtime.InteropServices;

namespace MinishCap.Helpers;

public static class StructHelper {
    public static Span<byte> AsByteSpan<T> (ref T data)
        where T : struct {
        Span<byte> span;

        unsafe {
            var tSpan = MemoryMarshal.CreateSpan(ref data, 1);
            span = MemoryMarshal.AsBytes(tSpan);
        }

        return span;
    }

    public static ReadOnlySpan<byte> AsReadonlyByteSpan<T>(in T data)
        where T : struct {
        ReadOnlySpan<byte> span;

        unsafe {
            var tSpan = MemoryMarshal.CreateReadOnlySpan(in data, 1);
            span = MemoryMarshal.AsBytes(tSpan);
        }

        return span;
    }
}