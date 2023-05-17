using Microsoft.Win32;
using MyDataSafe.Model;
using MyDataSafe.ViewModel;
using MyDataSafe.Windows;
using System;
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


        /// <summary>
        /// ButtonClick callback for selecting the file to save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Refresh ListView collection
        /// </summary>
        private void refresh()
        {
            ListOfDatas.ItemsSource = null;
            ListOfDatas.ItemsSource =  DVM.LoadAllData();
        }

        /// <summary>
        /// Button Click Callback for opening the selected file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenTheFile(object sender, MouseButtonEventArgs e)
        {
            string[] types = { "wmv", "avi", "mp4", "mpeg", "mpg", "flv", "mp3", "wav", "wma" };
            DataClass? DC = (sender as ListView)?.SelectedItem as DataClass;

            if (types.Any(x => x == DC?.TypeFile))
            {
                PlayerWindow PW = new PlayerWindow(await DVM.CreateFileAsync(DC!.Name));
                PW.Show();
            }
        }
        /// <summary>
        /// Callback for removing any file from collection and database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RemoveTheFile(object sender, EventArgs e)
        {
            DataClass? selected = ListOfDatas.SelectedItem as DataClass;
            Loading ld = new Loading();
            ld.Show();
            Task task = Task.Run(async() => await DVM.RemoveFileAsync(selected!, null));
            task.GetAwaiter().OnCompleted(() =>
            {
                refresh();
                ld.Close();
            });
        }
        /// <summary>
        /// Summon the dialog for editation file parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EditTheFile(object sender, EventArgs e)
        {
            DataClass? selected = ListOfDatas.SelectedItem as DataClass;
            EditWindow EW = new EditWindow(selected!, DVM);
            EW.Closed += (s, e) =>
            {
                refresh();
            };
            EW.Show();
        }

        /// <summary>
        /// Opening selected file in system explorer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Explorer(object sender, EventArgs e)
        {
            DataClass? selected = ListOfDatas.SelectedItem as DataClass;
            DVM.CreateFileAsync(selected!.Name).GetAwaiter().OnCompleted(() =>
            {
                string fi = new FileInfo(selected.Name).FullName + "." + selected.TypeFile;
                string path = Path.GetDirectoryName(fi) + "\\TempFiles";
                Process.Start("explorer.exe", @path);
            });
        }

        /// <summary>
        /// Callback for load the content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var data = await DVM.LoadAllDataAsync();
            ListOfDatas.ItemsSource = data;
        }

        /// <summary>
        /// Callback for opening data in System Media Player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenInSystemPlayer(object sender, RoutedEventArgs e)
        {
            DataClass? selected = ListOfDatas.SelectedItem as DataClass;
            DVM.CreateFileAsync(selected!.Name).GetAwaiter().OnCompleted(() =>
            {
                string fi = new FileInfo(selected.Name).FullName + "." + selected.TypeFile;
                fi = @fi.Insert(fi.LastIndexOf('\\') + 1, "TempFiles\\");
                Process.Start("explorer.exe", @fi);
            });
        }
        /// <summary>
        /// Keydown callback for Delete key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListOfDatas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                RemoveTheFile(sender, EventArgs.Empty);
            }
        }
    }
}
