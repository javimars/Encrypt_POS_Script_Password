using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Encrypt_POS_Script_Password;

public partial class FileEncryptor : IDisposable
{
    private static readonly Regex _pattern = MyRegex();
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
            filePath.AddRange(txtFiles);
        }
        catch (Exception e)
        {
        }

        return filePath;
    }


    /*public void EncryptPasswordInPosIniFiles(List<string> FilePaths)
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
    }*/


    /*
    public void DecryptPasswordInPosIniFiles(string drive)
    {
        var files = Directory.GetFiles(drive, "POS-Setup.ini", SearchOption.AllDirectories);

        foreach (var file in files) DecryptPasswordInFile(file);
    }
    */

    /*private void EncryptPasswordInFile(string filePath)
    {
        var fileContent = File.ReadAllText(filePath);
        const string pattern = @"(?<=Password=).*?(?=\[Micros Settings\])";
        var match = MyRegex().Match(fileContent);

        if (!match.Success) return;
        var password = match.Value.Trim();
        var encryptedPassword = EncryptStringToBytes_Aes(password);

        /*var encryptedPasswordString = Convert.ToBase64String(encryptedPassword);
        var newFileContent = MyRegex().Replace(fileContent, encryptedPasswordString);#1#

        File.WriteAllText(filePath, newFileContent);
    }*/

    [GeneratedRegex("(?<=Password=).*?(?=\\[Micros Settings\\])")]
    private static partial Regex MyRegex();
}