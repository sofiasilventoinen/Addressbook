using Assignment_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Assignment_1.Service
{
    //Interface för dessa metoder
    internal interface IFileService
    {
        public void Save(string filepath, string text);

        public string Read(string filepath);
    }

    internal class FileService : IFileService
    {
        //Min metod för att spara
        public void Save(string filePath, string text)
        {
            try
            {
                using var sw = new StreamWriter(filePath);
                sw.WriteLine(text);
            }
            catch
            {
                //Går det ej att spara kontakten i "try" visas detta meddelande
                Console.Clear();
                Console.WriteLine("Unable to save contact");
                Console.ReadKey();
            }
        }

        //Min metod för att läsa
        public string Read(string filePath)
        {
            try
            {
                using var sr = new StreamReader(filePath);
                return sr.ReadToEnd();
            }
            catch { }

            return null!;
        }
    }
}
