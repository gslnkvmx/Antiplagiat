using System.IO;
using System.Security;
using System.Text.RegularExpressions;

namespace AntiplagiatLib
{
    public class TFIDF
    {
        private static string _directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AntiplagiatDocs");

        public static string DirectoryPath { get => _directoryPath; }
        //Для удаления всеx символов кроме букв на латинице и кириллице, цифр и пробелов
        private static Regex clear = new Regex(
                  "(?:[^а-яА-ЯёЁa-zA-Z0-9 ]|(?<=['\"])s)",
                  RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public static Dictionary<string, double> readDict(string path)
        {
            Dictionary<string, double> docDict = new Dictionary<string, double>();

            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(':');

                    if (parts.Length == 2 && !string.IsNullOrEmpty(parts[0]) && !string.IsNullOrEmpty(parts[1]))
                    {
                        docDict.Add(parts[0], Double.Parse(parts[1]));
                    }
                }
            }

            return docDict;
        }

        public static void UploadRefDocs(string dirPath)
        {
            if (Directory.Exists(_directoryPath)) Directory.Delete(_directoryPath, true);
            TFIDF.CreateDir();
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
            if (!directoryInfo.Exists) { Console.WriteLine("Неверно указан путь!"); return; }
            foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.txt", SearchOption.AllDirectories))
            {
                string docPath = fileInfo.FullName;
                TFIDF.GetWords(docPath);
                TFIDF.AddToRefDocs(docPath);
            }

            var idfList = TFIDF.FormIdfList(out int numOfDocs);

            DirectoryInfo appDirectoryInfo = new DirectoryInfo(_directoryPath);
            foreach (FileInfo fileInfo in appDirectoryInfo.GetFiles("TFIDF_*.txt", SearchOption.AllDirectories))
            {
                string docPath = fileInfo.FullName;
                TFIDF.countTFIDF(docPath, idfList, numOfDocs);
            }
        }
        /// <summary>
        /// Возвращает словарь форматата [слово]: кол-во повторений данного слова в тексте
        /// При подсчете слов переводит все слова в нижний регистр и удалаяет все символы кроме букв на латинице и кириллице, цифр и пробелов
        /// </summary>
        /// <param name="docPatch">Путь к текстовому документу</param>
        /// <returns>Словарь форматата [слово]: кол-во повторений данного слова в тексте</returns>
        public static Dictionary<string, int> GetWords(string docPatch)
        {
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
            catch (ArgumentException)
            {
                Console.WriteLine("Недопустимое имя файла! " + docPatch);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Не найден файл с таким именем! " + docPatch);
            }

            //Вывод получившегося словаря
            //foreach (string word in wordMap.Keys) Console.WriteLine($"{word}: {wordMap[word]}");

            return wordMap;
        }

        public static void CreateDir()
        {

            try
            {
                Directory.CreateDirectory(_directoryPath);
                Console.WriteLine("Папка создана! " + _directoryPath);
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

            string newFileName = "TFIDF_" + Path.GetFileName(fullPath);
            string newFilePath = Path.Combine(_directoryPath, newFileName);

            using (StreamWriter writer = new StreamWriter(newFilePath, false))
            {
                foreach (string word in wordMap.Keys) writer.WriteLine($"{word}: {wordMap[word]}");
            };


            Console.WriteLine("Файл добавлен: " + newFilePath);
            return wordMap;
        }

        public static Dictionary<string, double> FormIdfList(out int numOfDocs)
        {
            Dictionary<string, double> idfList = new Dictionary<string, double>();
            numOfDocs = 0;

            DirectoryInfo directoryInfo = new DirectoryInfo(_directoryPath);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles("TFIDF_*.txt", SearchOption.AllDirectories))
            {
                using (StreamReader reader = new StreamReader(fileInfo.FullName))
                {
                    Console.WriteLine("Открыт файл для IDF: " + fileInfo.FullName);
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine()!;
                        string[] parts = line.Split(": ");

                        if (parts.Length == 2 && !string.IsNullOrEmpty(parts[0]) && !string.IsNullOrEmpty(parts[1]))
                        {
                            if (idfList.ContainsKey(parts[0])) idfList[parts[0]] += 1.0;
                            else idfList[parts[0]] = 1.0;
                        }
                    }
                }
                numOfDocs++;
            }

            string idfPath = Path.Combine(_directoryPath, "idfCoef.txt");
            var existingIdfDict = new Dictionary<string, double>();
            if (Path.Exists(idfPath)) { existingIdfDict = readDict(idfPath); };

            using (StreamWriter writer = new StreamWriter(idfPath, true))
            {
                foreach (string word in idfList.Keys) {
                    if (!existingIdfDict.ContainsKey(word))
                        writer.WriteLine($"{word}: {Math.Log10((double)numOfDocs / idfList[word])}");
                };
            }
            return idfList;
        }

        public static void countTFIDF(string docPath)
        {
            var idfList = TFIDF.FormIdfList(out int numOfDocs);
            Dictionary<string, double> docDict = readDict(docPath);

            using (StreamWriter writer = new StreamWriter(docPath, false))
            {
                foreach (string word in docDict.Keys)
                {
                    writer.WriteLine($"{word}: {Math.Log10((double)numOfDocs / idfList[word])}");
                }
            };
        }

        public static void countTFIDF(string docPath, Dictionary<string, double> idfList, int numOfDocs)
        {
            Dictionary<string, double> docDict = readDict(docPath);
   
            docDict = docDict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            using (StreamWriter writer = new StreamWriter(docPath, false))
            {
                foreach (string word in docDict.Keys)
                {
                    writer.WriteLine($"{word}: {docDict[word] * Math.Log10((double)numOfDocs / idfList[word])}");
                }
            };
        }

        public static Dictionary<string, double> FindKeySentences(string docPath, int SentencesCount)
        {
            var fullPath = Path.Combine(_directoryPath, Path.GetFileName(docPath));
            string newFileName = "TFIDF_" + Path.GetFileName(fullPath);
            string newFilePath = Path.Combine(_directoryPath, newFileName);

            Dictionary<string, double> docDict = readDict(newFilePath);
            Dictionary<string, double> TfidfSentences = new Dictionary<string, double>();

            using (StreamReader reader = new StreamReader(docPath))
            {
                // Читаем весь текст из файла
                string fileContent = reader.ReadToEnd();

                char[] separators = { '.', '!', '?' };
                // Разбиваем текст на предложения
                string[] sentences = fileContent.Split(separators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                foreach (string sentence in sentences)
                {
                    double TfidfSum = 0;
                    string strippedLine = clear.Replace(sentence, "").ToLower();
                    string[] words = strippedLine.Split(' ');

                    foreach (string word in words)
                    {
                        if(docDict.ContainsKey(word)) TfidfSum += docDict[word];
                    }
                    if (!String.IsNullOrEmpty(sentence) && 
                        !TfidfSentences.ContainsKey(sentence)) TfidfSentences.Add(sentence, TfidfSum / words.Length);
                }
            }
            TfidfSentences = TfidfSentences.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            int i = 0;
            foreach (string word in TfidfSentences.Keys)
            {
                i++;
                if (i < SentencesCount) continue;
                TfidfSentences.Remove(word);
            }
            return TfidfSentences;
            //newFileName = "Sentences_" + Path.GetFileName(fullPath);
            //newFilePath = Path.Combine(_directoryPath, newFileName);
        }
    }
}