using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

class Program
{
    static void Main()
    {
        const int N = 6;
        List<int> main = RandomList(20, N);
        Console.WriteLine("Основний: " + Show(main));

        List<int> t1 = KeepGreaterThan(main, 3);
        Console.WriteLine("Завдання 1: " + Show(t1) + " Count = " + t1.Count);

        List<int> wrong = new List<int>(main);
        RemoveMultiplesForwardWrongInPlace(wrong, 3);
        List<int> backward = new List<int>(main);
        RemoveMultiplesBackwardInPlace(backward, 3);
        List<int> rebuilt = WithoutMultiples(main, 3);
        List<int> removeAll = new List<int>(main);
        RemoveMultiplesRemoveAllInPlace(removeAll, 3);

        Console.WriteLine("Завдання 2 хибний: " + Show(wrong));
        Console.WriteLine("Завдання 2 з кінця: " + Show(backward));
        Console.WriteLine("Завдання 2 новий: " + Show(rebuilt));
        Console.WriteLine("Завдання 2 RemoveAll: " + Show(removeAll));
        Console.WriteLine("Варіанти 2 і 3 збігаються: " + SameItems(backward, rebuilt));
        Console.WriteLine("Пропущені хибним: " + Show(MultiplesOf(wrong, 3)));

        List<int> first = SortedRandom(13, N);
        List<int> second = SortedRandom(9, N + 100);
        List<int> merged = Merge(first, second);
        Console.WriteLine("Завдання 3 перший: " + Show(first));
        Console.WriteLine("Завдання 3 другий: " + Show(second));
        Console.WriteLine("Завдання 3 злитий: " + Show(merged));
        Console.WriteLine("Впорядкований: " + IsSorted(merged));

        SplitLessThan(main, 3, out List<int> less, out List<int> rest);
        Console.WriteLine("Завдання 4 менші за 3: " + Show(less));
        Console.WriteLine("Завдання 4 решта: " + Show(rest));
        Console.WriteLine("Сума Count збігається: " + (less.Count + rest.Count == main.Count));

        Benchmark();
    }

    static List<int> RandomList(int count, int seed)
    {
        Random rnd = new Random(seed);
        List<int> list = new List<int>(count);
        for (int i = 0; i < count; i++)
        {
            list.Add(rnd.Next(-9, 10));
        }
        return list;
    }

    static List<int> SortedRandom(int count, int seed)
    {
        Random rnd = new Random(seed);
        List<int> list = new List<int>(count);
        if (count == 0)
        {
            return list;
        }
        int current = rnd.Next(1, 10);
        list.Add(current);
        for (int i = 1; i < count; i++)
        {
            current += rnd.Next(1, 10);
            list.Add(current);
        }
        return list;
    }

    static List<int> KeepGreaterThan(List<int> source, int threshold)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] > threshold)
            {
                result.Add(source[i]);
            }
        }
        return result;
    }

    static void RemoveMultiplesForwardWrongInPlace(List<int> list, int divisor)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] % divisor == 0)
            {
                list.RemoveAt(i);
            }
        }
    }

    static void RemoveMultiplesBackwardInPlace(List<int> list, int divisor)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] % divisor == 0)
            {
                list.RemoveAt(i);
            }
        }
    }

    static List<int> WithoutMultiples(List<int> source, int divisor)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] % divisor != 0)
            {
                result.Add(source[i]);
            }
        }
        return result;
    }

    static void RemoveMultiplesRemoveAllInPlace(List<int> list, int divisor)
    {
        list.RemoveAll(x => x % divisor == 0);
    }

    static List<int> MultiplesOf(List<int> source, int divisor)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] % divisor == 0)
            {
                result.Add(source[i]);
            }
        }
        return result;
    }

    static bool SameItems(List<int> x, List<int> y)
    {
        if (x.Count != y.Count)
        {
            return false;
        }
        for (int i = 0; i < x.Count; i++)
        {
            if (x[i] != y[i])
            {
                return false;
            }
        }
        return true;
    }

    static List<int> Merge(List<int> a, List<int> b)
    {
        List<int> result = new List<int>(a.Count + b.Count);
        int i = 0;
        int j = 0;
        while (i < a.Count && j < b.Count)
        {
            if (a[i] <= b[j])
            {
                result.Add(a[i]);
                i++;
            }
            else
            {
                result.Add(b[j]);
                j++;
            }
        }
        while (i < a.Count)
        {
            result.Add(a[i]);
            i++;
        }
        while (j < b.Count)
        {
            result.Add(b[j]);
            j++;
        }
        return result;
    }

    static bool IsSorted(List<int> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] < list[i - 1])
            {
                return false;
            }
        }
        return true;
    }

    static void SplitLessThan(List<int> source, int threshold, out List<int> less, out List<int> rest)
    {
        less = new List<int>();
        rest = new List<int>();
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] < threshold)
            {
                less.Add(source[i]);
            }
            else
            {
                rest.Add(source[i]);
            }
        }
    }

    static void Benchmark()
    {
        const int n = 100000;
        const int searches = 1000;

        List<int> warmAdd = new List<int>();
        for (int i = 0; i < 1000; i++)
        {
            warmAdd.Add(i);
        }
        List<int> warmCap = new List<int>(1000);
        for (int i = 0; i < 1000; i++)
        {
            warmCap.Add(i);
        }
        List<int> warmInsert = new List<int>();
        for (int i = 0; i < 1000; i++)
        {
            warmInsert.Insert(0, i);
        }
        for (int i = 0; i < 10; i++)
        {
            warmAdd.Contains(-1);
        }

        Stopwatch sw = Stopwatch.StartNew();
        List<int> plain = new List<int>();
        for (int i = 0; i < n; i++)
        {
            plain.Add(i);
        }
        sw.Stop();
        Console.WriteLine("Add без місткості: " + sw.Elapsed.TotalMilliseconds + " мс");

        sw.Restart();
        List<int> sized = new List<int>(n);
        for (int i = 0; i < n; i++)
        {
            sized.Add(i);
        }
        sw.Stop();
        Console.WriteLine("Add з місткістю: " + sw.Elapsed.TotalMilliseconds + " мс");

        sw.Restart();
        List<int> front = new List<int>();
        for (int i = 0; i < n; i++)
        {
            front.Insert(0, i);
        }
        sw.Stop();
        Console.WriteLine("Insert(0, x): " + sw.Elapsed.TotalMilliseconds + " мс");

        sw.Restart();
        bool found = false;
        for (int i = 0; i < searches; i++)
        {
            found = sized.Contains(-1);
        }
        sw.Stop();
        Console.WriteLine("Contains x1000: " + sw.Elapsed.TotalMilliseconds + " мс (знайдено: " + found + ")");
    }

    static string Show(List<int> list)
    {
        StringBuilder sb = new StringBuilder("[");
        for (int i = 0; i < list.Count; i++)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }
            sb.Append(list[i]);
        }
        sb.Append("]");
        return sb.ToString();
    }
}