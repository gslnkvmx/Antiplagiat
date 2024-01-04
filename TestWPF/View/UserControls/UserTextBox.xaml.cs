using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestWPF.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UserTextBox.xaml
    /// </summary>
    public partial class UserTextBox : UserControl, INotifyPropertyChanged
    {
        public UserTextBox()
        {
            DataContext = this;
            InitializeComponent();
        }

        private string text;

        public event PropertyChangedEventHandler PropertyChanged;

        public string TextForCheck
        {
            get { return text; }
            set { 
                text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextForCheck"));
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
            txtInput.IsEnabled = true;
            txtInput.Focus();
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtInput.Text))
                tbPlaceholder.Visibility = Visibility.Visible;
            else tbPlaceholder.Visibility = Visibility.Hidden;
        }
    }
}
