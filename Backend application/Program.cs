using AntiplagiatLib;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Backend_application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            TFIDF.UploadRefDocs(@"C:\Users\maxgu\OneDrive\Документы\RefTexts");

            /*
            string[] dirs = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\texts", "*.*", SearchOption.AllDirectories);
            foreach (var item in dirs)
            {
                Console.WriteLine(item);
            }

            const int SentencesCount = 5;

            Dictionary<string, double>[] SentencesTFIDF = new Dictionary<string, double>[dirs.Length];
            HashArray[,] SentencesHashArr = new HashArray[dirs.Length, SentencesCount];
            for (int i = 0; i < dirs.Length; ++i)
            {
                SentencesTFIDF[i] = TFIDF.FindKeySentences(dirs[i], SentencesCount);

                Console.WriteLine(dirs[i]);
                int j = 0;
                foreach (var line in SentencesTFIDF[i].Keys)
                {
                    SentencesHashArr[i, j] = new HashArray(line);
                    
                    Console.WriteLine($"{line.Substring(0, Math.Min(32, line.Length))}... -> {SentencesTFIDF[i][line]}");
                    Console.WriteLine($"{i}, {j}");
                    SentencesHashArr[i, j].Print();
                    j++;
                }
                Console.WriteLine();
            }
            
            // test pull request
            string s = "засмеялся Густав";
            HashArray s_h = new HashArray(s);
            for (int i = 0; i < SentencesHashArr.GetLength(0); ++i)
            {
                for (int j = 0; j < SentencesCount; ++j)
                {
                    if (SentencesHashArr[i, j] is null) continue;

                    List<int> pos = s_h.Positions(SentencesHashArr[i, j]);
                    Console.WriteLine($"{SentencesHashArr[i, j].s}:");
                    foreach (int k in pos)
                    {
                        Console.WriteLine(k);
                    }
                    Console.WriteLine();
                }
            }
            */
            
            CosineSimilarity.Calculate(
                @"C:\Users\maxgu\OneDrive\Документы\UserTexts\Текст 5.txt",
               TFIDF.DirectoryPath);
        }
    }
}
