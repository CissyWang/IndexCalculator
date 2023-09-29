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

        Campus campus;
        public ChartChange(Campus campus)
        {
            this.campus = campus;
            InitializeComponent();
            this.DataContext = new
            {
                Model = campus.MustBuildings.Buildings
            };
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (campus.MustBuildings.Area > campus.AreaTarget)
                MessageBox.Show("必配项超出");
            this.Close();
        }
    }
}
