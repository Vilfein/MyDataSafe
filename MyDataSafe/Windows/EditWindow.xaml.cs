using MyDataSafe.Model;
using MyDataSafe.ViewModel;
using System;
using System.Windows;

namespace MyDataSafe.Windows
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private DataClass data { get; set; }
        private readonly DataViewModel DVM;
        public EditWindow(DataClass data, DataViewModel DVM)
        {
            this.data = data;
            this.DVM= DVM;
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
            SaveBtn.IsEnabled= true;
        }

        private async void EditData(object sender, EventArgs e)
        {
            if(NameTB.Text.Length==0) return;
            else
            {
                data.Name = NameTB.Text;
                data.Color = (DataColor)CB.SelectedItem;
                DVM.UpdateFileAsync(data);
                Close();
            }           
        }
    }
}
