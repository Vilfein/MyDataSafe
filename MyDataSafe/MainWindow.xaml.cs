using Microsoft.Win32;
using MyDataSafe.Model;
using MyDataSafe.ViewModel;
using MyDataSafe.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
           
            this.Closing += (s, e) =>
            {
                DVM.CleanUp();
            };
        }

        private void SaveTheFile(object sender, RoutedEventArgs e)
        {
            string pattern = "Video Files (*.wmv; *.avi; *.mp4; *.mpeg; *.flv)|*.wmv; *.avi; *.mp4; *.mpeg; *.flv" +
                "| Pictures (*.jpg; *.jpeg; *.bmp; *.gif) | *.jpg; *.jpeg; *.bmp; *.gif;"+
                "| Documents (*.txt;*.pdf;*.docx)|*.txt;*.pdf;*.docx"+
                "| Archives (*.zip;*.rar)|*.zip;*.rar";

            OpenFileDialog OF = new OpenFileDialog();
            OF.Title = "Vybrat Soubor";
            OF.Filter = pattern;
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

        private async void Explorer(object sender, EventArgs e)
        {
            DataClass selected = ListOfDatas.SelectedItem as DataClass;
            await DVM.CreateFile(selected.Name);
            string fw = new FileInfo(selected.Name).FullName + "." + selected.TypeFile;
            string path = Path.GetDirectoryName(fw) + "\\TempFiles";
            Process.Start("explorer.exe", @path);
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
           var data = await DVM.LoadAllDataAsync();
            ListOfDatas.ItemsSource = data;
        }

        private async void OpenInSystemPlayer(object sender, RoutedEventArgs e)
        {
            DataClass selected = ListOfDatas.SelectedItem as DataClass;
            await DVM.CreateFile(selected.Name);
            string fw = new FileInfo(selected.Name).FullName + "." + selected.TypeFile;
            fw = @fw.Insert(fw.LastIndexOf('\\') + 1, "TempFiles\\");
            Process.Start("explorer.exe", @fw);
        }
    }
}
