using System.Diagnostics.CodeAnalysis;

namespace Encrypt_POS_Script_Password;

public partial class FileEncrypt : IDisposable
{
    private static string? _drive;

    public FileEncrypt(string? drive)
    {
        Drive = drive;
        FilePath = SearchDirectoryReturnFilePath();

        CreateKey();
    }

    [DisallowNull]
    private static string? Drive
    {
        get => _drive;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value cannot be null or empty.", nameof(value));
            _drive = value;
        }
    }

    private List<string> FilePath { get; }

    #region IDisposable Members

    public void Dispose()
    {
        Drive = null;
    }

    #endregion

    [GeneratedRegex("\"(?<=Password=).*?(?=\\\\[Micros Settings\\\\])\"", RegexOptions.IgnoreCase)]
    private static partial Regex RegexPattern();


    private static void CreateKey()
    {
        // Generate a new 256-bit key
        using (var aes = Aes.Create())
        {
            aes.KeySize = 256;
        }

        // Generate a new 128-bit IV
        using (var aes = Aes.Create())
        {
            aes.KeySize = 128;
        }
    }

    public static List<string> SearchDirectoryReturnFilePath()
    {
        var filePath = new List<string>();
        try
        {
            if (Drive != null)
            {
                var txtFiles = Directory.EnumerateFiles(Drive, "POS-Setup.ini", new EnumerationOptions
                {
                    RecurseSubdirectories = true,
                    AttributesToSkip = FileAttributes.System,
                    IgnoreInaccessible = true
                });
                filePath.AddRange(txtFiles);
            }
        }
        catch (Exception e)
        {
            // ignored
        }

        return filePath;
    }

    public static void EncryptPasswordInPosIniFiles(IEnumerable<string> filePaths)
    {
        foreach (var match in filePaths.Select(File.ReadAllText)
                     .Select(fileContent => RegexPattern().Match(fileContent)))
        {
            if (!match.Success) return;

            var password = match.Value.Trim();
        }
    }

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
}