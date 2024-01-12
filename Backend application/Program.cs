using AntiplagiatLib;
using System.Diagnostics.Metrics;

namespace Backend_application
{
    internal class Program
    {
        static void Main(string[] args)
        {

            TFIDF.UploadRefDocs(@"C:\Users\maxgu\OneDrive\Документы\RefDocs");

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

                    //Console.WriteLine($"{line.Substring(0, Math.Min(32, line.Length))}... -> {SentencesTFIDF[i][line]}");
                    //Console.WriteLine($"{i}, {j}");
                    //SentencesHashArr[i, j].Print();
                    j++;
                }
                Console.WriteLine();
            }
            
            
            // test pull request
            string s = "Исследование космоса";
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
            

            var result = CosineSimilarity.Calculate(
                @"C:\Users\maxgu\OneDrive\Документы\UserDocs\Текст 5.txt",
               TFIDF.DirectoryPath);
        }
    }
}
