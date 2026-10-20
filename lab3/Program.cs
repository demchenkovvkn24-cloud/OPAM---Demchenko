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
                Console.WriteLine($"Найбільший файл: {maxFile.Name} ({maxFile.Length} байт)\nШлях: {maxFile.FullName}");
            else
                Console.WriteLine("У папці немає файлів.");
        }
        else Console.WriteLine("Папка не існує.");

        Console.WriteLine("\n=== Завдання 2 ===");
        Random rnd = new Random(6);
        int[] arr16 = new int[16];
        for (int i = 0; i < 16; i++) arr16[i] = rnd.Next(-20, 21);

        Console.Write("Масив: ");
        PrintArray(arr16);

        int maxRec = FindMaxRecursive(arr16, 0, 15);
        Console.WriteLine($"Максимум (рекурсивно): {maxRec}");
        Console.WriteLine("Глибина рекурсії: 5 (log2(16) + 1)");

        Console.WriteLine("\n=== Завдання 3 ===");
        int[] arr10 = new int[10];
        for (int i = 0; i < 10; i++) arr10[i] = rnd.Next(1, 10);

        Console.Write("Масив: ");
        PrintArray(arr10);
        Console.WriteLine("Підмножини із сумою 13 (перші варіанти):");
        int printedCount = 0;
        int count = FindSubsets(arr10, 0, 13, new int[10], 0, ref printedCount);
        Console.WriteLine($"Загальна кількість варіантів: {count}");

        Console.WriteLine("\n=== Завдання 4 ===");
        int[] arr4 = new int[] { 7, -2, 15, 4 };
        Console.Write("Масив: ");
        PrintArray(arr4);
        Console.WriteLine("Трасування:");
        FindMaxWithTrace(arr4, 0, 3, 0);

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
            foreach (FileInfo file in dir.GetFiles())
                if (maxFile == null || file.Length > maxFile.Length) maxFile = file;

            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                FileInfo subMax = FindLargestFile(subDir);
                if (subMax != null && (maxFile == null || subMax.Length > maxFile.Length))
                    maxFile = subMax;
            }
        }
        catch (UnauthorizedAccessException) { }
        catch (DirectoryNotFoundException) { }

        return maxFile;
    }

    static int FindMaxRecursive(int[] a, int lo, int hi)
    {
        if (lo == hi) return a[lo];
        int mid = (lo + hi) / 2;
        int left = FindMaxRecursive(a, lo, mid);
        int right = FindMaxRecursive(a, mid + 1, hi);
        return left > right ? left : right;
    }

    static int FindSubsets(int[] arr, int index, int targetSum, int[] current, int currentSize, ref int printedCount)
    {
        if (targetSum == 0)
        {
            if (printedCount < 5)
            {
                Console.Write("[ ");
                for (int i = 0; i < currentSize; i++) Console.Write(current[i] + " ");
                Console.WriteLine("]");
                printedCount++;
            }
            return 1;
        }

        if (index >= arr.Length || targetSum < 0) return 0;

        current[currentSize] = arr[index];
        int count = FindSubsets(arr, index + 1, targetSum - arr[index], current, currentSize + 1, ref printedCount);
        count += FindSubsets(arr, index + 1, targetSum, current, currentSize, ref printedCount);

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
        int left = FindMaxWithTrace(a, lo, mid, depth + 1);
        int right = FindMaxWithTrace(a, mid + 1, hi, depth + 1);
        int res = left > right ? left : right;
        Console.WriteLine($"{indent}exit  Max({lo}..{hi}) -> {res}");
        return res;
    }

    static int FindMaxIterative(int[] a)
    {
        int max = a[0];
        for (int i = 1; i < a.Length; i++)
            if (a[i] > max) max = a[i];
        return max;
    }

    static void PrintArray(int[] arr)
    {
        Console.Write("[ ");
        for (int i = 0; i < arr.Length; i++) Console.Write(arr[i] + (i < arr.Length - 1 ? ", " : ""));
        Console.WriteLine(" ]");
    }
}