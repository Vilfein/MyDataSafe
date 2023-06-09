﻿using MyDataSafe.Model;
using MyDataSafe.ViewModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyDataSafe.Windows
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        LoginViewModel LVM;

        public LoginPage()
        {
            LVM = new LoginViewModel();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LVM.LoadSetting().GetAwaiter().OnCompleted(() =>
            {             
                var name = LoginName.Text;
                var pass = LoginPass.Password;

                if ((name == LVM.LoginName) && (pass == LVM.LoginPassword))
                {
                    MainWindow MW = new MainWindow();
                    MW.Loaded += (s, e) => Close();
                    MW.Show();

                }

                else
                {
                    MessageBox.Show("Wrong login!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    LoginPass.Password = string.Empty;
                }
            });
        }
    }
}
