using System.Security.Cryptography;
using System.Text;

namespace Seventy.WebService.Utils.Extensions;

public static class StringExtensions
{
    public static string GetChecksum(this string str)
    {
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(str));

        var hexString = string.Concat(hash.Select(b => b.ToString("X2")));

        return hexString;
    }
}