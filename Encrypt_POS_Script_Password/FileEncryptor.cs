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
        string[] files = Directory.GetFiles(drive, "POS-Setup.ini", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            EncryptPasswordInFile(file);
        }
    }

    public void DecryptPasswordInPOSIniFiles(string drive)
    {
        string[] files = Directory.GetFiles(drive, "POS-Setup.ini", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            DecryptPasswordInFile(file);
        }
    }

    private void EncryptPasswordInFile(string filePath)
    {
        string fileContent = File.ReadAllText(filePath);
        string pattern = @"(?<=Password=).*?(?=\[Micros Settings\])";
        Match match = Regex.Match(fileContent, pattern, RegexOptions.Singleline);

        if (match.Success)
        {
            string password = match.Value.Trim();
            byte[] encryptedPassword = EncryptStringToBytes_Aes(password);

            string encryptedPasswordString = Convert.ToBase64String(encryptedPassword);
            string newFileContent = Regex.Replace(fileContent, pattern, encryptedPasswordString);

            File.WriteAllText(filePath, newFileContent);
        }
    }

    private void DecryptPasswordInFile(string filePath)
    {
        string fileContent = File.ReadAllText(filePath);
        string pattern = @"(?<=Password=).*?(?=\[Micros Settings\])";
        Match match = Regex.Match(fileContent, pattern, RegexOptions.Singleline);

        if (match.Success)
        {
            string encryptedPasswordString = match.Value.Trim();
            byte[] encryptedPassword = Convert.FromBase64String(encryptedPasswordString);
            string password = DecryptStringFromBytes_Aes(encryptedPassword);

            string newFileContent = Regex.Replace(fileContent, pattern, password);

            File.WriteAllText(filePath, newFileContent);
        }
    }

    private byte[] EncryptStringToBytes_Aes(string plainText)
    {
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
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

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext; }
    
}
