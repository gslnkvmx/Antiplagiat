﻿using AntiplagiatLib;

namespace Backend_application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TFIDF.AddToRefDocs(@"C:\Users\maxgu\Downloads\tot.txt");
        }
    }
}