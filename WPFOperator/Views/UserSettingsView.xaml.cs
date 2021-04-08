using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using WPFOperator.ViewModels;

namespace WPFOperator.Views
{
    /// <summary>
    /// Логика взаимодействия для UserSettingsView.xaml
    /// </summary>
    public partial class UserSettingsView : Window
    {
        private Uri buttonOn;
        private Uri buttonOff;

        public UserSettingsView()
        {
            InitializeComponent();
        }

        public void SetSettings()
        {
            buttonOn = new Uri("/Images/ButtonOn.png", UriKind.Relative);
            buttonOff = new Uri("/Images/ButtonOff.png", UriKind.Relative);

            int be = ((MainViewModel)DataContext).GetUserBackground();
            FirstVideoEnabled = be != 1;
            SecondVideoEnabled = be != 2;
            ThirdVideoEnabled = be != 4;

            int vbe = ((MainViewModel)DataContext).GetCurrentUserBackground();
            FirstVideoEnabled = vbe == 1;
            SecondVideoEnabled = vbe == 2;
            ThirdVideoEnabled = vbe == 4;
            UpdateVideoButtons();

            ButtonsEnabled = ((MainViewModel)DataContext).IsFancyButtonsOn();
            if (ButtonsEnabled) ImageButtonsFirst = buttonOn;
            else ImageButtonsFirst = buttonOff;

            //ImageVideoFirst = new Uri(Directory.GetCurrentDirectory() + "/Images/ButtonOff.png", UriKind.Absolute);
            //ImageButtonFirst.Source = new BitmapImage(new Uri("/Images/ButtonOn.png", UriKind.Relative));
            int bc = ((MainViewModel)DataContext).GetCurrentUserBackground();
            /*TextSecond.Text = bc == 1 ? "Включено" : "Выключено";
            TextThird.Text = bc == 2 ? "Включено" : "Выключено";
            TextFourth.Text = bc == 3 ? "Включено" : "Выключено";*/
        }





