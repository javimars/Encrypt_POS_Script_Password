namespace Encrypt_POS_Script_Password;

public partial class POS_Setup_ini_Contents
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

    public void SetFolderName()
    {
        var match = RegexPatternFoldername().Match(FolderNamePath);
        FolderName = match.Success ? match.Value : null;
    }


    private static string MatchFolderName(string filepath)
    {
        var match = RegexPatternFoldername().Match(filepath);
        return match.Success ? match.Value : null;
    }

    public override string ToString()
    {
        return
            $"FolderName: {FolderName}\nFolderNamePath: {FolderNamePath}\nFileContent: {FileContent}\nCurrentPassword: {CurrentPassword}\nNewPassword: {NewPassword}";
    }

    public void LoadFileContent()
    {
        if (File.Exists(FolderNamePath))
            FileContent = File.ReadAllText(FolderNamePath);
    }

    [GeneratedRegex(@"(?<=\\MEMORY\\)(.*?)(?=\\POS-Setup.ini)")]
    private static partial Regex RegexPatternFoldername();
}