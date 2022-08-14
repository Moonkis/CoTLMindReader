using System;
using System.IO;
using System.Security.Cryptography;

namespace CoTL_MindReader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Too few arguments supplied.");
                Console.WriteLine("Specify (E)ncryption or (D)ecryption as well as file-path.");
                Console.WriteLine("Example:");
                Console.WriteLine("CoTL Mindreader.exe E <PATH_TO_FILE_TO_ENCRYPT>");
                Console.WriteLine("CoTL Mindreader.exe D <PATH_TO_FILE_TO_DECRYPT>");
            }

            switch(args[0].ToUpper().Trim())
            {
                case "E": Encrypt(args[1]); break;
                case "D": Decrypt(args[1]); break;
                default:
                    Console.WriteLine($"Invalid argument supplied {args[0]}, expected \"D\" or \"E\"");
                    break;
            }
        }


        private static void Decrypt(string path)
        {
            string result = string.Empty;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                byte encrypted = (byte)fs.ReadByte();
                if (encrypted != 'E')
                {
                    Console.WriteLine("Failed to decrypt file, because file was not encrypted.");
                    return;
                }

                byte[] encryptionKey = new byte[16];
                fs.Read(encryptionKey, 0, encryptionKey.Length);

                using (Aes aes = Aes.Create())
                {
                    byte[] initVector = new byte[aes.IV.Length];
                    int length = aes.IV.Length;
                    int offset = 0;

                    while(length > 0)
                    {
                        int o = fs.Read(initVector, offset, length);
                        if (o == 0)
                        {
                            break;
                        }

                        offset += o;
                        length -= o;
                    }

                    using (CryptoStream crypto = new CryptoStream(fs, aes.CreateDecryptor(encryptionKey, initVector), CryptoStreamMode.Read))
                    using (StreamReader sr = new StreamReader(crypto))
                    {
                        result = sr.ReadToEnd();
                        Console.WriteLine("Finished writing decrypted file.");
                    }
                }
            }

            if (!string.IsNullOrEmpty(result))
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(result);
                }
            }
        }

        private static void Encrypt(string path)
        {
            string content = File.ReadAllText(path);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                // Write the encryption flag 'E'
                fs.WriteByte(69);

                // Write the 16 byte initialization vector for AES
                Random rnd = new Random();
                byte[] initVector = new byte[16];
                rnd.NextBytes(initVector);

                fs.Write(initVector, 0, initVector.Length);

                // Encrypt the rest of the content with the initialization vector
                using (Aes aes = Aes.Create())
                {
                    aes.Key = initVector;
                    fs.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream crypto = new CryptoStream(fs, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(crypto))
                    {
                        sw.Write(content);
                    }
                }
            }
        }
    }
}
