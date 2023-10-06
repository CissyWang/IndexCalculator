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
using WpfApp1.ViewModel;

namespace WpfApp1.Xamls
{
    /// <summary>
    /// ChartChange2.xaml 的交互逻辑
    /// </summary>
    public partial class ChartChange2 : Window
    {
        //Campus campus;
        public ChartChange2(MainViewModel mainViewModel)
        {
            //this.campus = campus;
            InitializeComponent();
            this.DataContext = mainViewModel;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (campus.MustBuildings.Area > campus.AreaTarget)
            //    MessageBox.Show("选配项超出");
            this.Close();
        }
    }
}

