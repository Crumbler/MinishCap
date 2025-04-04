using System.Runtime.CompilerServices;

namespace MinishCap.Extensions;

public static class BitfieldExtensions {
    /// <summary>
    ///     Gets the bitfield with the specified offset and bit count
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetBits(this byte val, int offset, int bitCount) {
        var baseMask = ~((~0) << bitCount);

        return (byte)((val >>> offset) & baseMask);
    }

    // <summary>
    ///     Sets the bitfield with the specified offset and bit count
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte SetBits(this byte original, byte val, int offset, int bitCount) {
        var baseMask = ~((~0) << bitCount);
        var shiftedMask = baseMask << offset;

        return (byte)((original & ~shiftedMask) | ((val & baseMask) << offset));
    }

    /// <summary>
    ///     Gets the bitfield with the specified offset and bit count
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort GetBits(this ushort val, int offset, int bitCount) {
        var baseMask = ~((~0) << bitCount);

        return (ushort)((val >>> offset) & baseMask);
    }

    // <summary>
    ///     Sets the bitfield with the specified offset and bit count
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort SetBits(this ushort original, ushort val, int offset, int bitCount) {
        var baseMask = ~((~0) << bitCount);
        var shiftedMask = baseMask << offset;

        return (ushort)((original & ~shiftedMask) | ((val & baseMask) << offset));
    }
}