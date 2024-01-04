using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AntiplagiatLib
{
    public class TFIDF
    {

        /// <summary>
        /// Возвращает словарь форматата [слово]: кол-во повторений данного слова в тексте
        /// При подсчете слов переводит все слова в нижний регистр и удалаяет все символы кроме букв на латинице и кириллице, цифр и пробелов
        /// </summary>
        /// <param name="docPatch">Путь к текстовому документу</param>
        /// <returns>Словарь форматата [слово]: кол-во повторений данного слова в тексте</returns>
        public static Dictionary<string, int> GetWords(string docPatch)
        {
            //Для удаления всеx символов кроме букв на латинице и кириллице, цифр и пробелов
            Regex clear = new Regex(
                  "(?:[^а-яА-ЯёЁa-zA-Z0-9 ]|(?<=['\"])s)",
                  RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

            Dictionary<string, int> wordMap = new Dictionary<string, int>();

            try
            {
                using (StreamReader streamReader = new StreamReader(docPatch))
                {
                    string? line = "0";
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        string strippedLine = clear.Replace(line, "").ToLower();

                        var words = strippedLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        foreach (string word in words)
                        {
                            if (wordMap.ContainsKey(word)) wordMap[word] += 1;
                            else wordMap[word] = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось обработать документ!", ex.Message);
                throw;
            }

            //Вывод получившегося словаря
            foreach (string word in wordMap.Keys) Console.WriteLine($"{word}: {wordMap[word]}");

            return wordMap;
        }

        public static void AddToRefDocs(string docPatch)
        {
            /*
            try
            {
                FileStream fs = File.Open(docPatch, FileMode.Open, FileAccess.Read);
                string fileName = Path.GetFileName(docPatch);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot open file!", ex.Message);
            }
            */
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(appDir);
            Console.WriteLine(Path.Combine(appDir, @"RefDocsTFIDF\" + Path.GetFileName(docPatch)));

            var wordMap = TFIDF.GetWords(docPatch);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"\RefDocsTFIDF\", Path.GetFileName(docPatch)), true))
            {
                foreach (string word in wordMap.Keys) outputFile.WriteLine($"{word}: {wordMap[word]}");
            }
        }
    }
}