using AntiplagiatLib;
using System.Diagnostics.Metrics;

namespace Backend_application
{
    internal class Program
    {
        static void Main(string[] args)
        {

            TFIDF.UploadRefDocs(@"C:\Users\maxgu\OneDrive\Документы\RefDocs");


            var result = CosineSimilarity.Calculate(
                @"C:\Users\maxgu\OneDrive\Документы\UserDocs\Текст 1.txt",
               TFIDF.DirectoryPath);

            var result2 = RabinKarp.Calculate(
                @"C:\Users\maxgu\OneDrive\Документы\UserDocs\Текст 1.txt",
               TFIDF.DirectoryPath);

            foreach (var res in result2)
            {
                Console.WriteLine(res.Key + $"-> {res.Value}");
            }
        }
    }
}
