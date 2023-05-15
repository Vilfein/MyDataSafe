using MyDataSafe.ViewModel;
using System.Windows;

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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await LVM.LoadSetting();
            var name = LoginName.Text;
            var pass = LoginPass.Password;

            if (( name == LVM.LoginName) && (pass == LVM.LoginPassword))
            {
                MainWindow MW = new MainWindow();
                MW.Show();
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
