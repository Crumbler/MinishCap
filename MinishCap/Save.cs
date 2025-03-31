using System.Runtime.InteropServices;

namespace MinishCap;

public static class Save {
    private record struct SaveFileAddressInfo(ushort Size,
        ushort Checksum1, ushort Checksum2,
        ushort Address1, ushort Address2);

    private record struct SaveFileStatus(
        ushort Checksum1, ushort Checksum2, uint Status);

    // "AGBZELDA:THE MINISH CAP:ZELDA 5"

    private static readonly SaveFileAddressInfo[] SaveFileAddresses =
    [
        // Save 0
        new SaveFileAddressInfo(0x500, 0x30, 0x1030, 0x80, 0x1080),
        // Save 1
        new SaveFileAddressInfo(0x500, 0x40, 0x1040, 0x580, 0x158),
        // Save 2
        new SaveFileAddressInfo(0x500, 0x50, 0x1050, 0xA80, 0x1A80),
        // Save header
        new SaveFileAddressInfo(0x10, 0x20, 0x1020, 0x70, 0x1070),
        // 4: Signature (sSignatureLong)
        new SaveFileAddressInfo(0x20, 0, 0, 0, 0x1000),
        // ? sub_0807CF3C, sub_0807CF1C, InitSaveData
        new SaveFileAddressInfo(0x20, 0x60, 0x1060, 0xF80, 0x1F80)
    ];

    /// <summary>
    ///     Description of an untouched save file
    /// </summary>
    private static readonly SaveFileStatus SaveDescInit =
    // Status is 'TINI' in ASCII
        new(0xFFFF, 0xFFFF, 1414090313);

    private static readonly SaveFileStatus SaveDescDeleted =
    // Status is 'FleD' in ASCII
        new(0xFFFF, 0xFFFF, 1181508932);

    public static void InitSaveData() {
        int errorCount = 0;

        var signatureInfo = SaveFileAddresses[4];

        var signature = "AGBZELDA:THE MINISH CAP:ZELDA 5"u8;

        if (!DataCompare(signatureInfo.Address1, signatureInfo.Size, signature)) {
            ++errorCount;
        }

        if (!DataCompare(signatureInfo.Address2, signatureInfo.Size, signature)) {
            ++errorCount;
        }

        if (errorCount != 0) {
            if (errorCount == 2) {
                SetFileStatusInit(0);
                SetFileStatusInit(1);
                SetFileStatusInit(2);
                SetFileStatusInit(3);
                SetFileStatusInit(5);
            }

            DataWrite(signatureInfo.Address1, signature);
            DataWrite(signatureInfo.Address2, signature);
        }
    }

    private static void SetFileStatusInit(int index) {
        var addressInfo = SaveFileAddresses[index];

        WriteSaveFileStatus(addressInfo.Checksum1, SaveDescInit);
        WriteSaveFileStatus(addressInfo.Checksum2, SaveDescInit);
    }

    private static void WriteSaveFileStatus(int address, in SaveFileStatus status) {
        DataWrite(address, status);
    }

    /// <summary>
    ///     Returns true if equal
    /// </summary>
    private static bool DataCompare(int address, int size, ReadOnlySpan<byte> comparand) {
        return true;
    }

    private static void DataWrite<T>(int address, in T data)
        where T : struct {
        ReadOnlySpan<byte> span;

        unsafe {
            var tSpan = MemoryMarshal.CreateReadOnlySpan(in data, 1);
            span = MemoryMarshal.AsBytes(tSpan);
        }

        DataWrite(address, span);
    }

    private static void DataWrite(int address, ReadOnlySpan<byte> span) {

    }
}