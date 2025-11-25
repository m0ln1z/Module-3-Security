using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class RSAFileEncryptor
{
    // Генерация ключей и сохранение в файлы
    static void GenerateKeys(string publicKeyPath, string privateKeyPath)
    {
        using (RSA rsa = RSA.Create(2048))
        {
            File.WriteAllText(publicKeyPath, rsa.ToXmlString(false)); // public key
            File.WriteAllText(privateKeyPath, rsa.ToXmlString(true)); // private key
        }
    }

    // Загрузка RSA из XML
    static RSA LoadKey(string path)
    {
        RSA rsa = RSA.Create();
        rsa.FromXmlString(File.ReadAllText(path));
        return rsa;
    }

    static void EncryptFile(string inputPath, string outputPath, string publicKeyPath)
    {
        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Файл не найден!");
            return;
        }

        string text = File.ReadAllText(inputPath);
        byte[] data = Encoding.UTF8.GetBytes(text);

        using (RSA rsa = LoadKey(publicKeyPath))
        {
            // RSA умеет шифровать только небольшие блоки → используем OAEP
            byte[] encrypted = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
            File.WriteAllBytes(outputPath, encrypted);
        }

        Console.WriteLine("Файл успешно зашифрован!");
    }

    static void Main()
    {
        Console.WriteLine("=== RSA Шифратор файлов ===");

        string publicKeyPath = "public.xml";
        string privateKeyPath = "private.xml";

        // Генерация ключей, если их ещё нет
        if (!File.Exists(publicKeyPath) || !File.Exists(privateKeyPath))
        {
            Console.WriteLine("Генерируем ключи RSA...");
            GenerateKeys(publicKeyPath, privateKeyPath);
        }

        Console.Write("Введите путь входного файла: ");
        string inputPath = Console.ReadLine();

        Console.Write("Введите путь выходного файла: ");
        string outputPath = Console.ReadLine();

        EncryptFile(inputPath, outputPath, publicKeyPath);
    }
}
