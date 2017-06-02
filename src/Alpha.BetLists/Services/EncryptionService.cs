using Sunplus.Agent.Utilities;

namespace Alpha.BetLists.Services
{
    public interface EncryptionService
    {
        string CfsEncode(string codeStr, int number);
    }

    public class EncryptionServiceImpl : EncryptionService
    {
        public string CfsEncode(string codeStr, int number) => Encryption.CfsEnCode(codeStr, number);
    }
}
