using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

class Student
{
    public string Name;
    public string Group;
    public int Score;
}

class Program
{
    const int N = 6;
    const int K = 3;

    static void Main()
    {
        Console.WriteLine("N = 6, K = 3, a = 0, b = 2, c = 1");

        string text = "Алгоритм описує послідовність дій. Алгоритм можна записати словами, а можна записати кодом. "
            + "Кодом алгоритм стає тоді, коли його виконує машина, а не людина. Одна задача — багато алгоритмів.";
        WordFrequencies(text, K % 3 + 2);
        WordFrequencies("", K % 3 + 2);

        Student[] students =
        {
            S("Коваленко", "КН-3/1", 88), S("Мельник", "КН-3/2", 64),
            S("Іщенко", "КН-3/1", 93), S("Бондар", "КН-3/1", 71),
            S("Савченко", "КН-3/2", 55), S("Ткачук", "КН-3/2", 90),
            S("Кравець", "КН-3/1", 76), S("Іванів", "КН-3/1", 48),
            S("Мороз", "КН-3/2", 82), S("Шевчук", "КН-3/1", 67),
            S("Бойко", "КН-3/2", 95), S("Кушнір", "КН-3/2", 73)
        };
        GroupByRange(students);

        List<int> nums = RandomList(25, N, 1, K % 10 + 6);
        Console.WriteLine("Вихідний: " + Show(nums));
        Console.WriteLine("HashSet: " + Show(DedupHash(nums)));
        Console.WriteLine("Зі збереженням порядку: " + Show(DedupOrdered(nums)));

        List<int> a = RandomList(30, N, 1, 20 + K);
        List<int> b = RandomList(30, N + 100, 1, 20 + K);
        List<int> viaSet = OnlyInFirstHash(a, b);
        List<int> viaLoops = OnlyInFirstLoops(a, b);
        Console.WriteLine("Набір 1: " + Show(a));
        Console.WriteLine("Набір 2: " + Show(b));
        Console.WriteLine("HashSet: " + Show(viaSet));
        Console.WriteLine("Цикли: " + Show(viaLoops));
        Console.WriteLine("Збігаються: " + (viaSet.Count == viaLoops.Count && new HashSet<int>(viaSet).SetEquals(viaLoops)));

        Bench(100, false);
        Bench(1, true);
    }

    static Student S(string name, string group, int score)
    {
        Student s = new Student();
        s.Name = name;
        s.Group = group;
        s.Score = score;
        return s;
    }

    static List<int> RandomList(int count, int seed, int min, int max)
    {
        Random rnd = new Random(seed);
        List<int> list = new List<int>(count);
        for (int i = 0; i < count; i++) list.Add(rnd.Next(min, max + 1));
        return list;
    }

    static string Show(List<int> l)
    {
        StringBuilder sb = new StringBuilder("[");
        for (int i = 0; i < l.Count; i++)
        {
            if (i > 0) sb.Append(", ");
            sb.Append(l[i]);
        }
        return sb.Append("]").ToString();
    }

