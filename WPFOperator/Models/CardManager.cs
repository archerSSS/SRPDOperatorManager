using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFOperator.Models
{
    [Serializable]
    class CardManager : INotifyPropertyChanged
    {
        private ObservableCollection<CardObject> handledCards;
        private CardActionsHandler CAH;
        public ObservableCollection<CardObject> HandledCards { get { return handledCards; } set { handledCards = value; OnPropertyChanged("HandledCards"); } }

        public CardManager()
        {
            CAH = new CardActionsHandler();
            handledCards = new ObservableCollection<CardObject>();
        }

        public void AddCard(CardObject CO, DateTime date)
        {
            CAH.AddTimes.Add(date);
            CO.AddAction(date);
            HandledCards.Add(CO);
        }

        public void ReturnCard(CardObject CO, DateTime date)
        {
            CAH.ReturnTimes.Add(date);
            HandledCards.Remove(CO);
            CO.ReturnCard(date);
        }

        public void RemoveCard(CardObject CO, DateTime date)
        {
            CAH.RemoveTimes.Add(date);
            HandledCards.Remove(CO);
            CO.RemoveCard(date);
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
