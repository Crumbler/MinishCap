using MinishCap.Exceptions;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace MinishCap;

public static class Rom {
    public static byte[] Data = null!;

    public static void Load() {
        const string filename = "minishcap.gba";

        try {
            Data = File.ReadAllBytes(filename);
        } catch {
            Console.WriteLine("Rom file not found");
            MessageBox.Show("Rom file \"minishcap.gba\" not found");
            throw;
        }

        var hashData = SHA1.HashData(Data);

        // Minish Cap USA version
        const string expectedHexString = "b4bd50e4131b027c334547b4524e2dbbd4227130";

        var hexString = Convert.ToHexString(hashData);

        bool hashValid = expectedHexString.Equals(hexString, StringComparison.OrdinalIgnoreCase);

        if (!hashValid) {
            MessageBox.Show($"Invalid ROM SHA1.\nExpected: {expectedHexString}\nActual: {hexString}");
            throw new InvalidHashException(hexString, expectedHexString);
        }
    }
}