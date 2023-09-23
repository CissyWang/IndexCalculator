using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace WpfApp1
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        static public void Main(String[] args)
        {
            
            Application app = new Application();
            //Districts_Model win = new Districts_Model();
            Step1Window win = new Step1Window();
            app.Run(win);
        }
    }
}
