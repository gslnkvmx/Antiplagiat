using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AntiplagiatLib
{
    public class CountAhoCorasik
    {

        private static readonly Regex clear = new Regex(
                  "(?:[^а-яА-ЯёЁa-zA-Z0-9 ]|(?<=['\"])s)",
                  RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        static public Dictionary<string, double> Calculate(string pathToFile, string pathToStandartFiles)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(pathToStandartFiles);
            if (!directoryInfo.Exists) { Console.WriteLine("Неверно указан путь!: " + pathToStandartFiles); throw new DirectoryNotFoundException(); }

            var similarityList = new Dictionary<string, double>();

            string[] dirs = Directory.GetFiles(@"C:\Users\maxgu\OneDrive\Документы\RefDocs", "*.txt", SearchOption.AllDirectories);

            const int SentencesCount = 5;

            Dictionary<string, double> SentencesTFIDF;
            Dictionary<string, List<string>> refDocs = new Dictionary<string, List<string>>();
            for (int i = 0; i < dirs.Length; ++i)
            {
                SentencesTFIDF = TFIDF.FindKeySentences(dirs[i], SentencesCount);
                //Console.WriteLine(dirs[i]);
                refDocs.Add(dirs[i], SentencesTFIDF.Keys.ToList());
                foreach (var sentence in refDocs[dirs[i]]) Console.WriteLine(sentence);
                Console.WriteLine();
            }

            var text = File.ReadAllText(pathToFile);
            text = clear.Replace(text, "").ToLower();


            foreach (var refDoc in refDocs)
            {
                double found_count = 0.0;
                for (int j = 0; j < SentencesCount; ++j)
                {
                    try
                    {
                        if (refDoc.Value[j] is null) continue;
                    }
                    catch(ArgumentOutOfRangeException) { break; }

                    Trie trie = new Trie();
                    var words = refDoc.Value[j].Split(' ');
                    foreach (var word in words)
                    {
                        Console.WriteLine(word);
                        trie.Add(word);
                    }
                    trie.Build();

                    /// Ниже проверка нашли ли строку refDoc.Value[j]
                    var res = trie.Find(text);
                    bool found = true;
                    foreach (var word in words)
                    {
                        if (!res.Contains<string>(word)) found = false;
                    }
                    if (found)
                    {
                        foreach (var word in res)
                        {
                            Console.WriteLine(word);
                        }
                        found_count++;
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                similarityList.Add(refDoc.Key, found_count / SentencesCount);
            }

            return similarityList;
        }
    }
}
