using MyDataSafe.Model;
using MyDataSafe.Service;
using MyDataSafe.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

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
            CB.ItemsSource = Enum.GetValues(typeof(DataColor)).Cast<DataColor>();
            DataContext = data;
            NameTB.TextChanged += EnableButton;
            CB.SelectionChanged += EnableButton;
            SaveBtn.Click += EditData;
        }

        private void EnableButton(object sender, EventArgs e)
        {
            SaveBtn.IsEnabled= true;
        }

        private void EditData(object sender, EventArgs e)
        {
            if(NameTB.Text.Length==0) return;
            else
            {
                data.Name = NameTB.Text;
                data.dataColor = (DataColor)CB.SelectedItem;
                DVM.UpdateFile(data);
                Close();
            }           
        }
    }
}
