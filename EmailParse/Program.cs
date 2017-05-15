using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EmailParse
{
    class Program
    {
        private static List<string> _emails = new List<string>();
        private static string _filePath;
        static void Main(string[] args)
        {
           //if (args.Length == 0)
           //{
           //    Console.WriteLine("Provide a file path");
           //}
           //
           //_filePath = args[0];
            //_filePath = "Emails.txt";

            ParseFile();
        }

        public static void ParseFile()
        {
            string allText = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Emails.txt"));
            List<string> people = allText.Split(';').ToList();

            foreach (var p in people)
            {
                var cleanedString = RemoveSpecial(p);
                if (cleanedString.Contains("<"))
                {
                    AddToList(ExtractEmail(cleanedString, "<", ">"));
                    continue;
                }

                if (cleanedString.Contains("("))
                {
                    AddToList(ExtractEmail(cleanedString, "(", ")"));
                    continue;
                }

                if (cleanedString.Contains("'"))
                {
                    AddToList(ExtractEmail(cleanedString, "'", "'"));
                    continue;
                }
                AddToList(cleanedString);
            }

            foreach (var e in _emails)
            {
                Console.WriteLine(e);
            }
            Console.ReadLine();
        }

        public static string RemoveSpecial(string line)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in line)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '@' || c == '<' ||
                    c == '>' || c == '.' || c == '-' || c == '\'')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().ToLower();
        }

        public static string ExtractEmail(string str, string symbolOne, string symbolTwo)
        {
            var start = str.IndexOf(symbolOne) + 1;
            var end = str.IndexOf(symbolTwo, start);
            return str.Substring(start, end - start);
        }

        public static void AddToList(string str)
        {
            if (!_emails.Contains(str))
                _emails.Add(str);
        }
    }
}
