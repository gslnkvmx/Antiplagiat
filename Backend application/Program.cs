using AntiplagiatLib;

namespace Backend_application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TFIDF.AddToRefDocs(@"C:\Users\maxgu\Downloads\tot.txt");
            //TFIDF.countTFIDF(@"C:\Users\maxgu\AppData\Roaming\AntiplagiatDocs\TFIDF_tot.txt");
            TFIDF.UploadRefDocs(@"C:\Users\maxgu\OneDrive\Документы\RefTexts");
            var sen = TFIDF.FindKeySentences(@"C:\Users\maxgu\OneDrive\Документы\RefTexts\text1.txt");
            foreach (string line in sen.Keys)
            {
                Console.WriteLine(line + $"-> {sen[line]}");
            }
        }
    }
}