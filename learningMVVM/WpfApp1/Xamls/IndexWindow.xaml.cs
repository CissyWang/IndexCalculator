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
using System.Windows.Shapes;


namespace WpfApp1
{
    /// <summary>
    /// IndexWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IndexWindow : Window
    {
        public IndexWindow()
        {
            
            InitializeComponent();
        }


        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Step1Window window1 = new Step1Window();
            window1.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window1.Show();
            this.Hide();
        }
    }
}
