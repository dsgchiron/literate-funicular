using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncLab8_Variant2
{
    class Program
    {
        static async Task Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Stopwatch sw = new Stopwatch();

            // 1. Послідовне (синхронне) виконання
            Console.WriteLine("=== 1. Послідовне (синхронне) виконання ===");
            sw.Start();
            int syncRes1 = CalculateSumSync(100);
            double syncRes2 = CalculateAverageSync(100);
            sw.Stop();

            Console.WriteLine($"Результат методу 1 (сума): {syncRes1}");
            Console.WriteLine($"Результат методу 2 (середнє): {syncRes2:F2}");
            Console.WriteLine($"Час послідовного виконання: {sw.ElapsedMilliseconds} мс\n");

            // 2. Асинхронне виконання
            Console.WriteLine("=== 2. Асинхронне виконання ===");
            sw.Restart();
            Task<int> asyncTask1 = CalculateSumAsync(100);
            Task<double> asyncTask2 = CalculateAverageAsync(100);

            // Очікування завершення обох задач одночасно
            await Task.WhenAll(asyncTask1, asyncTask2);
            sw.Stop();

            Console.WriteLine($"Результат методу 1 (сума): {await asyncTask1}");
            Console.WriteLine($"Результат методу 2 (середнє): {await asyncTask2:F2}");
            Console.WriteLine($"Час асинхронного виконання: {sw.ElapsedMilliseconds} мс\n");

            // 3. Обробка виключень в асинхронному методі
            Console.WriteLine("=== 3. Обробка виключень ===");
            try
            {
                // Передаємо некоректне значення (-5), щоб викликати виняток
                int invalidResult = await CalculateSumAsync(-5);
                Console.WriteLine(invalidResult);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Перехоплено виключення: {ex.Message}");
            }
        }

        // Синхронні методи (імітація тривалої операції на 1500 мс та 2000 мс)
        static int CalculateSumSync(int n)
        {
            Thread.Sleep(1500);
            return n * (n + 1) / 2;
        }

        static double CalculateAverageSync(int n)
        {
            Thread.Sleep(2000);
            return (n + 1) / 2.0;
        }

        // Асинхронні методи
        static async Task<int> CalculateSumAsync(int n)
        {
            if (n <= 0)
                throw new ArgumentException("Параметр n має бути більшим за нуль.", nameof(n));

            await Task.Delay(1500);
            return n * (n + 1) / 2;
        }

        static async Task<double> CalculateAverageAsync(int n)
        {
            if (n <= 0)
                throw new ArgumentException("Параметр n має бути більшим за нуль.", nameof(n));

            await Task.Delay(2000);
            return (n + 1) / 2.0;
        }
    }
}