using Microsoft.Office.Interop.Excel;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Windows.Controls;
using AntiplagiatLib;
using System.Linq;

namespace TestWPF.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ChartViewModel : UserControl
    {
        public ChartViewModel()
        {

            InitializeComponent();

            this.MyModel = new PlotModel { Title = "Example 1" };

            var result = CosineSimilarity.Calculate(
                @"C:\Users\maxgu\OneDrive\Документы\UserDocs\Текст 5.txt",
               TFIDF.DirectoryPath);

            var plagiatPr = result.Values.ToArray();
            var labels = result.Keys.ToArray();

            var s2 = new BarSeries { Title = "Plagiat", StrokeColor = OxyColors.Black, StrokeThickness = 1 };



            MyModel.Series.Add(s2);

            MyModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CakeAxis",
                ItemsSource = labels
            });

            DataContext = this;
        }

        public PlotModel MyModel { get; private set; }
    }
}
