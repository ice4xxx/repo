using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security;
using Town;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;

namespace Lesson2
{
    class Program
    {
        //количетсво улиц
        private static int N = 0;

        private delegate bool CreateStreetArrayDelegate(out List<Street> list);

        //data.txt
        private static StreamReader dataFile;

        private static Dictionary<bool, CreateStreetArrayDelegate> CreateStreetArrays = new Dictionary<bool, CreateStreetArrayDelegate>
        {
            {true, CreateArrayOfStreets},
            {false, CreateRandomArrayOfStreets}
        };

        static void Main(string[] args)
        {
            do
            {
                Console.Clear();

                if (!IntializeDataFile())
                {
                    return;
                }

                InitializeN();

                List<Street> streetsArray = new List<Street>();

                CreateStreetArrays[IsDataCorrect()](out streetsArray);


                Console.WriteLine("Волшебные улицы:\n");
                foreach (var street in streetsArray)
                {
                    Console.WriteLine(street.ToString());
                }

                DoXmlSerialize(streetsArray);

                dataFile.Close();


                Console.WriteLine("\npress escape to exit, or press any button to run again");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }


        /// <summary>
        /// Сериализует массив
        /// </summary>
        /// <param name="list">сериализуемый массив</param>
        private static void DoXmlSerialize(List<Street> list)
        {
            File.Delete("out.ser");
            using (FileStream fs = new FileStream("out.ser", FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(list.GetType());

                serializer.Serialize(fs, list);
            }
        }

        /// <summary>
        /// Ввод числа N
        /// </summary>
        private static void InitializeN()
        {
            bool IsInputCorrect = false;
            do
            {
                Console.Write("Введите N: ");
                if (!int.TryParse(Console.ReadLine(), out N))
                {
                    Console.Clear();
                    Console.WriteLine("Ошибка ввода");
                }
                else
                {
                    IsInputCorrect = true;
                }
            } while (!IsInputCorrect);
            Console.Clear();
        }


        /// <summary>
        /// open data.txt
        /// </summary>
        /// <returns></returns>
        private static bool IntializeDataFile()
        {
            bool intialized = true;
            try
            {
                if (dataFile != null)
                {
                    dataFile.Close();
                }

                dataFile = new StreamReader(@"data.txt", Encoding.UTF8); // Encoding.GetEncoding(1251)
            }
            catch (Exception e)
            {
                Console.WriteLine("файл поврежден");
                intialized = false;
            }

            return intialized;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Рандомное название улицы</returns>
        private static string GetRandomString()
        {
            string alphbet = "ABCDEFGHIJKLMNOPQRSTUVWZYX";

            var rand = new Random();

            //количество букв в слове
            int n = rand.Next(5, 15);

            //Выбрать первую букву для названия улицы
            string answerString = alphbet[rand.Next(0, 25)].ToString();

            //Выборка последущих букв
            for (int i = 1; i < n; i++)
            {
                answerString += alphbet[rand.Next(0, 25)].ToString().ToLower();
            }

            return answerString;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Рандомный массив чисел</returns>
        private static List<int> GetRandomList()
        {
            var rand = new Random();
            List<int> answerList = new List<int>();
            int n = rand.Next(1, 10);

            for (int i = 0; i < n; i++)
            {
                answerList.Add(rand.Next(1, 100));
            }

            return answerList;

        }

        /// <summary>
        /// Метод для создание рандомного массива улиц
        /// </summary>
        /// <param name="streetsArray">массив улиц</param>
        /// <returns>true</returns>
        private static bool CreateRandomArrayOfStreets(out List<Street> streetsArray)
        {
            streetsArray = new List<Street>(N);
            for (int i = 0; i < N; i++)
            {
                streetsArray.Add(new Street(GetRandomString(), GetRandomList()));
            }

            return true;
        }


        /// <summary>
        /// Метод для создания массива улиц из data.txt
        /// </summary>
        /// <param name="streetsArray">массив улиц</param>
        /// <returns>true, если удачно открыт файл data.txt.   
        /// false, если открытие data.txt завершилось с ошибкой</returns>
        private static bool CreateArrayOfStreets(out List<Street> streetsArray)
        {
            streetsArray = new List<Street>();
            if (!IntializeDataFile())
            {
                return false;
            }

            //строка из data.txt
            string dataString;
            string[] streetString;

            for (int i = 0; i < N; i++)
            {
                dataString = dataFile.ReadLine();

                if (dataString == null)
                    break;

                streetString = dataString.Split(' ');
                List<int> houseNumberList = new List<int>();


                for (int j = 1; j < streetString.Length; j++)
                {
                    houseNumberList.Add(int.Parse(streetString[j]));
                }

                streetsArray.Add(new Street(streetString[0], houseNumberList));
            }

            return true;
        }

        /// <summary>
        /// Проверка data.txt на корректность
        /// </summary>
        /// <param name="dataFile">Входной файл</param>
        /// <returns></returns>
        private static bool IsDataCorrect()
        {
            //строка из data.txt
            string dataString;
            string[] streetString;

            while ((dataString = dataFile.ReadLine()) != null)
            {

                streetString = dataString.Split(' ');

                foreach (var c in streetString[0])
                {
                    if (!(c >= 'а' && c <= 'я' || c >= 'А' && c <= 'Я'))
                        return false;
                }

                //Есть ли дом на улице
                bool isExist = false;

                for (int i = 1; i < streetString.Length; i++)
                {
                    isExist = true;

                    int houseNumber = 0;
                    if (!int.TryParse(streetString[i], out houseNumber))
                    {
                        return false;
                    }

                    if (houseNumber > 100 || houseNumber <= 0)
                    {
                        return false;
                    }
                }

                if (!isExist)
                    return false;
            }

            return true;
        }
    }
}
