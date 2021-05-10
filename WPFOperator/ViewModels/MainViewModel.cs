using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WPFOperator.Commands;
using WPFOperator.Models;

namespace WPFOperator.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private UserStatus uStatus;
        private int collectedBits;
        private Visibility simpleVisibility;
        private Visibility fancyVisibility;
        private bool simpleOn;
        private bool fancyOn;
        private string rank;

        private int progressLoad;

        private string ReservedEmployerName;
        private string applicationDirection;
        private int currentSelectedBackground;
        private EmployerObject employer;
        private CardObject card;
        private CardManager cardMaster;
        private Dictionary<int, string> cardTypesDictionary;
        private ObservableCollection<string> cardTypesList;
        private ObservableCollection<CardObject> removedCards;
        private ObservableCollection<EmployerObject> employers;
        private ObservableCollection<CardActionState> monthlyActions;
        private SaveManager manager;
        private LauncherKeyObject key;
        private bool isChildWindowClosed;
        private ICommand childWindowCommand;
        private ICommand commandFileData;
        private ICommand commandPrintFileData;

        public UserStatus UStatus { get { return uStatus; } set { uStatus = value; OnPropertyChanged("UStatus"); } }
        public int CollectedBits { get { return collectedBits; } set { collectedBits = value; OnPropertyChanged("CollectedBits"); } }
        public bool SimpleOn { get { return simpleOn; } set { simpleOn = value; OnPropertyChanged("SimpleOn"); } }
        public bool FancyOn { get { return fancyOn; } set { fancyOn = value; OnPropertyChanged("FancyOn"); } }
        public Visibility SimpleVisibility { get { return simpleVisibility; } set { simpleVisibility = value; OnPropertyChanged("SimpleVisibility"); } }
        public Visibility FancyVisibility { get { return fancyVisibility; } set { fancyVisibility = value; OnPropertyChanged("FancyVisibility"); } }
        public string Rank { get { return rank; } set { rank = "Приветствую тебя, " + value; OnPropertyChanged("Rank"); } }

        public int ProgressLoad { get { return progressLoad; } set { progressLoad = value; OnPropertyChanged("ProgressLoad"); } }
        public string ApplicationDirection { get { return applicationDirection; } set { applicationDirection = value; OnPropertyChanged("ApplicationDirection"); } }
        public int CurrentSelectedBackground { get { return currentSelectedBackground; } set { currentSelectedBackground = value; OnPropertyChanged("CurrentSelectedBackgroud"); } }
        public EmployerObject Employer { get { return employer; } set { employer = value; OnPropertyChanged("Employer"); } }
        public CardObject Card { get { return card; } set { card = value; OnPropertyChanged("Card"); } }
        public CardManager CardMaster { get { return cardMaster; } set { cardMaster = value; OnPropertyChanged("CardMaster"); } }
        public ObservableCollection<CardObject> RemovedCards { get { return removedCards; } set { removedCards = value; OnPropertyChanged("RemovedCards"); } }
        public ObservableCollection<string> CardTypesList { get { return cardTypesList; } set { cardTypesList = value; OnPropertyChanged("CardTypesList"); }  }
        public ObservableCollection<EmployerObject> Employers { get { return employers; } set { employers = value; OnPropertyChanged("Employers"); } }
        public ObservableCollection<CardActionState> MonthlyActions { get { return monthlyActions; } set { monthlyActions = value; OnPropertyChanged("MonthlyActions"); } }
        public bool IsChildWindowClosed { get { return isChildWindowClosed; } set { isChildWindowClosed = value; OnPropertyChanged("IsChildWindowClosed"); } }
        public ICommand ChildWindowCommand { get { if (childWindowCommand == null) childWindowCommand = new RelayCommand(SetChildWindowClosed, CanExecute); return childWindowCommand; } }
        public ICommand CommandFileData { get { if (commandFileData == null) commandFileData = new RelayCommand(CreateFileData, CanExecute); return commandFileData; } }
        public ICommand CommandPrintFileData { get { if (commandPrintFileData == null) commandPrintFileData = new RelayCommand(PrintFileData, CanExecute); return commandPrintFileData; } }


        public MainViewModel()
        {
            /*SimpleVisibility = Visibility.Visible;
            FancyVisibility = Visibility.Hidden;
            SimpleOn = true;
            FancyOn = false;*/
            

            manager = new SaveManager();
            uStatus = manager.LoadUserStatus();
            UStatus.CheckLastRun(DateTime.Now);
            Rank = UStatus.Rank;
            
            CollectedBits = UStatus.Value;
            SimpleOn = UStatus.FancyButtonsOff;
            FancyOn = UStatus.FancyButtonsOn;
            SimpleVisibility = SimpleOn ? Visibility.Visible : Visibility.Hidden;
            FancyVisibility = FancyOn ? Visibility.Visible : Visibility.Hidden;
            CurrentSelectedBackground = UStatus.CurrentBackground;
            //Rank = "Бриллиантовая карта";
            
            /*Console.WriteLine("FancyButtons: " + UStatus.FancyButtons + "");
            Console.WriteLine("FancyOn: " + UStatus.FancyButtonsOn + "");
            Console.WriteLine("FancyOff: " + UStatus.FancyButtonsOff + "");
            Console.WriteLine("Background: " + UStatus.BackgroundEnabled + "");
            Console.WriteLine("CurrentBack: " + UStatus.CurrentBackground + "");*/

            SaveLoadEmployers();

            SaveLoadCards();

            SaveLoadCardTypes();


            CardMaster = manager.LoadCardManager();
            //key = manager.LoadKey();
            IsChildWindowClosed = true;
            

            ApplicationDirection = Directory.GetCurrentDirectory() + "/Media/" + currentSelectedBackground + ".mp4";

            /*UStatus.Value = 440;
            SaveUserStatus();*/
        }

        public void SaveLoadEmployers()
        {
            Employers = manager.LoadEmployers();
        }

        public void SaveLoadCards()
        {
            RemovedCards = manager.LoadCardsArchive();
        }

        public void SaveLoadCardTypes()
        {
            cardTypesDictionary = manager.LoadTypes();
            cardTypesList = new ObservableCollection<string>();
            for (int i = 1; i < cardTypesDictionary.Count + 1; i++)
                cardTypesList.Add(cardTypesDictionary[i]);
            CardTypesList.Add("- - -");
        }

        public void SetReservedEmployerName()
        {
            ReservedEmployerName = Employer.FullName;
        }

        public void RestoreEmployerName()
        {
            Employer.FullName = ReservedEmployerName;
        }

        public void AddNewEmployer(string n)
        {
            EmployerObject EO = new EmployerObject();
            //EO.OnPropertyChanged("-");
            EO.Id = Employers.Count + 1;
            EO.FullName = n;
            Employers.Add(EO);

            SaveEmployers();
        }

        public void RemoveEmployer()
        {
            List<EmployerObject> ArchivedEmployers = manager.LoadEmployersArchive();

            Employers.Remove(Employer);
            ArchivedEmployers.Add(Employer);
            // CARDS DISPOSING
            /*foreach (CardObject CO in Employer.Cards) 
                Cards.Remove(CO);*/

            Employer = null;
            SaveEmployers();
            SaveEmployerArchive();
        }

        public void AddMasterCard(string n, string t, DateTime dt)
        {
            CardObject CO = new CardObject() { CardType = t, Number = n, EmployerName = "Master" };
            CardMaster.AddCard(CO, dt);

            UStatus.UpdateValue(2);
            CollectedBits = UStatus.Value;
            SaveUserStatus();

            SaveCardManager();
        }

        public void AddNewCard(string n, string t, DateTime dt)
        {
            CardObject CO = new CardObject() { CardType = t, Number = n, EmployerName = Employer.FullName };
            Employer.AddCard(CO, dt);

            UStatus.UpdateValue(2);
            CollectedBits = UStatus.Value;
            SaveUserStatus();

            //Cards.Add(CO); // CARDS DISPOSING

            SaveEmployers();
        }

        public void AddNewType(string t)
        {
            cardTypesDictionary.Add(cardTypesDictionary.Count + 1, t);
            cardTypesList.Clear();
            CardTypesList.Add("- - -");
            for (int i = 1; i < cardTypesDictionary.Count + 1; i++)
                cardTypesList.Add(cardTypesDictionary[i]);

            SaveTypes();
        }


        public void RemoveCard(DateTime date)
        {
            CardObject CO = Card;
            Employer.RemoveCard(Card, date);
            RemovedCards.Add(CO);
            //Card.RemoveCard(date);
            SaveEmployers();
            SaveCardArchive();
        }

        public void ReturnCard(DateTime date)
        {
            Employer.ReturnCard(Card, date);
            //Card.ReturnCard(date);
            SaveEmployers();
        }

        public bool DeleteCard()
        {
            CardObject CO = Card;
            if (Employer.DeleteCard(CO) /*&& Cards.Remove(CO)*/) // CARDS DISPOSING
            {
                Card = null;
                SaveEmployers();
                return true;
            }
            return false;
        }

        public bool DeleteMasterCard()
        {
            CardObject CO = Card;
            if (CardMaster.DeleteCard(CO))
            {
                Card = null;
                SaveCardManager();
                return true;
            }
            return false;
        }

        public void SelectEmployer(EmployerObject EO)
        {
            Employer = EO;
        }

        public void DeselectEmployer()
        {
            Employer = null;
        }

        public void SelectCard(CardObject CO)
        {
            Card = CO;
        }

        public void DeselectCard()
        {
            Card = null;
        }

        private void SetChildWindowClosed(object o)
        {
            IsChildWindowClosed = !isChildWindowClosed;
        }

        private void CreateFileData(object o)
        {
            int rowCount = 0;
            List<string> emps = new List<string>();

            foreach (EmployerObject EO in Employers)
            {
                if (EO.HandledCards.Count != 0)
                {
                    rowCount += EO.HandledCards.Count;
                }
                else
                {
                    emps.Add(EO.FullName);
                    rowCount++;
                }
            }

            string[] names = new string[rowCount];
            string[] numbers = new string[rowCount];
            string[] types = new string[rowCount];

            int counter = 0;
            foreach (EmployerObject EO in Employers)
            {
                foreach (CardObject CO in EO.HandledCards)
                {
                    names[counter] = EO.FullName;
                    numbers[counter] = CO.Number;
                    types[counter] = CO.CardType;
                    counter++;
                }
            }

            int c = 0;
            for (int i = names.Length - emps.Count; i < names.Length; i++)
            {
                names[i] = emps[c++];
            }

            ExcelManager EM = new ExcelManager();
            EM.CreateXFile(names, types, numbers);
            /*string file = "";
            int NameLength = 40;

            int[] ks = new int[1];
            string[] vs = new string[1];

            foreach (EmployerObject EO in Employers)
            {
                

                ks = new int[EO.HandledCards.Count];
                vs = new string[EO.HandledCards.Count];

                foreach (CardObject CO in EO.HandledCards)
                {
                    string t = CO.CardType;
                    int h = 0;
                    for (int i = 0; i < t.Length; i++) h += t[i];

                    ks[h % ks.Length]++;
                    vs[h % ks.Length] = CO.CardType;
                }

                for (int i = 0; i < vs.Length; i++)
                {
                    if (ks[i] != 0)
                    {
                        file += EO.FullName;

                        if (EO.FullName.Length > NameLength)
                            file += "\r";
                        else
                            for (int j = 0; j < NameLength - EO.FullName.Length; j++)
                                file += " ";

                        file += vs[i] + "   " + ks[i] + "\r";
                    }
                }
                //file += "\r";
            }

            FileInfo fi = new FileInfo("C:/BitFiles");
            Directory.CreateDirectory(fi.FullName);
            
            StreamWriter sw = new StreamWriter("C:/BitFiles/BitFile.txt"); //"E:/BitFiles/BitFile.txt"
            sw.WriteLine(file);
            sw.Close();*/


            //new DataPrinter();
        }

        private void PrintFileData(object o)
        {
            string file = "";
            int NameLength = 40;

            int[] ks = new int[1];
            string[] vs = new string[1];

            foreach (EmployerObject EO in Employers)
            {


                ks = new int[EO.HandledCards.Count];
                vs = new string[EO.HandledCards.Count];

                foreach (CardObject CO in EO.HandledCards)
                {
                    string t = CO.CardType;
                    int h = 0;
                    for (int i = 0; i < t.Length; i++) h += t[i];

                    ks[h % ks.Length]++;
                    vs[h % ks.Length] = CO.CardType;
                }

                for (int i = 0; i < vs.Length; i++)
                {
                    if (ks[i] != 0)
                    {
                        file += EO.FullName;

                        if (EO.FullName.Length > NameLength)
                            file += "\r";
                        else
                            for (int j = 0; j < NameLength - EO.FullName.Length; j++)
                                file += " ";

                        file += vs[i] + "   " + ks[i] + "\r";
                    }
                }
                //file += "\r";
            }

            /*FileInfo fi = new FileInfo("C:/BitFiles");
            Directory.CreateDirectory(fi.FullName);*/
            //C:/OperatorManagerFiles
            DirectoryInfo dir = new DirectoryInfo("C:/OperatorManagerFiles");
            if (!dir.Exists)
            {
                dir.Create();
            }

            StreamWriter sw = new StreamWriter("C:/OperatorManagerFiles/BitFile.txt");

            sw.WriteLine(file);
            sw.Close();

            new DataPrinter();
        }

        private bool CanExecute(object o)
        {
            return true;
        }

        public string IsCardExist(string n)
        {
            foreach (EmployerObject EO in Employers)
            {
                foreach (CardObject CO in EO.HandledCards)
                {
                    if (CO.Number == n && CO.EmployerName != "")
                    {
                        return CO.EmployerName;
                    }
                }
            }

            // CARDS DISPOSING
            /*foreach (CardObject CO in Cards)
            {
                if (CO.Number == n && CO.EmployerName != "")
                {
                    return CO.EmployerName;
                }
            }*/

            return "";
        }

        public bool IsCardRemoved(string number)
        {
            foreach (CardObject CO in RemovedCards)
            {
                if (CO.Number == number) return true;
            }
            return false;
        }

        public bool IsEmployerFilled()
        {
            return Employer.HasCards();
        }

        public bool IsCardRemoved()
        {
            return Card.IsCardRemoved();
        }

        public bool IsCardReturned()
        {
            return Card.IsCardReturned();
        }

        public string GetCardNumber()
        {
            return Card.Number;
        }

        public void TransferCardFrom(string cardNumber, string EmployerFrom, DateTime date)
        {
            UStatus.UpdateValue(2);
            CollectedBits = UStatus.Value;
            SaveUserStatus();

            foreach (EmployerObject EO in Employers)
            {
                foreach (CardObject CO in EO.HandledCards)
                {
                    if (CO.Number == cardNumber)
                    {
                        Card = CO;
                        break;
                    }
                }
            }

            foreach (EmployerObject EOFrom in Employers)
            {
                if (EOFrom.FullName == EmployerFrom)
                {
                    if (!Card.IsCardReturned())
                    {
                        EOFrom.ReturnCard(Card, date);
                    }
                    
                    Employer.AddCardTransfer(Card, date);
                    Card.EmployerName = Employer.FullName;
                    SaveEmployers();
                    break;
                }
            }

            Card = null;
        }

        public void MasterTransfer(int mode, DateTime date)
        {
            if (Card == null)
            {
                return;
            }
            UStatus.UpdateValue(2);
            CollectedBits = UStatus.Value;
            SaveUserStatus();

            //CardObject CO = (CardObject)card;
            int m = Int32.Parse(mode.ToString());

            
            if (mode == 0) // Give to Employer
            {
                Employer.AddCardTransfer(Card, date);
                Card.EmployerName = Employer.FullName;
                CardMaster.ReturnCard(Card, date);
                SaveEmployers();
                SaveCardManager();
            }
            else if (mode == 1) // back from Employer
            {
                CardMaster.AddCard(Card, date);
                Card.EmployerName = "Master";
                Employer.ReturnCard(Card, date);
                SaveEmployers();
                SaveCardManager();
            }
            
            //Card = null;
        }

        public void FormActionsData(DateTime bgn, DateTime end)
        {
            foreach (EmployerObject EO in Employers)
            {
                EO.GetActionsState(bgn, end);
            }
        }

        public void UpdateUserFancy()
        {
            UStatus.FancyButtons = true;
            CollectedBits = UStatus.Value;
            UStatus.UpdateRank();
            SaveUserStatus();
        }

        public void UpdateUserFancyOn(bool b)
        {
            UStatus.FancyButtonsOn = b;
            UStatus.FancyButtonsOff = !b;

            SimpleOn = UStatus.FancyButtonsOff;
            FancyOn = UStatus.FancyButtonsOn;

            SimpleVisibility = SimpleOn ? Visibility.Visible : Visibility.Hidden;
            FancyVisibility = FancyOn ? Visibility.Visible : Visibility.Hidden;

            SaveUserStatus();
        }

        public void UpdateUserBackground(int i)
        {
            UStatus.BackgroundEnabled |= i;
            CollectedBits = UStatus.Value;
            UStatus.UpdateRank();
            SaveUserStatus();
        }

        public void UpdateCurrentUserBackground(int i)
        {
            UStatus.CurrentBackground = i;
            CurrentSelectedBackground = i;
            SaveUserStatus();
        }

        public void UpdateUserValue(int v)
        {
            UStatus.UpdateValue(v);
            SaveUserStatus();
        }

        public int GetUserBackground()
        {
            return UStatus.BackgroundEnabled;
        }

        public int GetCurrentUserBackground()
        {
            return UStatus.CurrentBackground;
        }

        public bool GetFancyButtons()
        {
            return UStatus.FancyButtons;
        }

        public int GetUserValue()
        {
            return UStatus.Value;
        }

        public bool IsFancyButtonsOn()
        {
            return UStatus.FancyButtonsOn;
        }

        public void SaveEmployers()
        {
            manager.SaveEmployers(Employers);
        }

        public void SaveEmployerArchive()
        {
            
        }

        public void SaveCardArchive()
        {
            manager.SaveCardsArchive(RemovedCards);
        }

        public void SaveCardManager()
        {
            manager.SaveCardManager(CardMaster);
        }

        public void SaveTypes()
        {
            manager.SaveCardTypes(cardTypesDictionary);
        }

        public void SaveUserStatus()
        {
            manager.SaveUserStatus(UStatus);
        }

        public void RemoveMonthlyActions()
        {
            MonthlyActions = null;
        }

        public bool IsKeyUptoDate()
        {
            return key.CheckUpdated();
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
