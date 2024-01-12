using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp2
{
    public class Histogramm
    {
        public static void ShowHistogramm(Dictionary<string, double> finalResults)
        {
            // создаём форму
            var form = new Form();
            // Создаем новый элемент управления Chart
            var chart = new Chart();
            // Устанавливаем размеры элемента управления
            chart.Size = new Size(500, 300);
            // Добавляем элемент управления на форму
            form.Controls.Add(chart);
            // Создаем новую область диаграммы
            var chartArea = new ChartArea();
            // Устанавливаем максимальное значение по оси Y
            chartArea.AxisY.Maximum = 1;
            // Добавляем область диаграммы в элемент управления Chart
            chart.ChartAreas.Add(chartArea);
            // Создаем новую серию данных для гистограммы
            var series = new Series("Series");
            // Устанавливаем тип диаграммы
            series.ChartType = SeriesChartType.Column;
            
            // теперь нужно добавить данные для диаграммы
            foreach (var current in finalResults)
            {
                var name = current.Key + "\n" + current.Value;
                series.Points.AddXY(name , current.Value);
            }
            
            // Добавляем серию данных в элемент управления Chart
            chart.Series.Add(series);

            // Устанавливаем название диаграммы
            chart.Titles.Add("Final result");

            // Устанавливаем названия осей
            chartArea.AxisX.Title = "Texts";
            chartArea.AxisY.Title = "Values";
            
            // Выводим итоговую гистограмму
            form.ShowDialog();
        }
    }
}