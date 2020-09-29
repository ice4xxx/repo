using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Library;

namespace _03Seminar
{
    class Program
    {
        private static Journal journal { get; set; }

        private static StreamReader data { get; set; }

        static void Main(string[] args)
        {
            do
            {
                Console.Clear();

                journal = new Journal();

                if (!IsDataCorrect())
                {
                    Console.WriteLine("Проблема с файлом или некоректные данные");
                    goto End;
                }

                journal.MySerialize("out.ser");
                var spisok = Journal.MyDeserialize("out.ser");

                foreach (var pair in spisok)
                {
                    Console.WriteLine(pair.ToString());
                }

                Console.WriteLine("\n");

                foreach (var pair in spisok.GetWorst(new Random().Next(0, 6)))
                {
                    Console.WriteLine(pair.ToString());
                }

                End:
                data.Dispose();
                Console.WriteLine("\npress escpae to exit or any key to run again");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        ///// <summary>
        ///// Добавление в results данных из файла
        ///// </summary>
        ///// <returns></returns>
        //private static bool GetResults()
        //{
        //    try
        //    {
        //        data = new StreamReader("data.txt");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return false;
        //    }

        //    while (!data.EndOfStream)
        //    {
        //        string[] dataStrings = data.ReadLine().Split(' ');

        //        double average = 0;

        //        for (int i = 1; i < dataStrings.Length; i++)
        //        {
        //            average += int.Parse(dataStrings[i]);
        //        }

        //        average /= dataStrings.Length - 1;

        //        journal += new Pair<string, double>() {item2 = average, item1 = dataStrings[0]};
        //    }

        //    return true;
        //}

        /// <summary>
        /// Проверка файла на корректность и запись в Journal если данные верны.
        /// </summary>
        /// <returns></returns>
        private static bool IsDataCorrect()
        {
            try
            {
                data = new StreamReader("data.txt", Encoding.UTF8);
            }
            catch (Exception e)
            {
                return false;
            }

            while (!data.EndOfStream)
            {
                string[] dataString = data.ReadLine().Split(' ');

                for (int i = 0; i < dataString[0].Length; i++)
                {
                    if (!(dataString[0][i] >= 'а' && dataString[0][i] <= 'я' ||
                        dataString[0][i] >= 'А' && dataString[0][i] <= 'Я'))
                    {
                        return false;
                    }
                }

                for (int i = 1; i < dataString.Length; i++)
                {
                    if (!int.TryParse(dataString[i], out int tmpResult))
                    {
                        return false;
                    }

                    if (tmpResult < 0 || tmpResult > 10)
                    {
                        return false;
                    }
                }
            }

            data.Close();

            try
            {
                data = new StreamReader("data.txt");
            }
            catch (Exception e)
            {
                return false;
            }

            while (!data.EndOfStream)
            {
                string[] dataStrings = data.ReadLine().Split(' ');

                double average = 0;

                for (int i = 1; i < dataStrings.Length; i++)
                {
                    average += int.Parse(dataStrings[i]);
                }

                average /= dataStrings.Length - 1;

                journal += new Pair<string, double>() { item2 = average, item1 = dataStrings[0] };
            }

            return true;
        }
    }
}
