namespace Encrypt_POS_Script_Password;

public class RecursiveFileProcessor
{
    // Process all files in the directory passed in, recurse on any directories
    // that are found, and process the files they contain.
    public static void ProcessDirectory(string targetDirectory)
    {
        // Process the list of files found in the directory.
        var fileEntries = Directory.GetFiles(targetDirectory);
        foreach (var fileName in fileEntries)
            ProcessFile(fileName);

        // Recurse into subdirectories of this directory.
        var subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (var subdirectory in subdirectoryEntries)
            ProcessDirectory(subdirectory);
    }

    // Insert logic for processing found files here.
    public static void ProcessFile(string path)
    {
        Console.WriteLine("Processed file '{0}'.", path);
    }
}