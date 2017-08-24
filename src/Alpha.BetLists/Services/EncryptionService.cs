using Sunplus.Agent.Utilities;

namespace Alpha.BetLists.Services
{
    public interface EncryptionService
    {
        string CfsEncode(string codeStr, int numberOfEncodeResult);
    }

    public class EncryptionServiceImpl : EncryptionService
    {
        public string CfsEncode(string codeStr, int numberOfEncodeResult) => Encryption.CfsEnCode(codeStr, numberOfEncodeResult);
    }
}
