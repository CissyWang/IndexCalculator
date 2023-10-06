﻿using System;
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
using Rhino.Geometry;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Step1Window : Window
    {
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


        private void NextStep(object sender, RoutedEventArgs e)
        {
            ///1. 启动rhinoCore
            _rhinoCore = new RhinoCore(new string[] { "/NOSPLASH" }, Rhino.Runtime.InProcess.WindowStyle.Hidden);//不显示启动界面，隐藏窗口
            SetRhinoView();
            //界面
            modeBox.ItemsSource = Rhino.Display.DisplayModeDescription.GetDisplayModes();

            doc = RhinoDoc.ActiveDoc;
        }

        RhinoDoc doc;
        List<Rectangle3d> zones;
        List<Guid> zonesGuid;
        List<Guid> buildingsGuid;
        int[] colorH;

        bool IsFirst;
        private void LoadModel(object sender, RoutedEventArgs e)
        {
            if (!IsFirst)
            {
                reload.Content = "重新加载";
                IsFirst = true;
            }
                
            colorH = new int[viewModel.Districts.Count];
            for (int i = 0; i < colorH.Length; i++)
            {
                colorH[i] = 12 * i;
            }
            columnCount = (int)columnSlider.Value;
            DrawZones();
            dH = heightSlider.Minimum;
            DrawBuildings();
            doc.Views.Redraw();
        }


        private void DrawZones()
        {
            if (zonesGuid != null)
            {
                doc.Objects.Delete(zonesGuid, true);
                zonesGuid = null;
            }
            zonesGuid = new List<Guid>();
            //绘制分区
            float width;
            float height;
            float height1;
            float x = 0;
            float y = 0;

            float k = 1.5f;
            int colorIndex = 0;


            var districts = viewModel.Campus.Districts;

            zones = new List<Rectangle3d>();
            var attributes = new List<Rhino.DocObjects.ObjectAttributes>();

            foreach (District d in districts)
            {
                width = 60;
                height = (float)d.Site_area / width;

                if (height > k * width)
                {
                    width = (float)Math.Sqrt(d.Site_area / k);
                    height = k * width;
                }
                else if (k * height < width)
                {
                    height = (float)Math.Sqrt(d.Site_area / k);
                    width = k * height;
                }
                //height1 = (float)floating * height;
                height1 = (float)height;

                int[] _color = HslToRgb(colorH[colorIndex % colorH.Length], 255, 150);
                var attri = new Rhino.DocObjects.ObjectAttributes();
                attri.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;

                attri.ObjectColor = System.Drawing.Color.FromArgb(_color[0], _color[1], _color[2]);

                var plane = new Plane(new Point3d(x, y, 0), Vector3d.ZAxis);
                var rect = new Rectangle3d(plane, width, height);
                zones.Add(rect);

                var zone = Brep.CreateTrimmedPlane(plane, rect.ToNurbsCurve());
                zonesGuid.Add(doc.Objects.AddBrep(zone, attri));
                //zonesGuid.Add(doc.Objects.AddCurve(rect.ToNurbsCurve(), attri));
                if (colorIndex>0&colorIndex % columnCount==0)//colorIndex>0&&colorIndex % 7==0
                {
                    x = 0;
                    y += 550;
                }
                else
                {
                    x += width + 40;
                }
                colorIndex++;
            }
        }
        private void DrawBuildings()
        {
            if (buildingsGuid != null)
            {
                doc.Objects.Delete(buildingsGuid, true);
                buildingsGuid = null;
            }
            buildingsGuid = new List<Guid>();
            var districts = viewModel.Campus.Districts;
            
            int colorIndex = 0;
            for (int i = 0; i < districts.Count; i++)
            {

                if (districts[i].Buildings == null)
                {
                    continue;
                }

                var attriB = new Rhino.DocObjects.ObjectAttributes();
                int[] color2 = HslToRgb(colorH[colorIndex % colorH.Length], 255, 180);
                attriB.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;
                attriB.ObjectColor = System.Drawing.Color.FromArgb(color2[0], color2[1], color2[2]);

                double dist = 10;
                double y1= 10;
                foreach (Building b in districts[i].Buildings)
                {
                    double width1 = zones[i].Width - dist * 2;//X宽度
                    double length = (float)b.Floor_area / width1;//Y宽度

                    if (length < 3)
                    {
                        length = 3;
                        width1 = (float)b.Floor_area / 3;
                    }

                    //底层轮廓线
                    var origin = new Point3d(zones[i].Center.X - width1 / 2, zones[i].Center.Y - zones[i].Height / 2 + y1, 0);
                    var plane = new Plane(origin, Vector3d.ZAxis);
                    var rect = new Rectangle3d(plane, width1, length);
                    var brep = Brep.CreateTrimmedPlane(plane, rect.ToNurbsCurve());
                    var line = new LineCurve(origin, new Point3d(zones[i].Center.X - width1 / 2, zones[i].Center.Y - zones[i].Height / 2 + y1,dH));
                    //每层体块
                    for (int floor = 0; floor < b.Layer; floor++)
                    {
                        
                        
                        var stack = brep.Faces[0].CreateExtrusion(line, true);
                        buildingsGuid.Add(doc.Objects.AddBrep(stack, attriB));
                        brep.Translate(new Vector3d(0, 0, dH));
                    }
                    y1 += length + dist;
                }

                colorIndex++;
            }
        }
        public static int[] HslToRgb(double Hue, double Saturation, double Lightness)
        {
            if (Hue < 0) Hue = 0.0;
            if (Saturation < 0) Saturation = 0.0;
            if (Lightness < 0) Lightness = 0.0;
            if (Hue >= 360) Hue = 359.0;
            if (Saturation > 255) Saturation = 255;
            if (Lightness > 255) Lightness = 255;
            Saturation = Saturation / 255.0;
            Lightness = Lightness / 255.0;
            double C = (1 - Math.Abs(2 * Lightness - 1)) * Saturation;
            double hh = Hue / 60.0;
            double X = C * (1 - Math.Abs(hh % 2 - 1));
            double r = 0, g = 0, b = 0;
            if (hh >= 0 && hh < 1)
            {
                r = C;
                g = X;
            }
            else if (hh >= 1 && hh < 2)
            {
                r = X;
                g = C;
            }
            else if (hh >= 2 && hh < 3)
            {
                g = C;
                b = X;
            }
            else if (hh >= 3 && hh < 4)
            {
                g = X;
                b = C;
            }
            else if (hh >= 4 && hh < 5)
            {
                r = X;
                b = C;
            }
            else
            {
                r = C;
                b = X;
            }
            double m = Lightness - C / 2;
            r += m;
            g += m;
            b += m;
            r = r * 255.0;
            g = g * 255.0;
            b = b * 255.0;
            r = Math.Round(r);
            g = Math.Round(g);
            b = Math.Round(b);

            return new int[3] { (int)r, (int)g, (int)b };
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        bool isClick = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isClick == false)
            {
                btn1.Content = "再次核验";
                isClick = true;
            }
        }

        ChartChange chartWindow1;

        private void SetMustBuilding_Click(object sender, RoutedEventArgs e)
        {
            bool go = true;
            if (chartWindow1 != null)
            {
                if (!chartWindow1.canOpen)
                    go = false;
            }
            if (go)
            {
                chartWindow1 = new ChartChange(viewModel);
                chartWindow1.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                chartWindow1.Show();
            }
        }

        ChartChange2 chartWindow2;
        private void SetOptionalBuilding_Click(object sender, RoutedEventArgs e)
        {
            bool go = true;
            if (chartWindow1 != null)
            {
                if (!chartWindow1.canOpen)
                    go = false;
            }
            if (go)
            {
                chartWindow2 = new ChartChange2(viewModel);
                chartWindow2.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                chartWindow2.Show();
            }
        }

        private void ShowDistrictGrid(object sender, RoutedEventArgs e)
        {
            districtGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        double dH;
        int columnCount;
        private void Building_Refresh(object sender, RoutedEventArgs e)
        {
            dH = heightSlider.Value;
            DrawBuildings();
            doc.Views.ActiveView.Redraw();
        }

        private void Zones_Refresh(object sender, RoutedEventArgs e)
        {
            columnCount = (int)columnSlider.Value;
            DrawZones();
            DrawBuildings();
            doc.Views.ActiveView.Redraw();
        }
    }
}
