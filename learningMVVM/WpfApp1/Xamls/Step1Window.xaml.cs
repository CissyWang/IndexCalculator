using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using WpfApp1.ViewModel;
using WpfApp1;
using WpfApp1.Xamls;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Step1Window : Window
    {
        public static string _myText = "院系及教师教研办公用房 建筑面积 : 24200用地面积: 12100\r\n图书馆建筑面积 :26000用地面积:13000\r\n室内体育用房 建筑面积 :用地面积: 20286\r\n大学生活动用房 建筑面积用地面积: 13714:9600\r\n学生宿舍(公寓)建筑面积 : 200000用地面积: 83333\r\n单身教师宿舍(公寓) 建筑面积 : 8000用地面积:4000\r\n食堂 建筑面积 : 23800用地面积: 34000\r\n校级办公用房 建筑面积 : 13000用地面积:6500\r\n后勤及附属用房建筑面积用地面积:30286:21200\r\n会堂建筑面积 : 4800用地面积:6857";
        public static string _myText1 = "实验研究用房(硕博)建筑面积 :9800用地面积:49002345\r\n图书馆(硕博) 建筑面积 : 3500用地面积:1750\r\n研究生校舍 建筑面积 : 40000用地面积: 32000\r\n留学生及外教生活用房 建筑面积:9425用地面积: 6283\r\n培训工作用房，建筑面积 : 6000用地面积: 40006\r\n学术交流中心(即综合性接待宾馆)建筑面积 : 30000用地面积:66677\r\n重点实验室、教学陈列、创业基地建筑面积 : 90000用地面积:450008\r\n对外继续教育建筑面积 : 20000用地面积: 10000";
        MainViewModel viewModel;
        public Step1Window()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            this.DataContext = viewModel;
        }

        private void ShowRes(object sender, RoutedEventArgs e)
        {
            res.Visibility = Visibility.Visible;
        }


        private void StartCal(object sender, RoutedEventArgs e)
        {
            CalButton.Content = "重新计算";
            ResAreaPanel.Visibility= Visibility.Visible;
            BasicAreas.Text = _myText;
            SelectiveAreas.Text= _myText1;
        }
        private void Change(object sender, RoutedEventArgs e)
        {
            ChartChange window4 = new ChartChange();
            window4.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window4.Show();
        }
        private void NextStep(object sender, RoutedEventArgs e)
        {
            Districts_Text window3= new Districts_Text();
            window3.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window3.Show();
            this.Hide();
        }
    }
}
