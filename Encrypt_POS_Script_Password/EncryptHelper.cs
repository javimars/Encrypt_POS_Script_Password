using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Encrypt_POS_Script_Password;

public static class EncryptHelper
{
    private const string PasswordHash = "Cb0rd123!";
    private const string SaltKey = "S@LT&KEY";
    private const string ViKey = "@1B2c3D4e5F6g7H8";


    public static string Encrypt(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        var keyBytes =
            new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey), 1000, HashAlgorithmName.SHA256)
                .GetBytes(256 / 8);
        var symmetricKey = new AesCryptoServiceProvider { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
        var encrypt = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(ViKey));

        byte[] cipherTextBytes;

        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                cipherTextBytes = memoryStream.ToArray();
                cryptoStream.Close();
            }

            memoryStream.Close();
        }

        return Convert.ToBase64String(cipherTextBytes);
    }


    public static string Decrypt(string encryptedText)
    {
        var cipherTextBytes = Convert.FromBase64String(encryptedText);
        var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
        var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.None };

        var decrypt = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(ViKey));
        var memoryStream = new MemoryStream(cipherTextBytes);
        var cryptoStream = new CryptoStream(memoryStream, decrypt, CryptoStreamMode.Read);
        var plainTextBytes = new byte[cipherTextBytes.Length];

        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        memoryStream.Close();
        cryptoStream.Close();
        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
    }

    public static string Encrypt2(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        var salt = Encoding.ASCII.GetBytes(SaltKey);
        var prf = KeyDerivationPrf.HMACSHA256;
        var iterations = 10000;
        var keyLength = 256 / 8;
        var passwordBytes = Encoding.UTF8.GetBytes(PasswordHash);

        var keyBytes = KeyDerivation.Pbkdf2(passwordBytes.ToString(), salt, prf, iterations, keyLength);

        var symmetricKey = new AesCryptoServiceProvider { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
        var encrypt = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(ViKey));

        byte[] cipherTextBytes;

        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                cipherTextBytes = memoryStream.ToArray();
                cryptoStream.Close();
            }

            memoryStream.Close();
        }

        return Convert.ToBase64String(cipherTextBytes);
    }
}