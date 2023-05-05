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

namespace MyDataSafe.Windows
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((LoginName.Text == "Vašek") && (LoginPass.Password == "Ahoj123!"))
            {
                MainWindow MW = new MainWindow();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong login!","Error",MessageBoxButton.OK, MessageBoxImage.Error);
                LoginPass.Password = string.Empty;
            }
        }
    }
}
