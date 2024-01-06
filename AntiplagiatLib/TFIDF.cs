using System.Security;
using System.Text.RegularExpressions;

namespace AntiplagiatLib
{
    public class TFIDF
    {
        private static string _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AntiplagiatDocs");

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
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Нет доступа к документу! " + docPatch);
                throw;
            }
            catch (SecurityException)
            {
                Console.WriteLine("Нет доступа к документу! " + docPatch);
                throw;
            }
            catch (ArgumentException) {
                Console.WriteLine("Недопустимое имя файла! " + docPatch);
            }
            catch(DirectoryNotFoundException) {
                Console.WriteLine("Не найден файл с таким именем! "+docPatch);
            }

            //Вывод получившегося словаря
            //foreach (string word in wordMap.Keys) Console.WriteLine($"{word}: {wordMap[word]}");

            return wordMap;
        }

        public static void CreateDir()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            try
            {
                Console.WriteLine(_directoryPath);
            }
            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine($"Ошибка при создании папки: {ex.Message}");
                return;
            }
        }

        public static Dictionary<string, int> AddToRefDocs(string docPath)
        {
            var fullPath = Path.Combine(_directoryPath, Path.GetFileName(docPath));

            Dictionary<string, int> wordMap = new Dictionary<string, int>();
            try
            {
                wordMap = TFIDF.GetWords(docPath);
            }
            catch
            {
                Console.WriteLine("Не удалось добавить данный файл! " + docPath);
                throw;
            }

            string newFileName = "TFIDF_"+Path.GetFileName(fullPath);
            string newFilePath = Path.Combine(_directoryPath, newFileName);

            using (StreamWriter writer = new StreamWriter(newFilePath, false))
            {
                foreach (string word in wordMap.Keys) writer.WriteLine($"{word}: {wordMap[word]}");
            };


            Console.WriteLine("Файл добавлен: " +newFilePath);
            return wordMap;
        }

        public static void IDF()
        {

        }
    }
}