using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace NewEra_Bot
{
    /// <summary>
    /// Логика взаимодействия для ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : Window
    {
        public ModalWindow(string Text)
        {
            InitializeComponent();
            NameField.Content = Text;

        }

        public string GetValue()
        {
            if (Value.Text == null)
                return "123";
            return Value.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Value.Text == "")
                MessageBox.Show("Введите значение поля");
            else
                this.DialogResult = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Value.Text == "")
            {
                MessageBox.Show("Введите значение поля");
                e.Cancel = true;
            }
        }
    }
}
