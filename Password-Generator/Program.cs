using System;
using System.Text;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Меню:");
            Console.WriteLine("1) Перевiрити надiйнiсть вашого пароля");
            Console.WriteLine("2) Сгенерувати пароль");
            Console.WriteLine("3) Вихiд");
            Console.Write("Введiть номер опцiї: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CheckPassword();
                    break;
                case "2":
                    GeneratePasswords();
                    break;
                case "3":
                    Console.WriteLine("Вихiд з програми...");
                    return;
                default:
                    Console.WriteLine("Некоректний ввiд. Спробуйте ще раз.");
                    Thread.Sleep(1500);
                    break;
            }
        }
    }

    static void CheckPassword()
    {
        Console.Clear();
        Console.WriteLine("Перевiрка надiйнiсть пароля");
        Console.Write("Введiть ваш пароль: ");
        string password = Console.ReadLine();

        string strength = EvaluatePasswordStrength(password);
        Console.WriteLine($"Надiйнiсть пароля: {strength}");
        ShowPasswordStrengthBar(strength);

        double entropy = CalculatePasswordEntropy(password);
        Console.WriteLine($"Ентропiя пароля: {entropy:N2} бiт");

        double bruteForceTimeInSeconds = CalculateBruteForceTime(password, DetermineLevel(password));
        Console.WriteLine($"Приблизний час для брутфорсу:");
        Console.WriteLine("1) На звичайному ПК");
        Console.WriteLine("2) На квантовому комп'ютерi");
        Console.Write("Виберiть тип обчислень (1 або 2): ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.WriteLine($"- На звичайному ПК: {CalculateBruteForceHumanReadable(bruteForceTimeInSeconds)}");
        }
        else if (choice == "2")
        {
            double quantumTimeInSeconds = bruteForceTimeInSeconds / 100_000_000;
            Console.WriteLine($"- На квантовому комп'ютерi: {CalculateBruteForceHumanReadable(quantumTimeInSeconds)}");
        }
        else
        {
            Console.WriteLine("Некоректний ввiд. Повернiться в меню.");
        }

        Console.WriteLine("\nНатиснiть будь-яку клавiшу для повернення в меню...");
        Console.ReadKey();
    }

    static void GeneratePasswords()
    {
        Console.Clear();
        Console.WriteLine("Генерацiя пароля");
        Console.Write("Введiть довжину пароля: ");
        int length = int.Parse(Console.ReadLine());

        Console.WriteLine("Виберiть рiвень складностi:");
        Console.WriteLine("1 - Тiльки цифри");
        Console.WriteLine("2 - Цифри i лiтери");
        Console.WriteLine("3 - Цифри, лiтери та спецсимволи");
        Console.Write("Ваш вибiр: ");
        int level = int.Parse(Console.ReadLine());

        string password = GeneratePassword(level, length);
        Console.WriteLine("\nВаш згенерований пароль:");
        ShowPasswordWithAnimation(password);

        string strength = EvaluatePasswordStrength(password);
        Console.WriteLine($"Надiйнiсть пароля: {strength}");
        ShowPasswordStrengthBar(strength);

        double entropy = CalculatePasswordEntropy(password);
        Console.WriteLine($"Ентропiя пароля: {entropy:N2} бiт");

        double bruteForceTimeInSeconds = CalculateBruteForceTime(password, level);
        Console.WriteLine($"Приблизний час для брутфорсу:");
        Console.WriteLine("1) На звичайному ПК");
        Console.WriteLine("2) На квантовому комп'ютерi");
        Console.Write("Виберiть тип обчислень (1 або 2): ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.WriteLine($"- На звичайному ПК: {CalculateBruteForceHumanReadable(bruteForceTimeInSeconds)}");
        }
        else if (choice == "2")
        {
            double quantumTimeInSeconds = bruteForceTimeInSeconds / 100_000_000;
            Console.WriteLine($"- На квантовому комп'ютерi: {CalculateBruteForceHumanReadable(quantumTimeInSeconds)}");
        }
        else
        {
            Console.WriteLine("Некоректний ввiд. Повернiться в меню.");
        }

        Console.WriteLine("\nНатиснiть будь-яку клавiшу для повернення в меню...");
        Console.ReadKey();
    }

    static string GeneratePassword(int level, int length)
    {
        const string digits = "0123456789";
        const string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string specialChars = "!@#$%^&*()_+[]{}|;:,.<>?/";

        string chars = digits;
        if (level == 2) chars += letters;
        else if (level == 3) chars += letters + specialChars;

        StringBuilder password = new StringBuilder();
        Random rnd = new Random();
        for (int i = 0; i < length; i++)
        {
            password.Append(chars[rnd.Next(chars.Length)]);
        }
        return password.ToString();
    }

    static void ShowPasswordWithAnimation(string password)
    {
        Random rnd = new Random();
        StringBuilder display = new StringBuilder(new string('_', password.Length));
        ConsoleColor[] colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));

        for (int i = 0; i < password.Length; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Console.ForegroundColor = colors[rnd.Next(colors.Length)];
                display[i] = (char)rnd.Next(33, 126);
                Console.Write($"\r{display}");
                Thread.Sleep(30);
            }
            display[i] = password[i];
            Console.Write($"\r{display}");
        }
        Console.WriteLine();
        Console.ResetColor();
    }

    static string EvaluatePasswordStrength(string password)
    {
        int score = 0;
        if (password.Length >= 8) score++;
        if (password.IndexOfAny("0123456789".ToCharArray()) != -1) score++;
        if (password.IndexOfAny("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()) != -1) score++;
        if (password.IndexOfAny("!@#$%^&*()_+[]{}|;:,.<>?/".ToCharArray()) != -1) score++;

        switch (score)
        {
            case 4: return "Сильний";
            case 3: return "Середнiй";
            case 2: return "Слабкий";
            default: return "Дуже слабкий";
        }
    }

    static int DetermineLevel(string password)
    {
        if (password.IndexOfAny("!@#$%^&*()_+[]{}|;:,.<>?/".ToCharArray()) != -1) return 3;
        if (password.IndexOfAny("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()) != -1) return 2;
        return 1;
    }

    static void ShowPasswordStrengthBar(string strength)
    {
        string bar;
        if (strength == "Дуже слабкий") bar = "[░░░░░░]";
        else if (strength == "Слабкий") bar = "[██░░░░░]";
        else if (strength == "Середнiй") bar = "[████░░░]";
        else if (strength == "Сильний") bar = "[██████░]";
        else bar = "[░░░░░░░]";

        Console.WriteLine($"Надiйнiсть: {bar}");
    }

    static double CalculatePasswordEntropy(string password)
    {
        int charsetSize = 10;
        if (password.IndexOfAny("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()) != -1) charsetSize = 62;
        if (password.IndexOfAny("!@#$%^&*()_+[]{}|;:,.<>?/".ToCharArray()) != -1) charsetSize = 94;

        double entropy = password.Length * Math.Log(charsetSize, 2);
        return entropy;
    }

    static double CalculateBruteForceTime(string password, int level)
    {
        int charsetSize = 10;  
        if (level == 2) charsetSize = 62;  
        else if (level == 3) charsetSize = 94;  

        double combinations = Math.Pow(charsetSize, password.Length);
        const double guessesPerSecond = 100_000_000;  

         
        double timeInSeconds = combinations / guessesPerSecond;

         
        if (timeInSeconds >= double.MaxValue)
        {
            return double.MaxValue;  
        }

        return timeInSeconds;
    }

    static string CalculateBruteForceHumanReadable(double seconds)
    {
        if (seconds >= double.MaxValue || seconds < 0)
        {
            return "Неможливо обчислити, занадто довго!";
        }

        if (seconds < 60) return $"{seconds:N2} секунд";
        if (seconds < 3600)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return $"{time.Minutes} хвилин {time.Seconds} секунд";
        }
        if (seconds < 86400)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return $"{time.Hours} годин {time.Minutes} хвилин";
        }
        if (seconds < 31536000)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return $"{time.Days} днiв {time.Hours} годин";
        }

        int years = (int)(seconds / 31536000);
        int days = (int)((seconds % 31536000) / 86400);
        return $"{years} рокiв {days} днiв";
    }
}
