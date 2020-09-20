using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Channels;
using System.Xml.Serialization;
using Town;

namespace Program2
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Street> streetsArray =
                GetXmlStreetsDeserialize().Where(element => element.houses.Count % 2 == 1 && !element);

            foreach (var street in streetsArray)
            {
                Console.WriteLine(street.ToString());
            }

            do
            {
                Console.WriteLine("Press escape to exit");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        /// <summary>
        /// Десериализует out.ser
        /// </summary>
        /// <returns></returns>
        private static List<Street> GetXmlStreetsDeserialize()
        {
            using (FileStream fs =
                new FileStream(@"C:\Users\ice4x\Desktop\с#\repo\01 Module\02 Seminar\Homework_1\Homework_1\bin\Debug\netcoreapp3.1\out.ser", //path to out.ser
                    FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Street>));

                return serializer.Deserialize(fs) as List<Street>;
            }
        }
    }
}