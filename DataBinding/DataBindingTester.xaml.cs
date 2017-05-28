﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfLearning
{
    /// <summary>
    /// DataBindingTester.xaml 的交互逻辑
    /// </summary>
    public partial class DataBindingTester : UserControl
    {
        public DataBindingTester()
        {
            InitializeComponent();

            this.DataContext = new ClerkManagerViewModel();

            clerkList.Add(new Clerk("long", "zhao", "male"));
            clerkList.Add(new Clerk("si", "li", "female"));

            //lstGrid.ItemsSource = clerkList;
        }
        ObservableCollection<Clerk> clerkList = new ObservableCollection<Clerk>();

        private void ListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Clerk clk = e.AddedItems[0] as Clerk;
                //txtName.Text = clk.Name;
                //txtSurName.Text = clk.SurName;
                //txtSex.Text = clk.Sex;
            }
        }
   }
}
