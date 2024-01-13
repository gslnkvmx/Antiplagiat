namespace AntiplagiatLib
{
    public class CosineSimilarity: IAntiplagiatMethod
    {
        private static string _path;
        public static string UserPath { get { return _path; } set { _path = value; } }
        public static Dictionary<string, double> Calculate(string pathToFile, string pathToStandartFiles)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(pathToStandartFiles);
            if (!directoryInfo.Exists) { Console.WriteLine("Неверно указан путь!: " + pathToStandartFiles); throw new DirectoryNotFoundException(); }

            var similarityList = new Dictionary<string, double>();

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
                //Console.WriteLine(docPath);
                foreach (string word in allWordsIdf.Keys) {
                    //Console.WriteLine(word);
                    double bi = 0;
                    double ai = 0;
                    if (fileDict.ContainsKey(word)) bi = fileDict[word];
                    if (userFileDict.ContainsKey(word)) ai = userFileDict[word]*allWordsIdf[word];
                    //Console.WriteLine($"ai: {ai}, bi: {bi}");
                    abSum += bi*ai;
                    aSquareSum+= ai*ai;
                    bSquareSum+= bi*bi;
                }

                Console.WriteLine(docPath+ $" Result = {abSum/(Math.Sqrt(aSquareSum)* Math.Sqrt(bSquareSum))}" );
                similarityList.Add(docPath, abSum / (Math.Sqrt(aSquareSum) * Math.Sqrt(bSquareSum)));
            }

            return similarityList;
        }

        public static Dictionary<string, double> Calculate()
        {
            string pathToStandartFiles = TFIDF.DirectoryPath;
            string pathToFile = UserPath;
            DirectoryInfo directoryInfo = new DirectoryInfo(pathToStandartFiles);
            if (!directoryInfo.Exists) { Console.WriteLine("Неверно указан путь!: " + pathToStandartFiles); throw new DirectoryNotFoundException(); }

            var similarityList = new Dictionary<string, double>();

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
                //Console.WriteLine(docPath);
                foreach (string word in allWordsIdf.Keys)
                {
                    //Console.WriteLine(word);
                    double bi = 0;
                    double ai = 0;
                    if (fileDict.ContainsKey(word)) bi = fileDict[word];
                    if (userFileDict.ContainsKey(word)) ai = userFileDict[word] * allWordsIdf[word];
                    //Console.WriteLine($"ai: {ai}, bi: {bi}");
                    abSum += bi * ai;
                    aSquareSum += ai * ai;
                    bSquareSum += bi * bi;
                }

                Console.WriteLine(docPath + $" Result = {abSum / (Math.Sqrt(aSquareSum) * Math.Sqrt(bSquareSum))}");
                similarityList.Add(docPath, abSum / (Math.Sqrt(aSquareSum) * Math.Sqrt(bSquareSum)));
            }

            return similarityList;
        }
    }
}