    static void WordFrequencies(string text, int threshold)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in text) sb.Append(char.IsLetter(c) ? char.ToLower(c) : ' ');
        string[] words = sb.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, int> freq = new Dictionary<string, int>();
        foreach (string w in words)
        {
            int c;
            if (freq.TryGetValue(w, out c)) freq[w] = c + 1;
            else freq[w] = 1;
        }

        Console.WriteLine("Слова з частотою >= " + threshold + ":");
        foreach (KeyValuePair<string, int> p in freq)
            if (p.Value >= threshold) Console.WriteLine("  " + p.Key + ": " + p.Value);
        Console.WriteLine("Різних слів: " + freq.Count);
    }

    static string RangeKey(int score)
    {
        if (score < 60) return "0-59";
        if (score < 75) return "60-74";
        if (score < 90) return "75-89";
        return "90-100";
    }

    static void GroupByRange(Student[] students)
    {
        Dictionary<string, List<Student>> groups = new Dictionary<string, List<Student>>();
        foreach (Student s in students)
        {
            string key = RangeKey(s.Score);
            if (!groups.ContainsKey(key)) groups[key] = new List<Student>();
            groups[key].Add(s);
        }

        string[] order = { "0-59", "60-74", "75-89", "90-100" };
        foreach (string key in order)
        {
            List<Student> list;
            if (!groups.TryGetValue(key, out list)) continue;
            double sum = 0;
            foreach (Student s in list) sum += s.Score;
            Console.WriteLine(key + ": студентів " + list.Count + ", середній бал " + (sum / list.Count).ToString("F1"));
        }
    }

    static List<int> DedupHash(List<int> src)
    {
        return new List<int>(new HashSet<int>(src));
    }

    static List<int> DedupOrdered(List<int> src)
    {
        HashSet<int> seen = new HashSet<int>();
        List<int> result = new List<int>();
        foreach (int x in src)
            if (seen.Add(x)) result.Add(x);
        return result;
    }

    static List<int> OnlyInFirstHash(List<int> a, List<int> b)
    {
        HashSet<int> set = new HashSet<int>(a);
        set.ExceptWith(b);
        return new List<int>(set);
    }

    static List<int> OnlyInFirstLoops(List<int> a, List<int> b)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < a.Count; i++)
        {
            bool inSecond = false;
            for (int j = 0; j < b.Count && !inSecond; j++)
                if (a[i] == b[j]) inSecond = true;

            bool already = false;
            for (int j = 0; j < result.Count && !already; j++)
                if (result[j] == a[i]) already = true;

            if (!inSecond && !already) result.Add(a[i]);
        }
        return result;
    }

    static void Row(string name, double list, double dict)
    {
        string ratio = dict > 0 ? (list / dict).ToString("F1") : "-";
        Console.WriteLine(name + " | " + list.ToString("F2") + " | " + dict.ToString("F2") + " | " + ratio);
    }

    static void Bench(int s, bool print)
    {
        Random r = new Random(1);
        Stopwatch sw = new Stopwatch();
        int hits = 0;

        int n = 100000 / s, q = 10000 / s;
        List<int> L = new List<int>(n);
        HashSet<int> H = new HashSet<int>();
        for (int i = 0; i < n; i++) { L.Add(i * 2); H.Add(i * 2); }
        sw.Restart();
        for (int j = 0; j < q; j++) if (L.Contains(j * 3)) hits++;
        double t1l = sw.Elapsed.TotalMilliseconds;
        sw.Restart();
        for (int j = 0; j < q; j++) if (H.Contains(j * 3)) hits++;
        double t1d = sw.Elapsed.TotalMilliseconds;

        string[] words = new string[50000 / s];
        for (int i = 0; i < words.Length; i++) words[i] = "w" + r.Next(2000);
        sw.Restart();
        List<string> names = new List<string>();
        List<int> counts = new List<int>();
        foreach (string w in words)
        {
            int idx = names.IndexOf(w);
            if (idx < 0) { names.Add(w); counts.Add(1); }
            else counts[idx]++;
        }
        double t2l = sw.Elapsed.TotalMilliseconds;
        sw.Restart();
        Dictionary<string, int> d = new Dictionary<string, int>();
        foreach (string w in words)
        {
            int c;
            if (d.TryGetValue(w, out c)) d[w] = c + 1;
            else d[w] = 1;
        }
        double t2d = sw.Elapsed.TotalMilliseconds;

        List<int> data = new List<int>(20000 / s);
        for (int i = 0; i < 20000 / s; i++) data.Add(r.Next(1, 10001));
        sw.Restart();
        List<int> uniq = new List<int>();
        foreach (int x in data) if (!uniq.Contains(x)) uniq.Add(x);
        double t3l = sw.Elapsed.TotalMilliseconds;
        sw.Restart();
        HashSet<int> uniqSet = new HashSet<int>(data);
        double t3d = sw.Elapsed.TotalMilliseconds;

        List<int> A = new List<int>(), B = new List<int>();
        for (int i = 0; i < 5000 / s; i++) { A.Add(r.Next(1, 10001)); B.Add(r.Next(1, 10001)); }
        sw.Restart();
        List<int> common = new List<int>();
        foreach (int x in A) if (B.Contains(x) && !common.Contains(x)) common.Add(x);
        double t4l = sw.Elapsed.TotalMilliseconds;
        sw.Restart();
        HashSet<int> inter = new HashSet<int>(A);
        inter.IntersectWith(B);
        double t4d = sw.Elapsed.TotalMilliseconds;

        if (!print) return;
        Console.WriteLine("Що міряємо | Список, мс | Словник/множина, мс | У скільки разів");
        Row("Перевірки наявності", t1l, t1d);
        Row("Частоти слів", t2l, t2d);
        Row("Дублікати", t3l, t3d);
        Row("Спільні елементи", t4l, t4d);
        Console.WriteLine("(контроль: " + hits + ", " + uniq.Count + "/" + uniqSet.Count + ", " + common.Count + "/" + inter.Count + ")");
    }
}