        public Uri ImageButtonsFirst
        {
            get { return (Uri)GetValue(ImageButtonsFirstProperty); }
            set { SetValue(ImageButtonsFirstProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageButtonsFirst.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageButtonsFirstProperty =
            DependencyProperty.Register("ImageButtonsFirst", typeof(Uri), typeof(UserSettingsView), new PropertyMetadata(new Uri("/Images/ButtonOff.png", UriKind.Relative)));



        public Uri ImageVideoFirst
        {
            get { return (Uri)GetValue(ImageVideoFirstProperty); }
            set { SetValue(ImageVideoFirstProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageVideoFirst.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageVideoFirstProperty =
            DependencyProperty.Register("ImageVideoFirst", typeof(Uri), typeof(UserSettingsView), new PropertyMetadata(new Uri("/Images/ButtonOff.png", UriKind.Relative)));



        public Uri ImageVideoSecond
        {
            get { return (Uri)GetValue(ImageVideoSecondProperty); }
            set { SetValue(ImageVideoSecondProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageVideoSecond.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageVideoSecondProperty =
            DependencyProperty.Register("ImageVideoSecond", typeof(Uri), typeof(UserSettingsView), new PropertyMetadata(new Uri("/Images/ButtonOff.png", UriKind.Relative)));



        public Uri ImageVideoThird
        {
            get { return (Uri)GetValue(ImageVideoThirdProperty); }
            set { SetValue(ImageVideoThirdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageVideoThird.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageVideoThirdProperty =
            DependencyProperty.Register("ImageVideoThird", typeof(Uri), typeof(UserSettingsView), new PropertyMetadata(new Uri("/Images/ButtonOff.png", UriKind.Relative)));




        public bool ButtonsEnabled
        {
            get { return (bool)GetValue(ButtonsEnabledProperty); }
            set { SetValue(ButtonsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonsEnabledProperty =
            DependencyProperty.Register("ButtonsEnabled", typeof(bool), typeof(UserSettingsView), new PropertyMetadata(false));




        public bool FirstVideoEnabled
        {
            get { return (bool)GetValue(FirstVideoEnabledProperty); }
            set { SetValue(FirstVideoEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstVideoEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstVideoEnabledProperty =
            DependencyProperty.Register("FirstVideoEnabled", typeof(bool), typeof(UserSettingsView), new PropertyMetadata(false));


        public bool SecondVideoEnabled
        {
            get { return (bool)GetValue(SecondVideoEnabledProperty); }
            set { SetValue(SecondVideoEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SecondVideoEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondVideoEnabledProperty =
            DependencyProperty.Register("SecondVideoEnabled", typeof(bool), typeof(UserSettingsView), new PropertyMetadata(false));


        public bool ThirdVideoEnabled
        {
            get { return (bool)GetValue(ThirdVideoEnabledProperty); }
            set { SetValue(ThirdVideoEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThirdVideoEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThirdVideoEnabledProperty =
            DependencyProperty.Register("ThirdVideoEnabled", typeof(bool), typeof(UserSettingsView), new PropertyMetadata(false));

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!((MainViewModel)DataContext).GetFancyButtons() && ((MainViewModel)DataContext).GetUserValue() >= 200)
            {
                ((MainViewModel)DataContext).UpdateUserValue(-200);
                ((MainViewModel)DataContext).UpdateUserFancy();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if ((((MainViewModel)DataContext).GetUserBackground() | 1) != ((MainViewModel)DataContext).GetUserBackground() && ((MainViewModel)DataContext).GetUserValue() >= 220)
            {
                ((MainViewModel)DataContext).UpdateUserValue(-220);
                ((MainViewModel)DataContext).UpdateUserBackground(1);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if ((((MainViewModel)DataContext).GetUserBackground() | 2) != ((MainViewModel)DataContext).GetUserBackground() && ((MainViewModel)DataContext).GetUserValue() >= 240)
            {
                ((MainViewModel)DataContext).UpdateUserValue(-240);
                ((MainViewModel)DataContext).UpdateUserBackground(2);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if ((((MainViewModel)DataContext).GetUserBackground() | 4) != ((MainViewModel)DataContext).GetUserBackground() && ((MainViewModel)DataContext).GetUserValue() >= 250)
            {
                ((MainViewModel)DataContext).UpdateUserValue(-250);
                ((MainViewModel)DataContext).UpdateUserBackground(4);
            }
        }

        private void ButtonsOn_Click(object sender, RoutedEventArgs e)
        {
            if (((MainViewModel)DataContext).GetFancyButtons())
            {
                ButtonsEnabled = !ButtonsEnabled;
                if (ButtonsEnabled) ImageButtonsFirst = buttonOn;
                else ImageButtonsFirst = buttonOff;

                ((MainViewModel)DataContext).UpdateUserFancyOn(ButtonsEnabled);
            }
        }

        private void VideoFirst_Click(object sender, RoutedEventArgs e)
        {
            if ((((MainViewModel)DataContext).GetUserBackground() | 1) != ((MainViewModel)DataContext).GetUserBackground()) return;
            if (((MainViewModel)DataContext).GetCurrentUserBackground() != 1)
            {
                SecondVideoEnabled = false;
                ThirdVideoEnabled = false;
                
                ((MainViewModel)DataContext).UpdateCurrentUserBackground(1);
            }
            else ((MainViewModel)DataContext).UpdateCurrentUserBackground(0);

            FirstVideoEnabled = !FirstVideoEnabled;
            UpdateVideoButtons();
        }

        private void VideoSecond_Click(object sender, RoutedEventArgs e)
        {
            if ((((MainViewModel)DataContext).GetUserBackground() | 2) != ((MainViewModel)DataContext).GetUserBackground()) return;
            if (((MainViewModel)DataContext).GetCurrentUserBackground() != 2)
            {
                FirstVideoEnabled = false;
                ThirdVideoEnabled = false;
                
                ((MainViewModel)DataContext).UpdateCurrentUserBackground(2);
            }
            else ((MainViewModel)DataContext).UpdateCurrentUserBackground(0);

            SecondVideoEnabled = !SecondVideoEnabled;
            UpdateVideoButtons();
        }

        private void VideoThird_Click(object sender, RoutedEventArgs e)
        {
            if ((((MainViewModel)DataContext).GetUserBackground() | 4) != ((MainViewModel)DataContext).GetUserBackground()) return;
            if (((MainViewModel)DataContext).GetCurrentUserBackground() != 4)
            {
                FirstVideoEnabled = false;
                SecondVideoEnabled = false;

                ((MainViewModel)DataContext).UpdateCurrentUserBackground(4);
            }
            else ((MainViewModel)DataContext).UpdateCurrentUserBackground(0);

            ThirdVideoEnabled = !ThirdVideoEnabled;
            UpdateVideoButtons();
        }

        private void UpdateVideoButtons()
        {
            ImageVideoFirst = FirstVideoEnabled ? buttonOn : buttonOff;
            ImageVideoSecond = SecondVideoEnabled ? buttonOn : buttonOff;
            ImageVideoThird = ThirdVideoEnabled ? buttonOn : buttonOff;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            FirstWindow FW = new FirstWindow();
            //FW.DataContext = DataContext;
            FW.Show();
        }
    }
}
