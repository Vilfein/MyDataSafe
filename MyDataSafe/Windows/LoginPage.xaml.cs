using MyDataSafe.ViewModel;
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
        Loading ld;
        public LoginPage()
        {
            ld = new Loading();
            LVM = new LoginViewModel();
            InitializeComponent();

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ld.Show();
            LVM.LoadSetting().GetAwaiter().OnCompleted(() =>
            {
                var name = LoginName.Text;
                var pass = LoginPass.Password;

                if ((name == LVM.LoginName) && (pass == LVM.LoginPassword))
                {
                    MainWindow MW = new MainWindow();
                    MW.Show();
                    ld.Close();

                    this.Close();
                }

                else
                {
                    ld.Close();
                    MessageBox.Show("Wrong login!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    LoginPass.Password = string.Empty;
                }
            });
        }
    }
}
