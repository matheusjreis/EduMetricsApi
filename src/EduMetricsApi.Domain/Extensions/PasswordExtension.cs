using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EduMetricsApi.Domain.Extensions;

public static class PasswordExtension
{
    public static string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password.SaltPassword()));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }

    public static string SaltPassword(this string password)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] salt = new byte[16];
            rng.GetBytes(salt);

            return Encoding.UTF8.GetString(salt) + password;
        }
    }
}
