namespace Alpha.BetLists.Services
{
    using Sunplus.Agent.Utilities;

    public interface EncryptionService
    {
        string CfsEncode(string codeStr, int numberOfEncodeResult);
    }

    public class EncryptionServiceImpl : EncryptionService
    {
        public string CfsEncode(string codeStr, int numberOfEncodeResult) => Encryption.CfsEnCode(codeStr, numberOfEncodeResult);
    }
}
