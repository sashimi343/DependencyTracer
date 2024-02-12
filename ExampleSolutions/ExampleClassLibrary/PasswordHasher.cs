using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ExampleClassLibrary
{
    internal static class PasswordHasher
    {
        internal static string HashPassword(string password)
        {
            return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)).Aggregate(new StringBuilder(), (sb, b) => sb.AppendFormat("{0:x2}", b)).ToString();
        }
    }
}
