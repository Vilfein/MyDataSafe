using MyDataSafe.Model;
using MyDataSafe.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace MyDataSafe.Windows
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public DataClass data { get; set; }
        private readonly DataViewModel DVM;
        public EditWindow(DataClass data, DataViewModel DVM)
        {
            this.data = data;
            this.DVM = DVM;
            InitializeComponent();
            CB.ItemsSource = DVM.LoadColors();
            DataContext = data;
            int id = data.DataColorId - 1;

            NameTB.TextChanged += EnableButton;
            CB.SelectionChanged += EnableButton;
            SaveBtn.Click += EditData;
        }

        private void EnableButton(object sender, EventArgs e)
        {
            SaveBtn.IsEnabled = true;
        }

        private async void EditData(object sender, EventArgs e)
        {
            if (NameTB.Text.Length > 0) 
            {
                data.Name = NameTB.Text;
                data.Color = CB.SelectedItem as DataColor;
                Close();
                Task.Run(()=> DVM.UpdateFile(data));
            }
        }
    }
}
