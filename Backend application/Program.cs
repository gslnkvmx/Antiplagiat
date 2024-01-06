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
        }
    }
}