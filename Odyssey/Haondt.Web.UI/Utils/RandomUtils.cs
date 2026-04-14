
using System.Security.Cryptography;

namespace Haondt.Web.UI.Utils
{
    public static class RandomUtils
    {
        private const string _alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string GetHtmlId(int chars = 6)
        {
            var bytes = RandomNumberGenerator.GetBytes(chars);
            return new string([.. bytes.Select(b => _alphabet[b % _alphabet.Length])]);
        }
    }
}
