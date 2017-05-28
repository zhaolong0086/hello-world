using System;
using System.Collections.Generic;
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
using System.IO;

namespace WpfLearning
{
    /// <summary>
    /// IoOperationTester.xaml 的交互逻辑
    /// </summary>
    public partial class IoOperationTester : UserControl
    {
        public IoOperationTester()
        {
            InitializeComponent();
        }

        private void CreateNewFileButtonClick(object sender, RoutedEventArgs e)
        {
            string fileContent = "This is a demo file for IO operations.";
            File.WriteAllText(@"C:\Users\long\Documents\Desktop\WpfLearning\IoTestDict\File1.txt", fileContent);
        }

        private void DeleteFileButtonClick(object sender, RoutedEventArgs e)
        {
            string sFilePath = @"C:\Users\long\Documents\Desktop\WpfLearning\IoTestDict\File1.txt";
            if (File.Exists(sFilePath))
                File.Delete(sFilePath);
        }
    }
}
