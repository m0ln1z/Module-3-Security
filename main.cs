using System;
using System.IO;

class AffineCipher
{
    // Алфавит 26 букв
    private const int m = 26;

    // Проверка, что a и 26 взаимно простые
    static bool AreCoprime(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a == 1;
    }

    // Аффинное шифрование символа
    static char EncryptChar(char ch, int a, int b)
    {
        if (char.IsLetter(ch))
        {
            bool isUpper = char.IsUpper(ch);
            int offset = isUpper ? 'A' : 'a';

            int x = ch - offset;
            int encrypted = (a * x + b) % m;
            return (char)(encrypted + offset);
        }

        return ch; // не буквы не меняем
    }

    // Шифрование строки
    static string EncryptString(string input, int a, int b)
    {
        char[] result = new char[input.Length];

        for (int i = 0; i < input.Length; i++)
            result[i] = EncryptChar(input[i], a, b);

        return new string(result);
    }

    static void Main()
    {
        Console.Write("Введите коэффициент a (взаимно простой с 26): ");
        int a = int.Parse(Console.ReadLine());
        Console.Write("Введите коэффициент b: ");
        int b = int.Parse(Console.ReadLine());

        if (!AreCoprime(a, 26))
        {
            Console.WriteLine("Ошибка: a должно быть взаимно простым с 26!");
            return;
        }

        Console.Write("Введите путь входного файла: ");
        string inputPath = Console.ReadLine();

        Console.Write("Введите путь выходного файла: ");
        string outputPath = Console.ReadLine();

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Файл не найден!");
            return;
        }

        string text = File.ReadAllText(inputPath);
        string encryptedText = EncryptString(text, a, b);
        File.WriteAllText(outputPath, encryptedText);

        Console.WriteLine("Файл успешно зашифрован!");
    }
}
