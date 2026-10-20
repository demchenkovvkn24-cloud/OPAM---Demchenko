using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Завдання 1 ===");
        Console.Write("Введіть шлях до папки: ");
        string path = Console.ReadLine() ?? "";

        if (Directory.Exists(path))
        {
            FileInfo maxFile = FindLargestFile(new DirectoryInfo(path));
            if (maxFile != null)
            {
                Console.WriteLine($"Найбільший файл: {maxFile.Name}");
                Console.WriteLine($"Розмір: {maxFile.Length} байт");
                Console.WriteLine($"Шлях: {maxFile.FullName}");
            }
            else
            {
                Console.WriteLine("У вказаній папці немає файлів.");
            }
        }
        else
        {
            Console.WriteLine("Помилка: Вказана папка не існує.");
        }

        Console.WriteLine("\n=== Завдання 2 ===");
        Random rnd = new Random(6);
        int[] arr16 = new int[16];
        for (int i = 0; i < arr16.Length; i++)
        {
            arr16[i] = rnd.Next(-20, 21);
        }

        Console.Write("Масив: ");
        PrintArray(arr16);

        int maxRec = FindMaxRecursive(arr16, 0, arr16.Length - 1);
        Console.WriteLine($"Максимум (рекурсивно): {maxRec}");
        Console.WriteLine("Максимальна глибина рекурсії: 5");
        Console.WriteLine("Порівняння з log2(16): log2(16) = 4 (глибина викликів = log2(16) + 1 = 5)");

        Console.WriteLine("\n=== Завдання 3 ===");
        int[] arr10 = new int[10];
        for (int i = 0; i < arr10.Length; i++)
        {
            arr10[i] = rnd.Next(1, 10);
        }

        Console.Write("Початковий масив: ");
        PrintArray(arr10);

        Console.WriteLine("Знайдені підмножини із сумою 13:");
        int count = FindSubsets(arr10, 0, 13, new int[10], 0);
        Console.WriteLine($"Загальна кількість варіантів: {count}");

        Console.WriteLine("\n=== Завдання 4 ===");
        int[] arr4 = new int[] { 7, -2, 15, 4 };
        Console.Write("Тестовий масив: ");
        PrintArray(arr4);
        Console.WriteLine("Трасування:");
        FindMaxWithTrace(arr4, 0, arr4.Length - 1, 0);

        Console.WriteLine("\n=== Завдання 5 ===");
        int maxIter = FindMaxIterative(arr16);
        Console.WriteLine($"Максимум (ітеративно): {maxIter}");
        Console.WriteLine($"Результати збігаються: {maxRec == maxIter}");
    }

    static FileInfo FindLargestFile(DirectoryInfo dir)
    {
        FileInfo maxFile = null;

        try
        {
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (maxFile == null || file.Length > maxFile.Length)
                {
                    maxFile = file;
                }
            }

            DirectoryInfo[] subDirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                FileInfo subMax = FindLargestFile(subDir);
                if (subMax != null)
                {
                    if (maxFile == null || subMax.Length > maxFile.Length)
                    {
                        maxFile = subMax;
                    }
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (DirectoryNotFoundException)
        {
        }

        return maxFile;
    }

    static int FindMaxRecursive(int[] a, int lo, int hi)
    {
        if (lo == hi)
        {
            return a[lo];
        }

        int mid = (lo + hi) / 2;
        int leftMax = FindMaxRecursive(a, lo, mid);
        int rightMax = FindMaxRecursive(a, mid + 1, hi);

        return leftMax > rightMax ? leftMax : rightMax;
    }

    static int FindSubsets(int[] arr, int index, int targetSum, int[] current, int currentSize)
    {
        if (targetSum == 0)
        {
            Console.Write("[ ");
            for (int i = 0; i < currentSize; i++)
            {
                Console.Write(current[i] + " ");
            }
            Console.WriteLine("]");
            return 1;
        }

        if (index >= arr.Length || targetSum < 0)
        {
            return 0;
        }

        int count = 0;

        current[currentSize] = arr[index];
        count += FindSubsets(arr, index + 1, targetSum - arr[index], current, currentSize + 1);
        count += FindSubsets(arr, index + 1, targetSum, current, currentSize);

        return count;
    }

    static int FindMaxWithTrace(int[] a, int lo, int hi, int depth)
    {
        string indent = new string(' ', depth * 2);

        if (lo == hi)
        {
            Console.WriteLine($"{indent}enter Max({lo}..{hi}) -> {a[lo]}");
            return a[lo];
        }

        Console.WriteLine($"{indent}enter Max({lo}..{hi})");

        int mid = (lo + hi) / 2;
        int leftMax = FindMaxWithTrace(a, lo, mid, depth + 1);
        int rightMax = FindMaxWithTrace(a, mid + 1, hi, depth + 1);

        int result = leftMax > rightMax ? leftMax : rightMax;

        Console.WriteLine($"{indent}exit  Max({lo}..{hi}) -> {result}");
        return result;
    }

    static int FindMaxIterative(int[] a)
    {
        int max = a[0];
        for (int i = 1; i < a.Length; i++)
        {
            if (a[i] > max)
            {
                max = a[i];
            }
        }
        return max;
    }

    static void PrintArray(int[] arr)
    {
        Console.Write("[ ");
        for (int i = 0; i < arr.Length; i++)
        {
            Console.Write(arr[i] + (i < arr.Length - 1 ? ", " : ""));
        }
        Console.WriteLine(" ]");
    }
}