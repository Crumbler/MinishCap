namespace MinishCap;

public static class Constants {
    /// <summary>
    ///     'MCZ3' in ASCII
    /// </summary>
    public const int FileSignature = 1296259635;

    /// <summary>
    ///     'TINI' in ASCII
    /// </summary>
    public const int EmptyFileSignature = 1414090313;

    /// <summary>
    ///     'FleD' in ASCII
    /// </summary>
    public const int DeletedFileSignature = 1181508932;

    public const int MaxGfxSlots = 44;

    public static class BackgroundControl {
        public const ushort Mosaic = 0x40;

        /// <summary>
        ///     4 bits per pixel
        /// </summary>
        public const ushort Color16 = 0;

        /// <summary>
        ///     8 bits per pixel
        /// </summary>
        public const ushort Color256 = 0x80;
    }
}