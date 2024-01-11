namespace AntiplagiatLib
{
    public class CosineSimilarity
    {
        public static void Calculate(string pathToFile, string pathToStandartFiles)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(pathToStandartFiles);
            if (!directoryInfo.Exists) { Console.WriteLine("Тут Неверно указан путь!"); return; }

            var userFileDict = TFIDF.GetWords(pathToFile);

            var allWordsIdf = TFIDF.readDict(Path.Combine(TFIDF.DirectoryPath, "idfCoef.txt"));

            var refFiles = directoryInfo.GetFiles("TFIDF_*.txt", SearchOption.AllDirectories);

            double numOfDocs = refFiles.Length;
            foreach (FileInfo fileInfo in refFiles)
            {
                double abSum = 0.0;
                double aSquareSum = 0.0;
                double bSquareSum = 0.0;
                string docPath = fileInfo.FullName;
                var fileDict = TFIDF.readDict(docPath);
                foreach(string word in allWordsIdf.Keys) {
                    double bi = 0;
                    double ai = 0;
                    if (fileDict.ContainsKey(word)) bi = fileDict[word];
                    if (userFileDict.ContainsKey(word)) ai = userFileDict[word]* Math.Log10((numOfDocs +1.0) / (allWordsIdf[word] +1.0));

                    abSum+= bi*ai;
                    aSquareSum+= ai*ai;
                    bSquareSum+= bi*bi;
                }

                Console.WriteLine(docPath+ $" Result = {abSum/(Math.Sqrt(aSquareSum)* Math.Sqrt(bSquareSum))}" );
            }
        }

    }
}
