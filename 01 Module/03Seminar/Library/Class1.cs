using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Xml.Serialization;

namespace Library
{
    public class Pair<T, U> : IComparable<Pair<T, U>>
        where U : IComparable<U>
    {
        public T item1;
        public U item2;

        public int CompareTo(Pair<T, U> pair)
        {
            return this.item2.CompareTo(pair.item2);
        }

        public override string ToString()
        {
            return item1.ToString() + " " + item2.ToString();
        }
    }

    public class Journal
    {
        /// <summary>
        /// Список студентов и их оценок
        /// </summary>
        public List<Pair<string, double>> results;

        /// <summary>
        /// ctor
        /// </summary>
        public Journal()
        {
            results = new List<Pair<string, double>>();
        }

        public override string ToString()
        {
            return results[0].item1.ToString() + " " + results[0].item2.ToString();
        }

        public static Journal operator +(Journal journal, Pair<string, double> pair)
        {
            journal.results.Add(pair);
            return new Journal() {results = journal.results};
        }

        /// <summary>
        /// Перечисление results в порядке невозрастания
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            List<Pair<string, double>> sortedResults = results;
            sortedResults.Sort();
            for (int i = sortedResults.Count-1; i >= 0; i--)
            {
                yield return sortedResults[i];
            }
        }

        /// <summary>
        /// Перечисление худших студентов по оценкам
        /// </summary>
        /// <param name="N">Количество студентов для вывода</param>
        /// <returns></returns>
        public IEnumerable GetWorst(int N)
        {
            List<Pair<string, double>> sortedResults = results;
            sortedResults.Sort();
            for (int i = 0; i < (N <= sortedResults.Count ? N : sortedResults.Count); i++)
            {
                yield return sortedResults[i];
            }
        }

        /// <summary>
        /// Сериализует текущий объект класса Journal в xml
        /// </summary>
        /// <param name="path">путь до файла</param>
        public void MySerialize(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());

                serializer.Serialize(fs,this);
            }
        }

        /// <summary>
        /// Десериализует xml файл в объект класса Journal
        /// </summary>
        /// <param name="path">путь до файла</param>
        /// <returns></returns>
        public static Journal MyDeserialize(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Journal));

                return serializer.Deserialize(fs) as Journal;
            }
        }
    }
}
