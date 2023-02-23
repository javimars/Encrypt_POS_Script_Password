// See https://aka.ms/new-console-template for more information

using Encrypt_POS_Script_Password;

public class Program
{
    public static void Main(string[] args)
    {
        var path = Console.ReadLine();

        if (File.Exists(path))
            // This path is a file
            RecursiveFileProcessor.ProcessFile(path);
        else if (Directory.Exists(path))
            // This path is a directory
            RecursiveFileProcessor.ProcessDirectory(path);
        else
            Console.WriteLine("{0} is not a valid file or directory.", path);
    }
}


// encryptor = new FileEncryptor();
//encryptor.EncryptPasswordInPOSIniFiles("e:\\");