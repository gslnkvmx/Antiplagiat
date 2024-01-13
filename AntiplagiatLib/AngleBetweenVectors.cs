using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AntiplagiatLib
{
    public class AngleBetweenVectors
    {
        /// <summary>
        /// Метод который вычисляет косинус угла между векторами, тем самым находя похожесть текстов
        /// </summary>
        /// <param name="pathToFile">Путь до файла с текстом пользователя</param>
        /// <param name="pathToStandartFiles">Путь до папки, где лежат эталонные документы</param>
        public static Dictionary<string,double> Calculate(string pathToFile, string pathToStandartFiles)
        {
            // словарь в котором будет слово - количество данного слова в тексте пользователя
            Dictionary<string, int> countOfWordsInPersonFile = TFIDF.GetWords(pathToFile);
            var countOfWordsInPerson =
                countOfWordsInPersonFile.Sum(x => x.Value); // количество всех слов в пользовательском документе

            var dir = new DirectoryInfo(pathToStandartFiles); // папка в которой лежат эталонные документы

            // далее нужно вычислить TFIDF(коэффициент каждого слова) для каждого эталонного документа
            // и заодно сохранить названия имён для файлов


            var allDoc = dir.GetFiles("*.txt"); // все документы .txt в папке с эталонными
            var countOfDoc = allDoc.Length; // количество всех этих документов

            var dictionaries = new Dictionary<string, int>[countOfDoc]; // словарь к TFIDF для эталонных документов
            var filesName = new string[countOfDoc]; // будем хранить название файлов .txt, которые являются эталонными
            // теперь нужно вычислить TFIDF для папки с эталонным документами
            var count = 0;
            foreach (var currentFile in allDoc)
            {
                dictionaries[count] =
                    TFIDF.GetWords(currentFile.FullName); // для каждого документа получаем частоту слов
                filesName[count] = currentFile.Name; // сохраняем имя файла
                count++;
            }


            // теперь нужно посчитать для всей папки с эталонными документами частоту слов
            Dictionary<string, int> allDocStat = GetAllWordsInFolder(dictionaries);


            Dictionary<string, double>
                tfidfStat = GetTfidfStat(allDocStat,
                    countOfDoc); // TFIDF для каждого слова во всей папки эталонных документов

            Dictionary<string, double> finalResults = new Dictionary<string, double>();

            for (var i = 0; i < dictionaries.Length; i++)
            {
                var currentDict = dictionaries[i]; // текущий документ
                var countOfWordInStandart =
                    dictionaries[i]
                        .Sum(x => x.Value); // количество слов в текущем документу( нужно для частотности слова)

                // следующие переменные нужны для вычисления угла между векторами
                double x = 0;
                double sumFilePerson = 0;
                double sumStandardFile = 0;

                Dictionary<string, double> pair = new Dictionary<string, double>();

                foreach (var personFile in countOfWordsInPersonFile)
                {
                    if (tfidfStat.ContainsKey(personFile.Key))
                    {
                        // прибавляем сумму квадратов коэффициентов
                        sumFilePerson +=
                            Math.Pow(((double) personFile.Value / countOfWordsInPerson) * tfidfStat[personFile.Key], 2);
                        if (currentDict.ContainsKey(personFile.Key))
                        {
                            pair.Add(personFile.Key,
                                ((double) personFile.Value / countOfWordsInPerson) * tfidfStat[personFile.Key] *
                                ((double) currentDict[personFile.Key] / countOfWordInStandart) *
                                tfidfStat[personFile.Key]);
                        }
                    }
                    else
                    {
                        sumFilePerson += Math.Pow(((double) personFile.Value / countOfWordsInPerson) * 1,
                            2);
                    }
                }

                foreach (var standartFile in currentDict)
                {
                    if (tfidfStat.ContainsKey(standartFile.Key))
                    {
                        sumStandardFile +=
                            Math.Pow(
                                ((double) standartFile.Value / countOfWordInStandart) * tfidfStat[standartFile.Key], 2);
                    }
                }

                
                var lenPersonFile = Math.Sqrt(sumFilePerson);
                var lenStandartFile = Math.Sqrt(sumStandardFile);
                x = pair.Sum(z => z.Value);
                
                double angle;
                if (lenPersonFile == 0 && lenStandartFile == 0)
                {
                    angle = 1;
                }
                else if (lenPersonFile != 0 && lenStandartFile == 0)
                {
                    
                    angle = 0;
                }
                else
                {
                    angle =  x / (lenPersonFile * lenStandartFile);
                    if (angle > 1)
                    {
                        angle = 1;
                    }
                }


                finalResults.Add(filesName[i], angle);
            }

            //ShowResults(finalResults);
            return finalResults;
        }

        public static void ShowResults(Dictionary<string, double> finalResults)
        {
            Console.WriteLine("Результаты схожести с текстами");
            foreach (var current in finalResults)
            {
                Console.WriteLine(current.Key + ": " + current.Value);
            }
        }


        /// <summary>
        /// Метод, который возрващает словарь для коэффициентов слов
        /// </summary>
        /// <param name="allDocStat">частотность слова во всех документах</param>
        /// <param name="countOfDoc">количество всех документов</param>
        /// <returns></returns>
        public static Dictionary<string, double> GetTfidfStat(Dictionary<string, int> allDocStat, int countOfDoc)
        {
            Dictionary<string, double> tfidfStat = new Dictionary<string, double>();

            foreach (var current in allDocStat)
            {
                tfidfStat.Add(current.Key, Math.Log10((double) countOfDoc / current.Value));
            }

            return tfidfStat;
        }

        /// <summary>
        /// Метод, который подсчитывает количество слов во всех во всех документах
        /// </summary>
        /// <param name="dictionaries">Слова всех документов переданы в массиве</param>
        /// <returns>Возвращаем массив, где ключ - слово, а значение - количество данного слова во всех текстах</returns>
        public static Dictionary<string, int> GetAllWordsInFolder(Dictionary<string, int>[] dictionaries)
        {
            Dictionary<string, int> allDocStat = new Dictionary<string, int>();

            for (var i = 0; i < dictionaries.Length; i++)
            {
                foreach (var currentDict in dictionaries[i])
                {
                    // если это слова не было посчитано, то заходим в данный if
                    if (!allDocStat.ContainsKey(currentDict.Key))
                    {
                        var countSum = currentDict.Value;
                        for (var j = 0; j < dictionaries.Length; j++)
                        {
                            if (i != j)
                            {
                                if (dictionaries[j].ContainsKey(currentDict.Key))
                                {
                                    countSum += dictionaries[j][currentDict.Key];
                                }
                            }
                        }

                        allDocStat.Add(currentDict.Key, countSum);
                    }
                }
            }

            return allDocStat;
        }
    }
}