using Microsoft.Win32;
using MyDataSafe.Command;
using MyDataSafe.Model;
using MyDataSafe.ViewModel;
using MyDataSafe.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        Loading ld;
        public ICommand cmd { get; set; }
        private DataViewModel DVM { get; set; }
        public MainWindow()
        {
            DVM = new DataViewModel(new Service.DBService());

            InitializeComponent();

            WebSite.MouseDoubleClick += (s, e) =>
            {
                ProcessStartInfo psi = new ProcessStartInfo("https://www.vasekdoskar.cz")
                {
                    UseShellExecute = true
                };
                Process.Start(psi);
            };

            this.Closing += (s, e) =>
            {
                DVM.CleanUp();
            };
        }



        private async void SaveTheFile(object sender, RoutedEventArgs e)
        {
            string pattern =
                "Video Files (*.wmv; *.avi; *.mp4; *.mpeg; *.mpg; *.flv)|*.wmv; *.avi; *.mp4; *.mpeg; *.mpg; *.flv" +
                "| Pictures (*.jpg; *.jpeg; *.bmp; *.gif) | *.jpg; *.jpeg; *.bmp; *.gif;" +
                "| Documents (*.txt;*.pdf;*.docx)|*.txt;*.pdf;*.docx" +
                "| Archives (*.zip;*.rar)|*.zip;*.rar" +
                "| Music (*.mp3;*.wav;*.wma)|*.mp3;*.wav;*.wma" +
                "| All files (*.*)|*.*";

            OpenFileDialog OF = new OpenFileDialog();
            OF.Title = "Vybrat Soubor";
            OF.Filter = pattern;
            OF.ShowDialog();
            Loading ld = new Loading();
            ld.Show();
            Task task = Task.Run(() => DVM.SaveDataAsync(OF.FileName));
            task.GetAwaiter().OnCompleted(() =>
            {
                refresh();
                ld.Close();
            });
        }

        private void refresh()
        {
            ListOfDatas.ItemsSource = null;
            ListOfDatas.ItemsSource = DVM.LoadAllData();
        }

        private async void OpenTheFile(object sender, MouseButtonEventArgs e)
        {
            string[] types = { "wmv", "avi", "mp4", "mpeg", "mpg", "flv", "mp3", "wav", "wma" };
            DataClass? DC = (sender as ListView)?.SelectedItem as DataClass;

            if (types.Any(x => x == DC?.TypeFile))
            {
                PlayerWindow PW = new PlayerWindow(await DVM.CreateFile(DC.Name));
                PW.Show();
            }
        }

        private async void RemoveTheFile(object sender, EventArgs e)
        {
            DataClass? selected = ListOfDatas.SelectedItem as DataClass;
            Task task = Task.Run(() => DVM.RemoveFileAsync(selected!, null));
            task.GetAwaiter().OnCompleted(refresh);
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
            DVM.CreateFile(selected.Name).GetAwaiter().OnCompleted(async () =>
            {
                string fw = new FileInfo(selected.Name).FullName + "." + selected.TypeFile;
                string path = Path.GetDirectoryName(fw) + "\\TempFiles";
                Process.Start("explorer.exe", @path);
            });
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var data = await DVM.LoadAllDataAsync();
            ListOfDatas.ItemsSource = data;
        }

        private async void OpenInSystemPlayer(object sender, RoutedEventArgs e)
        {
            DataClass selected = ListOfDatas.SelectedItem as DataClass;
            DVM.CreateFile(selected.Name).GetAwaiter().OnCompleted(() =>
            {
                string fw = new FileInfo(selected.Name).FullName + "." + selected.TypeFile;
                fw = @fw.Insert(fw.LastIndexOf('\\') + 1, "TempFiles\\");
                Process.Start("explorer.exe", @fw);
            });
        }

        private void ListOfDatas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                RemoveTheFile(sender, EventArgs.Empty);
            }
        }
    }
}
