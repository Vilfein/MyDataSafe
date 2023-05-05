using Microsoft.Win32;
using MyDataSafe.Model;
using MyDataSafe.ViewModel;
using MyDataSafe.Windows;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace MyDataSafe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool PasswordSignIn = false;
        private DataViewModel DVM { get; set; }
        public MainWindow()
        {
            DVM = new DataViewModel();
            InitializeComponent();
            ListOfDatas.ItemsSource = DVM.LoadAllData();
            this.Closing += (s, e) =>
            {
                DVM.CleanUp();
            };
        }

        private void SaveTheFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OF = new OpenFileDialog();
            OF.ShowDialog();
            DVM.SaveData(OF.FileName);
            refresh();
        }

        private void refresh()
        {
            ListOfDatas.ItemsSource = null;
            ListOfDatas.ItemsSource = DVM.LoadAllData();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private async void OpenTheFile(object sender, MouseButtonEventArgs e)
        {
            DataClass? DC = (sender as ListView)?.SelectedItem as DataClass;   
            PlayerWindow PW = new PlayerWindow(await DVM.CreateFile(DC.Name));
            PW.Show();
        }
    }
}
