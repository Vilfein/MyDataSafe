﻿using Microsoft.Win32;
using MyDataSafe.Model;
using MyDataSafe.ViewModel;
using MyDataSafe.Windows;
using System;
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

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            ListOfDatas.ItemsSource = await DVM.LoadAllDataAsync();
        }
    }
}
