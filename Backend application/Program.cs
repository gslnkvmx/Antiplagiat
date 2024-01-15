using AntiplagiatLib;
using System.Diagnostics.Metrics;

namespace Backend_application
{
    internal class Program
    {
        static void Main()
        {

            TFIDF.UploadRefDocs(@"C:\Users\maxgu\OneDrive\Документы\RefDocs");


            var result = CountAhoCorasik.Calculate(
                @"C:\Users\maxgu\OneDrive\Документы\UserDocs\Текст 1.txt",
               @"C:\Users\maxgu\OneDrive\Документы\RefDocs");



            foreach (var res in result)
            {
                Console.WriteLine(res.Key + $"-> {res.Value}");
            }
        }
    }
}
