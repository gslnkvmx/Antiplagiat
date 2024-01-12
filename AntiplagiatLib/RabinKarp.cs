using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiplagiatLib
{
    internal class RabinKarp: TFIDF, IAntiplagiatMethod
    {
        static public Dictionary<string, double> Calculate(string pathToFile, string pathToStandartFiles)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(pathToStandartFiles);
            if (!directoryInfo.Exists) { Console.WriteLine("Неверно указан путь!: " + pathToStandartFiles); throw new DirectoryNotFoundException(); }

            var similarityList = new Dictionary<string, double>();

            string[] dirs = Directory.GetFiles(@"C:\Users\maxgu\OneDrive\Документы\RefDocs", "*.*", SearchOption.AllDirectories);

            const int SentencesCount = 10;

            Dictionary<string, double>[] SentencesTFIDF = new Dictionary<string, double>[dirs.Length];
            HashArray[,] SentencesHashArr = new HashArray[dirs.Length, SentencesCount];
            for (int i = 0; i < dirs.Length; ++i)
            {
                SentencesTFIDF[i] = TFIDF.FindKeySentences(dirs[i], SentencesCount);
                //Console.WriteLine(dirs[i]);
                int j = 0;
                foreach (var line in SentencesTFIDF[i].Keys)
                {
                    SentencesHashArr[i, j] = new HashArray(line);

                    Console.WriteLine($"{line}... -> {SentencesTFIDF[i][line]}");
                    j++;
                }
                Console.WriteLine();
            }


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
        }
    }
}
