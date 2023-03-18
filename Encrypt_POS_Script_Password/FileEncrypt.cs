namespace Encrypt_POS_Script_Password;

public partial class FileEncrypt : IDisposable
{
    private static string? _drive;

    public FileEncrypt(string? drive)
    {
        Drive = drive;
        FilePath = SearchDirectoryReturnFilePath();

        CreateKey();
        FindPassword(FilePath);
    }

    public static POS_Setup_ini_Contents PosSetupIniContents { get; set; } = new()
    {
        FileContent = null,
        CurrentPassword = null
    };


    public static string Password { get; set; }

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

    private static List<string> FilePath { get; set; }

    #region IDisposable Members

    public void Dispose()
    {
        Drive = null!;
    }

    #endregion

    [GeneratedRegex(@"(?<=UserName=cbordsim
Password=)(.*?)*(?=\[Micros Settings\])", RegexOptions.Singleline)]
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

    public static string FindPassword(IEnumerable<string> filePaths)
    {
        foreach (var fileContent in filePaths.Select(File.ReadAllText))
        {
            var match = RegexPattern().Match(fileContent);
            if (!match.Success) return null;

            Password = match.Value.TrimEnd();
        }


        return Password;
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