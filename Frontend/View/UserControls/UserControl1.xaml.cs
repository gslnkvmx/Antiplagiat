using AntiplagiatLib;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Frontend.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>

    
    public partial class ChartViewModel : UserControl
    {

        private static int Menu()
        {
            Console.WriteLine("Выберите метод:\n" +
                "1. Косинусное сходство\n" +
                "2. Поиск ключевых предложений");
            int choise = 0;
            bool num = false;
            while (!num)
            {
                Console.WriteLine("Введите число!");
                num = Int32.TryParse(Console.ReadLine(), out choise);
                if (choise < 1 || choise > 2)
                {
                    num = false;
                    Console.WriteLine("Нет такого варианта!");
                }
            }
            return choise;
        }

        public ChartViewModel()
        {
            this.MyModel = new PlotModel { Title = "Процент плагиата по текстам" };

            string? pathToPersonFile;
            string? pathToStandartFiles;
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите путь до файла с документом, который хотите проверить на схожесть с эталонными");
                    pathToPersonFile = Console.ReadLine();
                    if (pathToPersonFile == null || !File.Exists(pathToPersonFile))
                    {
                        throw new ArgumentException();
                    }

                    Console.WriteLine("Введите путь до папки, где хранятся эталонные документы");
                    pathToStandartFiles = Console.ReadLine();

                    if (pathToStandartFiles == null || !Directory.Exists(pathToStandartFiles))
                    {
                        throw new ArgumentException();
                    }
                    break;

                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Введённый вами путь неккоректен, повторите попытку");
                }
                catch
                {
                    Console.WriteLine("Произошла непредвиденная ошибка, повторите попытку");
                }
            }

            var result = new Dictionary<string, double>();

            switch (Menu())
            {
                case (int)NameOfMethods.CosineSimilarity:
                    {
                        TFIDF.UploadRefDocs(pathToStandartFiles);
                        result = CosineSimilarity.Calculate(
                    pathToPersonFile,
                   TFIDF.DirectoryPath);
                        break;
                    }

                case (int)NameOfMethods.FindKeySentences:
                    {
                        TFIDF.UploadRefDocs(pathToStandartFiles);
                        result = RabinKarp.Calculate(
                    pathToPersonFile,
                   TFIDF.DirectoryPath);
                        break;
                    }
            }


            var plagiatPr = result.Values.ToArray();
            var labels = result.Keys.ToArray();

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = Path.GetFileName(labels[i]);
            }
            var s2 = new BarSeries { Title = "Plagiat", StrokeColor = OxyColors.Black, StrokeThickness = 1 };

            foreach (var pr in plagiatPr)
            {
                s2.Items.Add(new BarItem { Value = pr * 100 });
            }

            MyModel.Series.Add(s2);

            MyModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = labels
            });
            InitializeComponent();
            DataContext = this;
        }

        public PlotModel MyModel { get; private set; }
    }

}
