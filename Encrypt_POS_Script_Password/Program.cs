// See https://aka.ms/new-console-template for more information

using Encrypt_POS_Script_Password;

Console.WriteLine("Hello, World!");
var encryptor = new FileEncryptor();
encryptor.EncryptPasswordInPOSIniFiles("d:\\");
