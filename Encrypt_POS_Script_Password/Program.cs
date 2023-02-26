// See https://aka.ms/new-console-template for more information

namespace Encrypt_POS_Script_Password;

public class Program
{
    public static void Main(string[] args)
    {
        var di = new FileEncryptor(@"e:\");
        FileEncryptor.SearchDirectoryReturnFilePath();

        Console.ReadLine();
    }
}