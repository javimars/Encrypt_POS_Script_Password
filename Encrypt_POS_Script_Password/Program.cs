// See https://aka.ms/new-console-template for more information

using Encrypt_POS_Script_Password;

public class Program
{
    public static void Main(string[] args)
    {
        var encryptor = new FileEncryptor("e:\\");
        encryptor.EncryptPasswordInPOSIniFiles();
        var path = Console.ReadLine();
    }
}