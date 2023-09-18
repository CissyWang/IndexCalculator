
using Rhino.Display;
using Rhino.Geometry;
using Rhino.Runtime.InProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rhino;
using System.Windows.Threading;
using System.Threading;
using System.IO;


namespace TestRhinoInWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RhinoEXEWindow : Window
    {
        public RhinoEXEWindow()
        {
            InitializeComponent();

        }

        RhinoCore _rhinoCore;
        private IntPtr viewHandle = IntPtr.Zero;
        Grasshopper.Plugin.GH_RhinoScriptInterface gh;

        //测试选择物件
        Rhino.DocObjects.RhinoObject selectedObject;
        bool canSelect=false;
        //double angle=0;

        RhinoView rv;
        
        /// <summary>
        /// 加载项
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ///1. 启动rhinoCore
            _rhinoCore = new RhinoCore(new string[] { "/NOSPLASH" }, Rhino.Runtime.InProcess.WindowStyle.Hidden);//不显示启动界面，隐藏窗口
            RunGH();

            //var files = Directory.GetFiles(@"Resources\");
            //foreach (var file in files)
            //{
            //    if (System.IO.Path.GetExtension(file) == ".3dm")
            //    {
            //        RhinoDoc.Open(file, out bool alreadyOpen);
            //        break;
            //    }
            //}

            SetRhinoView();

            //界面
            modeBox.ItemsSource = Rhino.Display.DisplayModeDescription.GetDisplayModes();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            RhinoDoc.ActiveDoc.Modified = true;
            var result = MessageBox.Show("Save Current Model？", "确认", MessageBoxButton.YesNoCancel);
            if ( result == MessageBoxResult.Yes)
            {
                RhinoApp.RunScript("Save",false);
                e.Cancel = false;
            }
            else if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
            
        }

        /// <summary>
        /// 退出项
        /// </summary>
        private void UnLoaded(object sender, RoutedEventArgs e)
        {
            
            gh.CloseAllDocuments();
            Grasshopper.Plugin.Commands.Run_GrasshopperUnloadPlugin();
            gh = null;

            _rhinoCore.Dispose();
        }
        void RunGH()
        {
            //这两项必须放在方法里面；
            gh = RhinoApp.GetPlugInObject("Grasshopper") as Grasshopper.Plugin.GH_RhinoScriptInterface;
            gh.RunHeadless();
        }
        #region 设置rhino视窗
        private void SetRhinoView()
        {

            ///2. 新增一个view，启动时是perspective，启动后可以修改视图和窗口大小
            //if (Rhino.RhinoDoc.ActiveDoc.Views.ActiveView == null)
            //    rv = Rhino.RhinoDoc.ActiveDoc.Views.Add("RhinoView", DefinedViewportProjection.Perspective, new System.Drawing.Rectangle(0, 0, 300, 600), true);//主要
            //else
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



        /// <summary>
        ///打开新文件，重新启动绑定视图
        /// </summary>
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

        /// <summary>
        /// 选择物件
        /// </summary>
        private void SelectObject_Click(object sender, RoutedEventArgs e)
        {
            canSelect = true;
            RhinoDoc.SelectObjects += RhinoDoc_SelectObjects;
        }
        private void RhinoDoc_SelectObjects(object sender, Rhino.DocObjects.RhinoObjectSelectionEventArgs e)
        {
            if (!canSelect)
                return;
            
            Console.WriteLine("selected");
            //foreach(Rhino.DocObjects.RhinoObject o in e.RhinoObjects)
            //{
            //    if (o.Geometry.GetType() == typeof(Rhino.Geometry.Brep))
            //    {
            //        selectedObject = o;
            //        //o.Select(true, true);
            //        Console.WriteLine("Find a "+ o.Geometry.GetType().ToString());
            //        selectLb.Content = "A brep has been selected";
            //        break;
            //    }
            //}      
           
            canSelect = false;
            
            //throw new NotImplementedException();
        }

        ///<summary>
        ///操作object（需要commit changes）
        ///</summary>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //angle = RotateRadians.Value;
            //if (selectedObject != null)
            //{
            //    var geometry = selectedObject.Geometry;
            //    var centerPoi = geometry.GetBoundingBox(false).Center;
            //    geometry.Rotate(RotateRadians.Value, new Rhino.Geometry.Vector3d(0, 0, 1), centerPoi);
            //    selectedObject.CommitChanges();

            //    selectedObject.Select(false);
            //    selectLb.Content = "nothing selected";

            //    RhinoDoc.ActiveDoc.Views.Redraw();
            //}
        }

        private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            //var rect = new Rectangle3d(Plane.WorldXY,100,120);
            //var attri = new Rhino.DocObjects.ObjectAttributes();
            
            //attri.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;
            //attri.ObjectColor = System.Drawing.Color.Red;

            ////attri.LayerIndex = 2;
            //RhinoDoc.ActiveDoc.Objects.AddRectangle(rect,attri);
            //RhinoDoc.ActiveDoc.Objects.AddBox(new Box(Plane.WorldXY, new Interval(200,220), new Interval(200, 220), new Interval(200, 220)), attri);
            //RhinoDoc.ActiveDoc.Views.Redraw();
            ////RhinoApp.RunScript("_box", true);

        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            RhinoApp.RunScript("Import",false);
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            RhinoApp.RunScript("SaveAs", false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        ///运行gh文件
        ///
    }
}
