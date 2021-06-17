using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

public class SecurityUtils
{
    public static string EncriptarSHA2(string texto)
    {
        SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

        byte[] inputBytes = Encoding.UTF8.GetBytes(texto);
        byte[] hashedBytes = provider.ComputeHash(inputBytes);

        StringBuilder hash = new StringBuilder();

        for (int i = 0; i < hashedBytes.Length; i++)
            hash.Append(hashedBytes[i].ToString("x2").ToLower());

        return hash.ToString();
    }

    public static string generarLlave()
    {
        Random r = new Random();

        int size = r.Next(5, 21);

        char[] caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        byte[] data = new byte[4 * size];

        using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
        {
            crypto.GetBytes(data);
        }
        StringBuilder result = new StringBuilder(size);
        for (int i = 0; i < size; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * 4);
            var idx = rnd % caracteres.Length;

            result.Append(caracteres[idx]);
        }

        return result.ToString();

    }
}