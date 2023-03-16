namespace Encrypt_POS_Script_Password;

public partial class FileEncrypt : IDisposable
{
    //[GeneratedRegex("\"(?<=Password=).*?(?=\\\\[Micros Settings\\\\])\"", RegexOptions.IgnoreCase, 100)]
    //public static partial Regex RegexPattern();

    private static string? _drive;
    private byte[]? Iv { get; set; }
    private byte[]? Key { get; set; }

    public FileEncrypt(string? drive)
    {
        _drive = drive;
        FilePath = SearchDirectoryReturnFilePath();

        CreateKey();
    }

    private List<string> FilePath { get; }

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
            Key = aes.Key;
        }

        // Generate a new 128-bit IV
        using (var aes = Aes.Create())
        {
            aes.KeySize = 128;
            Iv = aes.IV;
        }
    }

    public static List<string> SearchDirectoryReturnFilePath()
    {
        var filePath = new List<string>();
        try
        {
            if (_drive != null)
            {
                var txtFiles = Directory.EnumerateFiles(_drive, "POS-Setup.ini", new EnumerationOptions
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

    //public void EncryptPasswordInPosIniFiles(IEnumerable<string> filePaths)
    //{
    //    foreach (var match in filePaths.Select(File.ReadAllText).Select(fileContent => RegexPattern.Match(fileContent)))
    //    {
    //        if (!match.Success)
    //        {
    //            return;
    //        }

    //        var password = match.Value.Trim();
    //    }
    //}

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