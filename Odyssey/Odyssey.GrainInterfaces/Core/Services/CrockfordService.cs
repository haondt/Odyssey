using System.Security.Cryptography;
using System.Text;

namespace Odyssey.GrainInterfaces.Core.Services
{
    public class CrockfordService : ICrockfordService
    {
        private const string _alphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

        public string Encode(byte[] bytes)
        {
            if (bytes.Length == 0) return $"{_alphabet[0]}";

            var sb = new StringBuilder();
            int buffer = 0;
            int bitsLeft = 0;

            foreach (var b in bytes)
            {
                buffer = (buffer << 8) | b;
                bitsLeft += 8;

                while (bitsLeft >= 5)
                {
                    bitsLeft -= 5;
                    sb.Append(_alphabet[(buffer >> bitsLeft) & 0x1F]);
                }
            }

            // Handle any remaining bits
            if (bitsLeft > 0)
                sb.Append(_alphabet[(buffer << (5 - bitsLeft)) & 0x1F]);

            return sb.ToString();
        }

        public byte[] Decode(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be null or empty.");

            var normalized = Normalize(input);
            var result = new List<byte>();
            int buffer = 0;
            int bitsLeft = 0;

            foreach (var c in normalized)
            {
                var index = _alphabet.IndexOf(c);
                if (index < 0)
                    throw new FormatException($"Invalid Crockford Base32 character: '{c}'");

                buffer = (buffer << 5) | index;
                bitsLeft += 5;

                if (bitsLeft >= 8)
                {
                    bitsLeft -= 8;
                    result.Add((byte)((buffer >> bitsLeft) & 0xFF));
                }
            }

            return [.. result];
        }

        public string Normalize(string input)
        {
            if (input == null)
                throw new NullReferenceException("Input cannot be null");

            var sb = new StringBuilder(input.Length);
            foreach (var c in input.Trim().ToUpperInvariant())
            {
                if (!_alphabet.Contains(c))
                    continue;
                sb.Append(c switch
                {
                    'O' => '0',
                    'I' or 'L' => '1',
                    _ => c
                });
            }
            return sb.ToString();
        }


        public string Random(int length)
        {
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                sb.Append(_alphabet[RandomNumberGenerator.GetInt32(32)]);
            return sb.ToString();
        }

    }
}