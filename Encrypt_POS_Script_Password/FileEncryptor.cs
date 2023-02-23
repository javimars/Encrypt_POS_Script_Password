using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Encrypt_POS_Script_Password;


public class FileEncryptor
{
    private readonly byte[] key = new byte[0]
    {
        // Key byte array   to encrypt password 
        
        /* Your secret key here */
    };

    private readonly byte[] iv = new byte[0]
    {
        /* Your secret IV here */
    };

    public void EncryptPasswordInPOSIniFiles(string drive)
    {
        var files = Directory.GetFiles(drive, "POS-Setup.ini", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            EncryptPasswordInFile(file);
        }
    }

    public void DecryptPasswordInPOSIniFiles(string drive)
    {
        var files = Directory.GetFiles(drive, "POS-Setup.ini", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            DecryptPasswordInFile(file);
        }
    }

    private void EncryptPasswordInFile(string filePath)
    {
        var fileContent = File.ReadAllText(filePath);
        var pattern = @"(?<=Password=).*?(?=\[Micros Settings\])";
        var match = Regex.Match(fileContent, pattern, RegexOptions.Singleline);

        if (match.Success)
        {
            var password = match.Value.Trim();
            var encryptedPassword = EncryptStringToBytes_Aes(password);

            var encryptedPasswordString = Convert.ToBase64String(encryptedPassword);
            var newFileContent = Regex.Replace(fileContent, pattern, encryptedPasswordString);

            File.WriteAllText(filePath, newFileContent);
        }
    }

    private void DecryptPasswordInFile(string filePath)
    {
        var fileContent = File.ReadAllText(filePath);
        var pattern = @"(?<=Password=).*?(?=\[Micros Settings\])";
        var match = Regex.Match(fileContent, pattern, RegexOptions.Singleline);

        if (match.Success)
        {
            var encryptedPasswordString = match.Value.Trim();
            var encryptedPassword = Convert.FromBase64String(encryptedPasswordString);
            var password = DecryptStringFromBytes_Aes(encryptedPassword);

            var newFileContent = Regex.Replace(fileContent, pattern, password);

            File.WriteAllText(filePath, newFileContent);
        }
    }

    private byte[] EncryptStringToBytes_Aes(string plainText)
    {
        byte[] encrypted;

        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        return encrypted;
    }

    private string DecryptStringFromBytes_Aes(byte[] cipherText)
    {
        string plaintext = null;

        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(cipherText))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext; }
    
}
