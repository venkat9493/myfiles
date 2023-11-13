using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Meridian.IngestionManagement.Core.Helper
{
    /// <summary>
    /// MD5ChecksumExtensions
    /// </summary>
    public static class MD5ChecksumExtensions
    {
        /// <summary>
        /// GenerateMD5Checksum
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string GenerateMD5Checksum(string fullPath)
        {
            using (var md5Instance = MD5.Create())
            {
                using (var stream = File.OpenRead(fullPath))
                {
                    var hashResult = md5Instance.ComputeHash(stream);
                    return BitConverter.ToString(hashResult).Replace("-", "").ToLowerInvariant();
                }
            }

        }
        /// <summary>
        /// VerifyMD5Checksum
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="MD5Hash"></param>
        /// <returns></returns>
        public static bool VerifyMD5Checksum(string FilePath, string MD5Hash)
        {
            string hashOfInput = GenerateMD5Checksum(FilePath);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, MD5Hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
