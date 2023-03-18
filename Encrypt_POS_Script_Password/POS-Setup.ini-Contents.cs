namespace Encrypt_POS_Script_Password;

public partial class POS_Setup_ini_Contents
{
    public required string FolderName
    {
        get;
        set => throw new NotImplementedException();
    }

    public required string FolderNamePath { get; set; }
    public required string FileContent { get; set; }

    public required string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }

    [GeneratedRegex(@"(?<=\\MEMORY\\)(.*?)(?=\\POS-Setup.ini)")]
    private static partial Regex RegexPatternFoldername();
}