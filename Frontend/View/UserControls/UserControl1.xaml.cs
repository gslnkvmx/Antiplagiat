using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AntiplagiatLib;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Windows.Controls;
using System.IO;

namespace Frontend.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>

    public partial class ChartViewModel : UserControl
    {
        public ChartViewModel()
        {
            InitializeComponent();
            this.MyModel = new PlotModel { Title = "Процент плагиата по текстам" };

            var result = CosineSimilarity.Calculate(
                @"C:\Users\maxgu\OneDrive\Документы\UserDocs\Текст 1.txt",
               TFIDF.DirectoryPath);

            var plagiatPr = result.Values.ToArray();
            var labels = result.Keys.ToArray();

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = Path.GetFileName(labels[i]);
            }
            var s2 = new BarSeries { Title = "Plagiat", StrokeColor = OxyColors.Black, StrokeThickness = 1 };

            foreach ( var pr in plagiatPr)
            {
                s2.Items.Add(new BarItem { Value = pr*100 });
            }

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
