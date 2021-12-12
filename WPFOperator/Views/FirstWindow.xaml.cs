using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFOperator.ViewModels;

namespace WPFOperator.Views
{
    /// <summary>
    /// Логика взаимодействия для FirstWindow.xaml
    /// </summary>
    public partial class FirstWindow : Window, INotifyPropertyChanged
    {

        public FirstWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }


        

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            MainMenuView MMV = new MainMenuView();

            MMV.DataContext = DataContext;
            MMV.SetMainImage();
            MMV.Show();
            Close();

        }

        

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            UserSettingsView USV = new UserSettingsView();
            USV.DataContext = DataContext;
            USV.SetSettings();
            USV.Show();
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string pn)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pn));
            }
        }
    }
}
