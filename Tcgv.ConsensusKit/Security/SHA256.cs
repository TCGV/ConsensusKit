using System.Security.Cryptography;
using System.Text;

namespace Tcgv.ConsensusKit.Security
{
    public class SHA256
    {
        public static byte[] Hash(byte[] data)
        {
            lock (sha256)
            {
                return sha256.ComputeHash(data);
            }
        }

        public static byte[] Hash(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            return Hash(bytes);
        }

        private static readonly SHA256Managed sha256 = new SHA256Managed();
    }
}
