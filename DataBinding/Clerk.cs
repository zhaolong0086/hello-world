using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfLearning
{
    public class Clerk : INotifyPropertyChanged
    {
        public Clerk()
        {
        }

        public Clerk(string name, string surName, string sex)
        {
            Name = name;
            SurName = surName;
            Sex = sex;
        }

        /// <summary>名</summary>
        public string Name { get { return name; } set { name = value; IChanged("Name"); } }
        string name;

        /// <summary>姓</summary>
        public string SurName { get { return surName; } set { surName = value; IChanged("SurName"); } }
        string surName;

        /// <summary>性别</summary>
        public string Sex { get { return sex; } set { sex = value; IChanged("Sex"); } }
        string sex;

        /// <summary>地址</summary>
        public string Address { get { return address; } set { address = value; IChanged("Address"); } }
        string address;

        public event PropertyChangedEventHandler PropertyChanged;

        public void IChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
