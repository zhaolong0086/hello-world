using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfLearning
{
    public class PtGrid : Grid
    {
        public PtGrid()
        {
            Debug.Print("PtGrid Constructor.\n" + DateTime.Now.ToString());
            Debug.Print(Children.Count.ToString());

            this.Initialized += PtGrid_Initialized;
            this.Loaded += PtGrid_Loaded;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Debug.Print("PtGrid OnInitialized.\n" + DateTime.Now.ToString());
            Debug.Print(Children.Count.ToString());
        }

        void PtGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.Print("PtGrid Loaded.\n" + DateTime.Now.ToString());
            Debug.Print(Children.Count.ToString());
        }

        void PtGrid_Initialized(object sender, EventArgs e)
        {
            Debug.Print("PtGrid Initialized.\n" + DateTime.Now.ToString());
            Debug.Print(Children.Count.ToString());
        }
   }
}
