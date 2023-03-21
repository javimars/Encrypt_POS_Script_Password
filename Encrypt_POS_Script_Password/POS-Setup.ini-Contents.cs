namespace Encrypt_POS_Script_Password;

public partial class POS_Setup_ini_Contents : IDisposable
{
    public POS_Setup_ini_Contents(string filePath)
    {
        if (filePath != null) FolderNamePath = Path.GetFullPath(filePath);
    }

    public string FolderName { get; private set; }
    public string FolderNamePath { get; set; }
    public string FileContent { get; set; }

    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? EncryptedPassword { get; set; }

    #region IDisposable Members

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    [GeneratedRegex(@"(?<=\\MEMORY\\)(.*?)(?=\\POS-Setup.ini)")]
    private static partial Regex RegexPatternFoldername1();

    public void SetFolderName()
    {
        var match = RegexPatternFoldername1().Match(FolderNamePath);
        FolderName = match.Success ? match.Value : null;
    }


    private static string MatchFolderName(string filepath)
    {
        var match = RegexPatternFoldername1().Match(filepath);
        return match.Success ? match.Value : null;
    }

    public override string ToString()
    {
        return
            $@"FolderName: {FolderName}
FolderNamePath: {FolderNamePath}
FileContent: {FileContent}
CurrentPassword: {CurrentPassword}
NewPassword: {NewPassword}";
    }

    public void LoadFileContent()
    {
        if (File.Exists(FolderNamePath))
            FileContent = File.ReadAllText(FolderNamePath);
    }


    private void ReleaseUnmanagedResources()
    {
        CurrentPassword = null;
        EncryptedPassword = null;
        FileContent = string.Empty;
        FolderName = string.Empty;
        FolderNamePath = string.Empty;
        CurrentPassword = null;
        EncryptedPassword = null;
    }

    protected void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
    }

    ~POS_Setup_ini_Contents()
    {
        Dispose(false);
    }
}