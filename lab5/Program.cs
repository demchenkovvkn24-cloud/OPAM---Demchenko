using System;
using System.Collections.Generic;
using System.Globalization;

class IntStack
{
    private int[] data;
    private int count;

    public IntStack(int capacity)
    {
        data = new int[capacity < 1 ? 1 : capacity];
    }

    public int Count { get { return count; } }
    public int Capacity { get { return data.Length; } }

    public void Push(int x)
    {
        if (count == data.Length)
        {
            int[] bigger = new int[data.Length * 2];
            for (int i = 0; i < count; i++) bigger[i] = data[i];
            data = bigger;
        }
        data[count++] = x;
    }

    public int Pop()
    {
        if (count == 0) throw new InvalidOperationException("Стек порожній");
        return data[--count];
    }

    public int Peek()
    {
        if (count == 0) throw new InvalidOperationException("Стек порожній");
        return data[count - 1];
    }
}

class Program
{
    const int N = 6;
    const int K = 3;

    static void Main()
    {
        Console.WriteLine("N = 6, K = 3, a = 0, b = 2, c = 1");

        string[] br = { "(a[b]{c})", "([)]", ")(", "((", "", "abc", "([{}]" };
        foreach (string s in br)
        {
            int e1, e2;
            bool r1 = CheckBrackets(s, out e1);
            bool r2 = CheckBracketsCustom(s, out e2);
            Console.WriteLine("\"" + s + "\" -> " + r1 + ", " + e1 + " | власний: " + r2 + ", " + e2 + " | збіг: " + (r1 == r2 && e1 == e2));
        }

        string[] rpn = { "3 4 + 2 *", "5 3 -", "8 2 /", "2 3 4 * +", "5 0 /", "2 3 ^ 4 - 5 * 5 / 1 -", "", "7", "3 x +", "3 4", "3 +" };
        foreach (string s in rpn)
        {
            double r; string err;
            Console.WriteLine("\"" + s + "\" -> " + (Rpn(s, out r, out err) ? r.ToString() : "помилка: " + err));
        }

        Simulate(8 + K % 5);
        Simulate(0);

        IntStack st = new IntStack(K % 4 + 2);
        for (int i = 1; i <= K % 4 + 3; i++)
        {
            st.Push(i);
            Console.WriteLine("Push " + i + ": Count = " + st.Count + ", Capacity = " + st.Capacity);
        }
        Console.WriteLine("Peek: " + st.Peek());
        while (st.Count > 0) st.Pop();
        try { st.Pop(); }
        catch (InvalidOperationException ex) { Console.WriteLine("Pop на порожньому: " + ex.Message); }
    }

    static bool Matches(char o, char c)
    {
        return (o == '(' && c == ')') || (o == '[' && c == ']') || (o == '{' && c == '}');
    }

    static bool CheckBrackets(string s, out int err)
    {
        Stack<int> st = new Stack<int>();
        err = -1;
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (c == '(' || c == '[' || c == '{') st.Push(i);
            else if (c == ')' || c == ']' || c == '}')
            {
                if (st.Count == 0 || !Matches(s[st.Pop()], c)) { err = i; return false; }
            }
        }
        if (st.Count == 0) return true;
        while (st.Count > 0) err = st.Pop();
        return false;
    }

    static bool CheckBracketsCustom(string s, out int err)
    {
        IntStack st = new IntStack(K % 4 + 2);
        err = -1;
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (c == '(' || c == '[' || c == '{') st.Push(i);
            else if (c == ')' || c == ']' || c == '}')
            {
                if (st.Count == 0 || !Matches(s[st.Pop()], c)) { err = i; return false; }
            }
        }
        if (st.Count == 0) return true;
        while (st.Count > 0) err = st.Pop();
        return false;
    }

    static bool Rpn(string expr, out double result, out string err)
    {
        result = 0;
        err = "";
        Stack<double> st = new Stack<double>();
        string[] tokens = expr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0) { err = "порожній вираз"; return false; }
        foreach (string t in tokens)
        {
            if (t == "+" || t == "-" || t == "*" || t == "/" || t == "^")
            {
                if (st.Count < 2) { err = "не вистачає чисел для " + t; return false; }
                double r = st.Pop(), l = st.Pop();
                if (t == "/" && r == 0) { err = "ділення на нуль"; return false; }
                st.Push(t == "+" ? l + r : t == "-" ? l - r : t == "*" ? l * r : t == "/" ? l / r : Math.Pow(l, r));
            }
            else
            {
                double n;
                if (!double.TryParse(t, NumberStyles.Float, CultureInfo.InvariantCulture, out n)) { err = "невідомий токен " + t; return false; }
                st.Push(n);
            }
        }
        if (st.Count != 1) { err = "забагато чисел"; return false; }
        result = st.Pop();
        return true;
    }

    static void Simulate(int count)
    {
        if (count <= 0) { Console.WriteLine("Заявок немає"); return; }
        Random rnd = new Random(N);
        int[] w = new int[count], arr = new int[count];
        for (int i = 0; i < count; i++) w[i] = rnd.Next(1, 10);
        arr[0] = rnd.Next(0, 5);
        for (int i = 1; i < count; i++) arr[i] = arr[i - 1] + rnd.Next(1, 9);

        Queue<int> q = new Queue<int>();
        int time = 0, next = 0, idle = 0, maxQ = 0;
        while (next < count || q.Count > 0)
        {
            if (q.Count == 0 && time < arr[next]) { idle += arr[next] - time; time = arr[next]; }
            while (next < count && arr[next] <= time) q.Enqueue(next++);
            if (q.Count > maxQ) maxQ = q.Count;
            int id = q.Dequeue();
            Console.WriteLine("Заявка " + (id + 1) + ": надійшла " + arr[id] + ", вага " + w[id] + ", початок " + time + ", кінець " + (time + w[id]));
            time += w[id];
        }
        Console.WriteLine("Макс. довжина черги: " + maxQ + ", простій: " + idle + " хв");
    }
}