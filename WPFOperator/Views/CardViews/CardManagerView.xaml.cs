using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPFOperator.ViewModels;

namespace WPFOperator.Views.CardViews
{
    /// <summary>
    /// Логика взаимодействия для CardManagerView.xaml
    /// </summary>
    public partial class CardManagerView : Window
    {
        public CardManagerView()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            CardListView CLV = new CardListView();
            CLV.DataContext = DataContext;
            CLV.Show();
        }
        

        private void MasterTransferTo_Click(object sender, RoutedEventArgs e)
        {
            if (ListMasterCards.SelectedItem == null || ComboEmployers.SelectedItem == null)
            {
                MessageBox.Show("Не выбрана карта или сотрудник которому надо передать карту.");
                return;
            }

            DateTime? dt = CalendarTransfer.SelectedDate;

            if (dt != null)
            {
                MainViewModel MVM = (MainViewModel)DataContext;
                MVM.MasterTransfer(0, dt.Value);
            }
            else if (MessageBox.Show("Дата не выбрана, выбрать сегодняшний день?", "Alert", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
            {
                MainViewModel MVM = (MainViewModel)DataContext;
                MVM.MasterTransfer(0, DateTime.Now);
            }
        }

        private void MasterTransferFrom_Click(object sender, RoutedEventArgs e)
        {
            if (ListMasterCards.SelectedItem == null)
            {
                MessageBox.Show("Не выбрана карта.");
                return;
            }

            DateTime? dt = CalendarTransfer.SelectedDate;

            if (dt != null)
            {
                MainViewModel MVM = (MainViewModel)DataContext;
                MVM.MasterTransfer(1, dt.Value);
            }
            else if (MessageBox.Show("Дата не выбрана, выбрать сегодняшний день?", "Alert", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
            {
                MainViewModel MVM = (MainViewModel)DataContext;
                MVM.MasterTransfer(1, DateTime.Now);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (ComboType.SelectedItem == null || string.IsNullOrEmpty(TextNumber.Text)) return;

            DateTime? calendarDate = CalendarTransfer.SelectedDate;
            DateTime dt = DateTime.Now;
            string number = TextNumber.Text;
            string t = (string)ComboType.SelectedItem;

            if (calendarDate != null)
            {
                dt = calendarDate.Value;
            }
            else if (MessageBox.Show("Тобой не выбрана дата. \rЕсли нажмешь 'ДА', то я установлю сегодняшний день. \rЕсли нет, то я всё отменю.", "Alert", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            dt = new DateTime(dt.Year, dt.Month, dt.Day);
            ((MainViewModel)DataContext).AddMasterCard(number, t, dt);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Хочешь удалить эту карту?", "Deletion", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
            if (ListMasterCards.SelectedItem != null)
            {
                if (((MainViewModel)DataContext).DeleteMasterCard()) MessageBox.Show("Как хочш, я удалил");
                else MessageBox.Show("Увы, эту карту нельзя больше удалять. Прости :(");
            }
            else MessageBox.Show("Не выбрана карта. Подскажи что удалить, пж...");
        }
    }
}
