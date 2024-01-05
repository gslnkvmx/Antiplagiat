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
            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            try
            {
                di = Directory.CreateDirectory(Path.Combine(di.FullName, "AntiplagiatDocs"));
                Console.WriteLine(di.FullName);
            }
            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine($"Ошибка при создании папки: {ex.Message}");
                return;
            }

            var fullPath = Path.Combine(di.FullName, Path.GetFileName(docPatch));
            Console.WriteLine(fullPath);

            var wordMap = TFIDF.GetWords(fullPath);

            string newFileName = "TFIDF_"+Path.GetFileName(fullPath);
            string newFilePath = Path.Combine(di.FullName, newFileName);

            using (StreamWriter writer = new StreamWriter(newFilePath, true))
            {
                foreach (string word in wordMap.Keys) writer.WriteLine($"{word}: {wordMap[word]}");
            };
        }
    }
}