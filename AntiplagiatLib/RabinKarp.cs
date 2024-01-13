    using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AntiplagiatLib
{
    public class RabinKarp: TFIDF, IAntiplagiatMethod
    {
        private static Regex clear = new Regex(
                  "(?:[^а-яА-ЯёЁa-zA-Z0-9 ]|(?<=['\"])s)",
                  RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        static public Dictionary<string, double> Calculate(string pathToFile, string pathToStandartFiles)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(pathToStandartFiles);
            if (!directoryInfo.Exists) { Console.WriteLine("Неверно указан путь!: " + pathToStandartFiles); throw new DirectoryNotFoundException(); }

            var similarityList = new Dictionary<string, double>();

            string[] dirs = Directory.GetFiles(@"C:\Users\maxgu\OneDrive\Документы\RefDocs", "*.txt", SearchOption.AllDirectories);

            const int SentencesCount = 10;

            Dictionary<string, double> SentencesTFIDF = new Dictionary<string, double>();
            Dictionary<string, HashArray[]> refDocs = new Dictionary<string, HashArray[]>();
            for (int i = 0; i < dirs.Length; ++i)
            {
                SentencesTFIDF = TFIDF.FindKeySentences(dirs[i], SentencesCount);
                //Console.WriteLine(dirs[i]);
                int j = 0;
                HashArray[] SentencesHashArr = new HashArray[SentencesCount];
                foreach (var line in SentencesTFIDF.Keys)
                {
                    SentencesHashArr[j] = new HashArray(line);

                    Console.WriteLine($"{line}... -> {SentencesTFIDF[line]}");
                    j++;
                }
                refDocs.Add(dirs[i], SentencesHashArr);
                Console.WriteLine();
            }

            var text = File.ReadAllText(pathToFile);
            text = clear.Replace(text, "").ToLower();

            HashArray text_hash = new HashArray(text);
            foreach (var refDoc in refDocs) {
                double found_count = 0.0;
                double plagiatPr = 0.0;
                for (int j = 0; j < SentencesCount; ++j)
                {
                    if (refDoc.Value[j] is null) continue;

                    List<int> pos = refDoc.Value[j].Positions(text_hash);
                    //Console.WriteLine($"{SentencesHashArr[i, j].s}:");
                    if (pos.Count > 0) {
                        found_count++;
                    }
                    
                }
                similarityList.Add(refDoc.Key, found_count / SentencesCount);
            }

            return similarityList;
        }
    }
}
