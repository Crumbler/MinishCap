using MinishCap.Helpers;
using MinishCap.Structures;
using System.Runtime.InteropServices;

namespace MinishCap;

public static class Save {
    private record struct SaveFileAddressInfo(ushort Size,
        ushort Checksum1, ushort Checksum2,
        ushort Address1, ushort Address2);

    private record struct SaveFileStatus(
        ushort Checksum1, ushort Checksum2, uint Status);

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
        new(0xFFFF, 0xFFFF, Constants.EmptyFileSignature);

    /// <summary>
    ///     Description of a deleted file
    /// </summary>
    private static readonly SaveFileStatus SaveDescDeleted =
        new(0xFFFF, 0xFFFF, Constants.DeletedFileSignature);

    private static FileStream SaveFileStream = null!;
    private static readonly byte[] ReadBuf = new byte[0x500];

    private static void OpenOrCreateSaveFile() {
        var stream = File.Open("saveFile.bin", FileMode.OpenOrCreate,
            FileAccess.ReadWrite, FileShare.Read);

        SaveFileStream = stream;

        const int fileLength = 0x1FA0;

        stream.SetLength(fileLength);
    }

    public static void Flush() {
        SaveFileStream.Flush(true);
    }

    public static void InitSaveData() {
        OpenOrCreateSaveFile();

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

    private static int ReadSaveFileStatus(int address, ref SaveFileStatus status) {
        var span = StructHelper.AsByteSpan(ref status);

        DataRead(address, span);

        return ParseSaveFileStatus(status);
    }

    private static void WriteSaveFileStatus(int address, in SaveFileStatus status) {
        DataWrite(address, status);
    }

    /// <summary>
    ///     Parses save file status
    /// </summary>
    /// <returns>
    /// 2 for valid existing file
    /// 1 for valid empty file
    /// 0 for bad file
    /// </returns>
    private static int ParseSaveFileStatus(SaveFileStatus status) {
        switch (status.Status) {
            case Constants.FileSignature:
                if ((status.Checksum1 + status.Checksum2) == 0x10000) {
                    return 2;
                }

                return 0;

            case Constants.EmptyFileSignature:
            case Constants.DeletedFileSignature:
                if ((status.Checksum1 & status.Checksum2) == 0xFFFF) {
                    return 1;
                }

                return 0;

            default:
                return 0;
        }
    }

    public static int ReadSaveHeader(ref SaveHeader header) {
        var span = StructHelper.AsByteSpan(ref header);

        return DataDoubleReadWithStatus(3, span);
    }

    public static void WriteSaveHeader(in SaveHeader header) {
        var span = StructHelper.AsReadonlyByteSpan(header);

        DataDoubleWriteWithStatus(3, span);
    }
    
    private static int DataDoubleReadWithStatus(int index, Span<byte> data) {
        var addressInfo = SaveFileAddresses[index];

        SaveFileStatus fileStatus = default;

        int readStatus1 = ReadSaveFileStatus(addressInfo.Checksum1, ref fileStatus);
        if (readStatus1 == 2) {
            DataRead(addressInfo.Address1, data);

            if (VerifyChecksum(fileStatus, data)) {
                Console.WriteLine("Checksum 1 valid");
                return 1;
            }

            Console.WriteLine("Checksum 1 invalid");

            readStatus1 = 0;
        }

        int readStatus2 = ReadSaveFileStatus(addressInfo.Checksum2, ref fileStatus);
        if (readStatus2 == 2) {
            DataRead(addressInfo.Address2, data);

            if (VerifyChecksum(fileStatus, data)) {
                Console.WriteLine("Checksum 2 valid");
                return 1;
            }

            Console.WriteLine("Checksum 2 invalid");

            readStatus2 = 0;
        }

        if ((readStatus1 | readStatus2) == 0) {
            Console.WriteLine("Both reads failed");
            return -1;
        }

        return 0;
    }

    private static int DataDoubleWriteWithStatus(int index, ReadOnlySpan<byte> data) {
        var addressInfo = SaveFileAddresses[index];

        SaveFileStatus fileStatus = default;

        fileStatus.Status = Constants.FileSignature;

        var statusSpan = StructHelper.AsReadonlyByteSpan(fileStatus.Status);

        ushort checksum = CalculateChecksum(statusSpan, 4);
        checksum += CalculateChecksum(data, addressInfo.Size);

        fileStatus.Checksum1 = checksum;
        fileStatus.Checksum2 = (ushort)-checksum;

        DataWrite(addressInfo.Address1, data);
        WriteSaveFileStatus(addressInfo.Checksum1, fileStatus);

        DataWrite(addressInfo.Address2, data);
        WriteSaveFileStatus(addressInfo.Checksum2, fileStatus);

        return 0;
    }

    private static bool VerifyChecksum(SaveFileStatus status, ReadOnlySpan<byte> data) {

        ushort checksum = CalculateChecksum(StructHelper.AsReadonlyByteSpan(status), 4);
        checksum += CalculateChecksum(data, data.Length);

        uint temp = (uint)(status.Checksum1 << 16);

        if (status.Checksum1 != checksum ||
            status.Checksum2 != ((-temp) >> 16) ||
            status.Status != Constants.FileSignature) {
            return false;
        }

        return true;
    }

    private static ushort CalculateChecksum(ReadOnlySpan<byte> data, int size) {
        uint checksum = 0;

        var span = MemoryMarshal.Cast<byte, ushort>(data);

        for (int i = 0; size != 0; ++i, size -= 2) {
            checksum += (uint)(span[i] ^ size);
        }

        return (ushort)checksum;
    }

    private static void DataRead(int address, Span<byte> data) {
        var stream = SaveFileStream;

        stream.Position = address;

        stream.ReadExactly(data);
    }

    /// <summary>
    ///     Returns true if equal
    /// </summary>
    private static bool DataCompare(int address, int size, ReadOnlySpan<byte> comparand) {
        var stream = SaveFileStream;

        stream.Position = address;

        var span = ReadBuf.AsSpan(..size);

        stream.ReadExactly(span);

        return span.SequenceEqual(comparand);
    }

    private static void DataWrite<T>(int address, in T data)
        where T : struct {
        var span = StructHelper.AsReadonlyByteSpan(data);

        DataWrite(address, span);
    }

    private static void DataWrite(int address, ReadOnlySpan<byte> span) {
        var stream = SaveFileStream;

        stream.Position = address;

        stream.Write(span);
    }
}