using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WpfLearning;
using System.IO;
using System.Xml.Serialization;

namespace WpfLearning
{
    class ClerkManagerViewModel : INotifyPropertyChanged
    {
        public ClerkManagerViewModel()
        {
            ClerkList.Add(new Clerk("long", "zhao", "male"));
            ClerkList.Add(new Clerk("si", "li", "female"));
        }


        public ObservableCollection<Clerk> ClerkList { get { return clerkList; } set { clerkList = value; IChanged("ClerkList"); } }
        ObservableCollection<Clerk> clerkList = new ObservableCollection<Clerk>();

        /// <summary></summary>
        public Clerk CurrentClerk
        {
            get { return currentClerk; }
            set { currentClerk = value; IChanged("CurrentClerk"); }
        }
        private Clerk currentClerk = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public void IChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }







        public ICommand AddNewItem
        {
            get { return new CommandBase(AddNewItemExecute, obj => { return true; }); }
        }

        public void AddNewItemExecute(object sender)
        {
            Clerk clk = new Clerk("NewName", "NewSurName", "Sex");
            ClerkList.Add(clk);
        }

        public ICommand DeleteItem
        {
            get { return new CommandBase(DeleteItemExecute, obj => { return true; }); }
        }

        public void DeleteItemExecute(object sender)
        {
            if (CurrentClerk != null)
            {
                ClerkList.Remove(CurrentClerk);
                CurrentClerk = null;
            }
        }

        public ICommand SaveToFile
        {
            get { return new CommandBase(SaveToFileExecute, obj => { return true; }); }
        }

        public void SaveToFileExecute(object sender)
        {
            string sPath = @"C:\Users\long\Documents\Visual Studio 2013\Projects\WpfLearning\DataBinding\Data.xml";
            if (File.Exists(sPath))
            { 
            }

            FileStream stream = new FileStream(sPath, FileMode.OpenOrCreate);
            XmlSerializer xmlSer = new XmlSerializer(typeof(ObservableCollection<Clerk>));
            xmlSer.Serialize(stream, ClerkList);
            stream.Close();

        }


        public ICommand LoadFromFile
        {
            get { return new CommandBase(LoadFromFileExecute, obj => { return true; }); }
        }

        public void LoadFromFileExecute(object sender)
        {
        }


    }
}
