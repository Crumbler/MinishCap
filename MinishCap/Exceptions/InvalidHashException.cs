namespace MinishCap.Exceptions;
public class InvalidHashException : Exception {
    public InvalidHashException(string actualHash, string expectedHash)
        : base($"Invalid ROM SHA1. Expected: {expectedHash} Actual: {actualHash}") { }
}