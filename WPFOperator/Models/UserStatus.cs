using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFOperator.Models
{
    [Serializable]
    class UserStatus : INotifyPropertyChanged
    {
        private string rank;
        /*
         * User currency. By using it enabling different functionality of the program
         */
        private int value;

        private int dailyValue;

        /*
         * 1 / 2 / 4.  flags: 01 / 10 / 11 / 100 / 101 / 110 / 111
         */
        private int backgroundEnabled;

        /*
         * 1 / 2 / 4.
         */
        private int currentBackground;


        private bool fancyButtons;
        private bool fancyButtonsOn;
        private bool fancyButtonsOff;
        private DateTime lastTimeRun;

        public string Rank { get { return rank; } set { rank = value; OnPropertyChanged("Rank"); } }
        public int Value { get { return value; } set { this.value = value; OnPropertyChanged("Value"); } }
        public int DailyValue { get { return dailyValue; } set { dailyValue = value; OnPropertyChanged("DailyValue"); } }
        public int BackgroundEnabled { get { return backgroundEnabled; } set { this.backgroundEnabled = value; OnPropertyChanged("BackgroundEnabled"); } }
        public int CurrentBackground { get { return currentBackground; } set { this.currentBackground = value; OnPropertyChanged("CurrentBackground"); } }
        public bool FancyButtons { get { return fancyButtons; } set { fancyButtons = value; OnPropertyChanged("FancyButtons"); } }
        public bool FancyButtonsOn { get { return fancyButtonsOn; } set { fancyButtonsOn = value; OnPropertyChanged("FancyButtonsOn"); } }
        public bool FancyButtonsOff { get { return fancyButtonsOff; } set { fancyButtonsOff = value; OnPropertyChanged("FancyButtonsOff"); } }

        public UserStatus()
        {
            Rank = "Юниор";
            FancyButtonsOff = true;
            FancyButtonsOn = false;
        }

        public void CheckLastRun(DateTime date)
        {
            if (date.Month >= lastTimeRun.Month && date.Day > lastTimeRun.Day)
            {
                value += 5;
                lastTimeRun = date;
                DailyValue = 0;
            }
        }

        public void UpdateValue(int v)
        {
            if (v < 0)
            {
                Value += v;
            }
            else if (DailyValue < 20)
            {
                Value += v;
                DailyValue += v;
            }
        }

        public void UpdateRank()
        {
            if (Rank == "Юниор") Rank = "Специалист";
            else if (Rank == "Специалист") Rank = "Золотой Специалист";
            else if (Rank == "Золотой Специалист") Rank = "Магистр";
            else Rank = "Бриллиантовая карта";
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string pn)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pn));
            }
        }
    }
}
