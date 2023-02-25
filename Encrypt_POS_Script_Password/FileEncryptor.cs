﻿using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Encrypt_POS_Script_Password;

public partial class FileEncryptor : IDisposable
{
    private static string _drive;
    private byte[] _iv;
    private byte[] _key;

    public FileEncryptor(string drive)
    {
        _drive = drive;
        FilePath = SearchDirectoryReturnFilePath();


        CreateKey();
    }

    public List<string> FilePath { get; }

    public static Regex Pattern { get; } = new(@"(?<=Password=)[\s\S]*?(?=\[Micros Settings\])");


    public void Dispose()
    {
        _drive = null;
    }

    private void CreateKey()
    {
        // Generate a new 256-bit key
        using (var aes = Aes.Create())
        {
            aes.KeySize = 256;
            _key = aes.Key;
        }

        // Generate a new 128-bit IV
        using (var aes = Aes.Create())
        {
            aes.KeySize = 128;
            _iv = aes.IV;
        }
    }

    public static List<string> SearchDirectoryReturnFilePath()
    {
        var filePath = new List<string>();
        try
        {
            var txtFiles = Directory.EnumerateFiles(_drive, "POS-Setup.ini", new EnumerationOptions
            {
                RecurseSubdirectories = true,
                AttributesToSkip = FileAttributes.System,
                IgnoreInaccessible = true
            });
            foreach (var currentFile in txtFiles) filePath.Add(currentFile);
        }
        catch (Exception e)
        {
        }

        return filePath;
    }


    public void EncryptPasswordInPosIniFiles(List<string> FilePaths)
    {
        foreach (var filePath in FilePaths)
        {
            var fileContent = File.ReadAllText(filePath);
            var match = Pattern.Match(fileContent);
            if (!match.Success)
            {
                return;
            }

            var password = match.Value.Trim();
            var encryptedPassword = EncryptStringToBytes_Aes(password);
            var encryptedPasswordString = Convert.ToBase64String(encryptedPassword);
            var newFileContent = MyRegex3().Replace(fileContent, encryptedPasswordString);

            File.WriteAllText(filePath, newFileContent);
        }
    }


    public void DecryptPasswordInPosIniFiles(string drive)
    {
        var files = Directory.GetFiles(drive, "POS-Setup.ini", SearchOption.AllDirectories);

        foreach (var file in files) DecryptPasswordInFile(file);
    }

    private void EncryptPasswordInFile(string filePath)
    {
        var fileContent = File.ReadAllText(filePath);
        const string pattern = @"(?<=Password=).*?(?=\[Micros Settings\])";
        var match = MyRegex().Match(fileContent);

        if (!match.Success) return;
        var password = match.Value.Trim();
        var encryptedPassword = EncryptStringToBytes_Aes(password);

        var encryptedPasswordString = Convert.ToBase64String(encryptedPassword);
        var newFileContent = MyRegex3().Replace(fileContent, encryptedPasswordString);

        File.WriteAllText(filePath, newFileContent);
    }

    private void DecryptPasswordInFile(string filePath)
    {
        var fileContent = File.ReadAllText(filePath);
        const string pattern = @"(?<=Password=).*?(?=\[Micros Settings\])";
        var match = MyRegex1().Match(fileContent);

        if (!match.Success) return;
        var encryptedPasswordString = match.Value.Trim();
        var encryptedPassword = Convert.FromBase64String(encryptedPasswordString);
        var password = DecryptStringFromBytes_Aes(encryptedPassword);

        var newFileContent = MyRegex2().Replace(fileContent, password);

        File.WriteAllText(filePath, newFileContent);
    }

    private byte[] EncryptStringToBytes_Aes(string plainText)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = _key;
        aesAlg.IV = _iv;

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            swEncrypt.Write(plainText);
        }

        var encrypted = msEncrypt.ToArray();

        return encrypted;
    }

    private string DecryptStringFromBytes_Aes(byte[] cipherText)
    {
        string plaintext = null;

        using var aesAlg = Aes.Create();
        aesAlg.Key = _key;
        aesAlg.IV = _iv;

        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(cipherText);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        plaintext = srDecrypt.ReadToEnd();

        return plaintext;
    }

    [GeneratedRegex("(?<=Password=).*?(?=\\[Micros Settings\\])", RegexOptions.Singleline)]
    private static partial Regex MyRegex();

    [GeneratedRegex("(?<=Password=).*?(?=\\[Micros Settings\\])", RegexOptions.Singleline)]
    private static partial Regex MyRegex1();

    [GeneratedRegex("(?<=Password=).*?(?=\\[Micros Settings\\])")]
    private static partial Regex MyRegex2();

    [GeneratedRegex("(?<=Password=).*?(?=\\[Micros Settings\\])")]
    private static partial Regex MyRegex3();
}