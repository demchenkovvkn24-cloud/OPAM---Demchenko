using System;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int N = 6;
        int K = 3;

        int[] array = new int[24];
        Random rnd = new Random(N);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = rnd.Next(1, 5);
        }

        Console.WriteLine("Початковий масив:");
        PrintArray(array);

        Console.WriteLine("\n--- Завдання 1 ---");
        Task1(array);

        Console.WriteLine("\n--- Завдання 2 ---");
        int[] filtered = Task2(array, K % 5);
        Console.Write("Відфільтрований: ");
        PrintArray(filtered);

        Console.WriteLine("\n--- Завдання 3 ---");
        Task3(array);

        Console.WriteLine("\n--- Завдання 4 ---");
        int[] arrayForShift = new int[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            arrayForShift[i] = array[i];
        }

        Console.Write("До:    ");
        PrintArray(arrayForShift);
        Task4(arrayForShift);
        Console.Write("Після: ");
        PrintArray(arrayForShift);

        Console.WriteLine("\n--- Завдання 5 ---");
        int[,] matrix = new int[3, 6];
        Random rndM = new Random(N);
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = rndM.Next(1, 5);
            }
        }
        Task5(matrix);

        Console.ReadLine();
    }

    static void PrintArray(int[] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            Console.WriteLine("Порожній масив");
            return;
        }

        Console.Write("[ ");
        for (int i = 0; i < arr.Length; i++)
        {
            Console.Write(arr[i] + " ");
        }
        Console.WriteLine("]");
    }

    static void Task1(int[] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            Console.WriteLine("Масив порожній");
            return;
        }

        int sum = 0;
        int min = arr[0];
        int minIdx = 0;
        int max = arr[0];
        int maxIdx = 0;
        int zeros = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            sum += arr[i];

            if (arr[i] < min)
            {
                min = arr[i];
                minIdx = i;
            }

            if (arr[i] > max)
            {
                max = arr[i];
                maxIdx = i;
            }

            if (arr[i] == 0)
            {
                zeros++;
            }
        }

        double avg = (double)sum / arr.Length;

        Console.WriteLine("Сума: " + sum);
        Console.WriteLine("Середнє: " + avg);
        Console.WriteLine("Мiнiмум: " + min + " (iндекс: " + minIdx + ")");
        Console.WriteLine("Максимум: " + max + " (iндекс: " + maxIdx + ")");
        Console.WriteLine("Кiлькiсть нулiв: " + zeros);
    }

    static int[] Task2(int[] arr, int target)
    {
        if (arr == null || arr.Length == 0)
        {
            return new int[0];
        }

        int count = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] > target)
            {
                count++;
            }
        }

        int[] result = new int[count];
        int idx = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] > target)
            {
                result[idx] = arr[i];
                idx++;
            }
        }

        return result;
    }

    static void Task3(int[] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            Console.WriteLine("Масив порожній");
            return;
        }

        int maxVal = arr[0];
        int maxLen = 1;
        int maxStart = 0;

        int curVal = arr[0];
        int curLen = 1;
        int curStart = 0;

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] == curVal)
            {
                curLen++;
            }
            else
            {
                if (curLen > maxLen)
                {
                    maxLen = curLen;
                    maxVal = curVal;
                    maxStart = curStart;
                }

                curVal = arr[i];
                curLen = 1;
                curStart = i;
            }
        }

        if (curLen > maxLen)
        {
            maxLen = curLen;
            maxVal = curVal;
            maxStart = curStart;
        }

        Console.WriteLine("Значення: " + maxVal);
        Console.WriteLine("Довжина: " + maxLen);
        Console.WriteLine("Індекс: " + maxStart);
    }

    static void Task4(int[] arr)
    {
        if (arr == null || arr.Length <= 1)
        {
            return;
        }

        int temp = arr[0];
        for (int i = 0; i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }
        arr[arr.Length - 1] = temp;
    }

    static void Task5(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        Console.WriteLine("Матриця:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }
            Console.WriteLine();
        }

        int maxSum = -1;
        int maxRowIdx = 0;

        for (int i = 0; i < rows; i++)
        {
            int rSum = 0;
            for (int j = 0; j < cols; j++)
            {
                rSum += matrix[i, j];
            }

            Console.WriteLine("Сума рядка " + i + ": " + rSum);

            if (rSum > maxSum)
            {
                maxSum = rSum;
                maxRowIdx = i;
            }
        }

        Console.WriteLine("Рядок з найбільшою сумою: " + maxRowIdx);

        for (int j = 0; j < cols; j++)
        {
            int cMax = matrix[0, j];
            for (int i = 1; i < rows; i++)
            {
                if (matrix[i, j] > cMax)
                {
                    cMax = matrix[i, j];
                }
            }
            Console.WriteLine("Максимум стовпця " + j + ": " + cMax);
        }
    }
}