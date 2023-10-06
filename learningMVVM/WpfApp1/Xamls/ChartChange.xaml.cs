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
using WpfApp1.ViewModel;

namespace WpfApp1.Xamls
{
    /// <summary>
    /// ChartChange.xaml 的交互逻辑
    /// </summary>
    public partial class ChartChange : Window
    {

        internal bool canOpen;
        public ChartChange(MainViewModel mainViewModel)
        {
            //this.campus = viewModel.Campus;
            InitializeComponent();
            this.DataContext = mainViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            canOpen = true;
        }







        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (campus.MustBuildings.Area < campus.AreaTarget && campus.MustBuildings.SiteArea < campus.BuildingSiteArea)
        //    {
        //        this.Close();
        //        return;
        //    }

        //    var r = MessageBox.Show("必配项总面积超出限制，请调整", "提示", MessageBoxButton.OKCancel);
        //    if (r == MessageBoxResult.Cancel)
        //        this.Close();
        //    //}
        //}
    }
}
