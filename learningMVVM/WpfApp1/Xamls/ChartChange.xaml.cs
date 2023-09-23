using System;
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
using System.Windows.Shapes;

namespace WpfApp1.Xamls
{
    /// <summary>
    /// ChartChange.xaml 的交互逻辑
    /// </summary>
    public partial class ChartChange : Window
    {
        //public ObservableCollection<Projects> projects { get; set; }
        public ChartChange(ObservableCollection<Building> buildingList)
        {
            InitializeComponent();
            this.DataContext = new
            {
                Model = buildingList
            };
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
