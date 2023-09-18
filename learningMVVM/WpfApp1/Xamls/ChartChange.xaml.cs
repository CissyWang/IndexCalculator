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
        public ObservableCollection<Projects> projects { get; set; }
        public ChartChange()
        {
            InitializeComponent();
            projects = new ObservableCollection<Projects>();

            projects.Add(new Projects { Name = "教学实训用房", Zbenke = 3.0f,Zshuoshi=6.0f,Zboshi=8.0f, DistrictName="专业教学实训",Level=5f,Density=0.4f,AreaBias=0.15f });
            projects.Add(new Projects { Name = "图书馆", Zshuoshi = 3.0f,Zboshi=0.5f, DistrictName = "图书馆", Level = 5f, Density = 0.4f, AreaBias = 0.15f });
            projects.Add(new Projects { Name = "学生宿舍（公寓）",Zshuoshi=5.0f,Zboshi=10.0f, DistrictName = "学生宿舍", Level = 5f, Density = 0.4f, AreaBias = 0.15f });
            projects.Add(new Projects { Name = "留学生及外教", DistrictName = "留学生及外教", Level = 5f, Density = 0.4f, AreaBias = 0.15f });
            projects.Add(new Projects { Name = "培训工作用房", DistrictName = "培训工作用房", Level = 5f, Density = 0.4f, AreaBias = 0.15f });
            projects.Add(new Projects { Name = "图书馆", DistrictName = "专业教学实训", Level = 5f, Density = 0.4f, AreaBias = 0.15f });
            projects.Add(new Projects { Name = "学术交流中心",TotalArea=50000f, DistrictName = "科研", Level = 10f, Density = 0.4f, AreaBias = 0.15f });
            projects.Add(new Projects { Name = "重点实验室", TotalArea = 50000f, DistrictName = "科研", Level = 5f, Density = 0.4f, AreaBias = 0.15f });
            projects.Add(new Projects { Name = "对外继续教育", TotalArea = 50000f, DistrictName = "科研", Level = 5f, Density = 0.4f, AreaBias = 0.15f });
            projects.Add(new Projects { Name = "研发 产学研", TotalArea = 50000f, DistrictName = "科研", Level = 5f, Density = 0.4f, AreaBias = 0.15f });
            myDataGrid.ItemsSource = projects;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public class Projects
        {
            public string Name { get; set; }
            public float Zbenke { get; set; }
            public float Zshuoshi { get; set; }
            public float Zboshi { get; set; }
            public float Zinter { get; set; }
            public float Ztotal { get; set; }
            public float TotalArea { get; set; }
            public string DistrictName { get; set; }
            public float Level { get; set; }
            public float Density { get; set; }
            public float AreaBias { get; set; }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
