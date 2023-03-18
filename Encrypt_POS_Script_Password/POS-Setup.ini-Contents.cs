namespace Encrypt_POS_Script_Password;

public record POS_Setup_ini_Contents
{
    public required string? FileContent { get; set; }
    public required string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}