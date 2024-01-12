using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Frontend
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private string resultProcent;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ResultProcent
        {
            get { return resultProcent; }
            set
            {
                resultProcent = value;
                OnPropertyChanged();
            }
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRollUp_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (textBox.TextForCheck.Length < 100)
            {
                MessageBox.Show("Введите текст длиной не менее 100 символов!", "Слишком короткий текст", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            */
            ResultProcent = "99%";
            originality.Visibility = Visibility.Visible;
            Console.WriteLine(ResultProcent);
        }

        private void DownloadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "TXT files | *.txt";

            bool? filePicked = fileDialog.ShowDialog();
            if (filePicked == true)
            {
                string path = fileDialog.FileName;
                //textBox.TextForCheck = "Документ загружен: " + path;
                //textBox.txtInput.IsEnabled = false;
            }
            else
            {
                //dont pick anth
            }
        }
    }
}
