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
using static MyDataSafe.ViewModel.DataViewModel;

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
            DVM = new DataViewModel(new Service.DBService());
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

        private async void OpenTheFile(object sender, MouseButtonEventArgs e)
        {
            DataClass? DC = (sender as ListView)?.SelectedItem as DataClass;   
            PlayerWindow PW = new PlayerWindow(await DVM.CreateFile(DC.Name));
            PW.Show();
        }

        private void RemoveTheFile(object sender, EventArgs e) 
        {
            DataClass? selected = ListOfDatas.SelectedItem as DataClass;
            DVM.RemoveFile(selected!,refresh);
        }
        private void EditTheFile(object sender, EventArgs e)
        {
            DataClass selected = ListOfDatas.SelectedItem as DataClass;
            EditWindow EW = new EditWindow(selected, DVM);
            EW.Closed += (s, e) => refresh();
            EW.Show();

        }
    }
}
