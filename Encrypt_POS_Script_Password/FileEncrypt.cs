using static System.GC;

namespace Encrypt_POS_Script_Password;

public partial class FileEncrypt : IDisposable
{
    private static string? _drive;
    private bool _disposed;

    public FileEncrypt(string? drive)
    {
        Drive = drive;
        FilePath = SearchDirectoryReturnFilePath();

        CreateKey();
        //FindPassword(FilePath);
    }

    public static POS_Setup_ini_Contents PosSetupIniContents { get; private set; }


    public static List<POS_Setup_ini_Contents> PosSetupIniContentsList { get; set; } = new();


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

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Dispose(true);
        SuppressFinalize(this);
    }

    #endregion

    public void Deconstruct(out bool disposed)
    {
        disposed = _disposed;
    }


    [GeneratedRegex(@"(?<=UserName=cbordsim
Password=)(.*?)*(?=\[Micros Settings\])", RegexOptions.Singleline)]
    private static partial Regex RegexPatternPassword();


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

    private static List<string> SearchDirectoryReturnFilePath()
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

        foreach (var fp in filePath)
        {
            PosSetupIniContents = new POS_Setup_ini_Contents(fp);

            PosSetupIniContents.SetFolderName();
            PosSetupIniContents.LoadFileContent();
            PosSetupIniContents.CurrentPassword = FindPassword(new[] { fp });
            PosSetupIniContentsList.Add(PosSetupIniContents);
        }

        return filePath;
    }

    private static string FindPassword(IEnumerable<string> filePaths)
    {
        foreach (var fileContent in filePaths.Select(File.ReadAllText))
        {
            var match = RegexPatternPassword().Match(fileContent);
            if (!match.Success) return null;

            Password = match.Value.TrimEnd();
        }


        return Password;
    }


    protected virtual void Dispose(bool disposing)
    {
        //SuppressFinalize(this);
        switch (_disposed)
        {
            case false:
            {
                if (disposing)
                {
                    // Dispose managed resources
                    // No managed resources to dispose in this case
                    PosSetupIniContents = null;
                    PosSetupIniContentsList = null;
                    _drive = string.Empty;
                }

                // Dispose unmanaged resources

                _disposed = true;
                break;
            }
        }
    }

    ~FileEncrypt()
    {
        bool disposed;
        Deconstruct(out disposed);
    }
}