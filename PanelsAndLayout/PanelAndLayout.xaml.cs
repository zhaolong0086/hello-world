using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// PanelAndLayout.xaml 的交互逻辑
    /// </summary>
    public partial class PanelAndLayout : UserControl
    {
        public PanelAndLayout()
        {
            InitializeComponent();
        }

        private void ExpandTest(object sender, RoutedEventArgs e)
        {
            Expander ex = sender as Expander;
            Grid gr = ex.Parent as Grid;
            int exRow=Grid.GetRow(ex);
            gr.RowDefinitions[exRow].Height = new GridLength(1, GridUnitType.Star);
            if (gr.RowDefinitions.Count > exRow + 1)
                gr.RowDefinitions[exRow+1].Height = new GridLength(3, GridUnitType.Pixel);
        }

        private void ItemCollapsed(object sender, RoutedEventArgs e)
        {
            Expander ex = sender as Expander;
            Grid gr = ex.Parent as Grid;
            int exRow = Grid.GetRow(ex);
            gr.RowDefinitions[exRow].Height = new GridLength(1, GridUnitType.Auto);
            if (gr.RowDefinitions.Count > exRow + 1)
                gr.RowDefinitions[exRow + 1].Height = new GridLength(0, GridUnitType.Pixel);
        }

        private void AddNewButton(object sender, RoutedEventArgs e)
        {
            Button btn = new Button();
            btn.Content = "New Button";
            btn.Background = new SolidColorBrush(Colors.Red);
            Pan.Children.Add(btn);
        }

        private void RedClicked(object sender, RoutedEventArgs e)
        {
            LayoutDemo.SetBrush(Brushes.Red);
            //this.InvalidateVisual();

            Debug.Print("Pause message.");
        }

        private void GreenClicked(object sender, RoutedEventArgs e)
        {
            LayoutDemo.SetBrush(Brushes.Green);
            //this.InvalidateVisual();

            Debug.Print("Pause message.");
        }
    }
}
