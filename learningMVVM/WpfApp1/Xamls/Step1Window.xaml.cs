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
using Rhino.Runtime.InProcess;
using Rhino.Display;
using Rhino;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Step1Window : Window
    {
        public static string _myText = "会堂建筑面积 : 4800用地面积:6857";//
        public static string _myText1 = "对外继续教育建筑面积 : 20000用地面积: 10000";//
        MainViewModel viewModel;
        public Step1Window()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            this.DataContext = viewModel;
            RhinoInside.Resolver.Initialize();
            
        }

        #region SetRhinoView

        RhinoCore _rhinoCore;
        RhinoView rv;
        private IntPtr viewHandle = IntPtr.Zero;
        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
        private void UnLoaded(object sender, RoutedEventArgs e)
        {
            _rhinoCore.Dispose();
        }
        private void SetRhinoView()
        {

            ///2. 新增一个view，启动时是perspective，启动后可以修改视图和窗口大小
            if (Rhino.RhinoDoc.ActiveDoc.Views.ActiveView == null)
                rv = Rhino.RhinoDoc.ActiveDoc.Views.Add("RhinoView", DefinedViewportProjection.Perspective, new System.Drawing.Rectangle(0, 0, 300, 600), true);//主要
            else
                rv = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView;
            rv.Floating = true;
            rv.TitleVisible = false;//标题不可见，结合3.3防止窗口被挪动
            rv.ActiveViewport.DisplayMode = DisplayModeDescription.GetDisplayMode(DisplayModeDescription.ShadedId);//预设显示模式-阴影ShadedId

            /// 3.将view绑定到布局,名称是rhinoView；需要用到Win32API控制Microsoft Windows
            viewHandle = FindWindow(null, "Rhino 工作视窗");//创建指针找到rhino的view
                                                        // viewHandle = rv.Handle;
            try
            {

                ///3.1 设置rhinoView为WPF窗口的子窗口（设定后rhino窗口可以在WPF里面移动）
                SetParent(viewHandle, new WindowInteropHelper(this).Handle);//

                ///3.2 改变rhino view风格,使边框不可见
                SetWindowLong(new HandleRef(this, viewHandle), GWL_STYLE, WS_VISIBLE);

                ///3.3 缩放到rhinoview布局的大小和位置
                SetPos();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetPos();
        }
        private void SetPos()
        {
            var scale = VisualTreeHelper.GetDpi(this);//分辨率，一直都是1
            var pt = rhinoViewGrid.TransformToAncestor(this).Transform(new System.Windows.Point(0, 0));//获取Grid的基点位置
                                                                                                       //ActualHeight/ActualWidth

            MoveWindow(viewHandle, (int)(pt.X * scale.DpiScaleX), (int)(pt.Y * scale.DpiScaleY),
                (int)Math.Ceiling(rhinoViewGrid.ActualWidth * scale.DpiScaleX),
                (int)Math.Ceiling(rhinoViewGrid.ActualHeight * scale.DpiScaleY), true);

        }
        #region 将C语言的Win32API方法转为C#的方法
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);



        public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }
        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, int dwNewLong);

        public const int GWL_STYLE = (-16);
        public const int WS_VISIBLE = 0x10000000;
        #endregion
        #endregion

        #region toolBar
        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            Rhino.UI.OpenFileDialog openFileDialog = new Rhino.UI.OpenFileDialog();
            openFileDialog.Filter = ".3dm";

            if (!openFileDialog.ShowOpenDialog())
                return;
            //Mouse.OverrideCursor = Cursors.Wait;
            RhinoDoc.ActiveDoc.Modified = false;
            if (openFileDialog.FileName.Contains(".3dm"))
            {
                RhinoDoc.ActiveDoc.Dispose();
                RhinoDoc.Open(openFileDialog.FileName, out bool alreadyOpen);
                //RhinoDoc.ActiveDoc.Views.ActiveView.Close();
                SetRhinoView();
            }
            else
            {
                MessageBox.Show("Not Rhino Files");
            }
            //Mouse.OverrideCursor = null;
        }
        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            RhinoApp.RunScript("Import", false);
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            RhinoApp.RunScript("SaveAs", false);
        }
        private void modeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            modeBox.SelectedItem = cmb.SelectedItem;
            try
            {
                rv.ActiveViewport.DisplayMode = cmb.SelectedItem as DisplayModeDescription;
            }
            catch { }
        }
        #endregion 

        private void ShowStep2(object sender, RoutedEventArgs e)
        {
            step2.Visibility = Visibility.Visible;
        }
        private void ShowStep3(object sender, RoutedEventArgs e)
        {
            step3.Visibility = Visibility.Visible;
        }
        private void StartCal(object sender, RoutedEventArgs e)
        {
            CalButton.Content = "重新配置";
            ResAreaPanel.Visibility= Visibility.Visible;
            BasicAreas.Text = _myText;
            SelectiveAreas.Text= _myText1;
        }

        private void NextStep(object sender, RoutedEventArgs e)
        {
            ///1. 启动rhinoCore
            _rhinoCore = new RhinoCore(new string[] { "/NOSPLASH" }, Rhino.Runtime.InProcess.WindowStyle.Hidden);//不显示启动界面，隐藏窗口
            SetRhinoView();
            //界面
            modeBox.ItemsSource = Rhino.Display.DisplayModeDescription.GetDisplayModes();

            ////
            //Districts_Text window3= new Districts_Text();
            //window3.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //window3.Show();
            //this.Hide();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
