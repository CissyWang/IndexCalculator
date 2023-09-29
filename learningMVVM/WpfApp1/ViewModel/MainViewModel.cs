using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using WpfApp1.Xamls;

namespace WpfApp1.ViewModel
{

    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Campus campus;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            campus = new Campus();
            ExaminePopulationCommand = new RelayCommand(this.ExaminePopulation);

            ConfirmCommand = new RelayCommand(this.Confirm);
            SetMustCommand = new RelayCommand(this.SetMustBuiding);
            //SetOptionalCommand = new RelayCommand(this.SetOptionalBuiding);
            OpenRegChartCommand = new RelayCommand(this.OpenRegChart);

            SelectCtypeChangedCommand = new RelayCommand(this.SelectCtypeChanged);

            CTypeList.Add("��ͨ�ߵ�ѧУ", "Resources/IndexChart-university.xlsx");
            CTypeList.Add("�ߵ�ְҵѧУ", "Resources/IndexChart.xlsx");


        }  
        public RelayCommand ExaminePopulationCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public RelayCommand SetMustCommand { get; set; }
        public RelayCommand SetOptionalCommand { get; set; }
        public RelayCommand OpenRegChartCommand { get; set; }
        public RelayCommand SelectCtypeChangedCommand { get; set; }

        public SchoolType SchoolType { get => campus.Type; set { campus.Type = value; RaisePropertyChanged(); } }
        public int Population { get => campus.Population; set { campus.Population = value > 0 ? value : 0; RaisePropertyChanged(); } }
        public double PlotRatio { get => campus.PlotRatio; set { campus.PlotRatio = value > 0 ? value : 0; RaisePropertyChanged(); } }
        public double SiteArea { get => campus.SiteArea; set { campus.SiteArea = value > 0 ? value : 0; RaisePropertyChanged(); } }

        public double SiteAreaPer_Limit { get => campus.SiteAreaPer_Limit; set { campus.SiteAreaPer_Limit = value; RaisePropertyChanged(); } }

        double areaTarget;
        double siteAreaPer;
        private double buildingSiteArea;
        private double sportsSiteArea;

        string examinePopulationResult;
        string examineRatioResult;
        public string ExaminePopulationResult
        {
            get => examinePopulationResult;
            set { examinePopulationResult = value; RaisePropertyChanged(); }
        }
        public string ExamineRatioResult { get => examineRatioResult; set { examineRatioResult = value; RaisePropertyChanged(); } }
        public Dictionary<string,string> CTypeList { get; set; } = new Dictionary<string, string>();

        private ObservableCollection<SchoolType> schoolTypeList;
        public ObservableCollection<SchoolType> SchoolTypeList { get => schoolTypeList; set { schoolTypeList = value; RaisePropertyChanged(); } }

        public double AreaTarget { get => areaTarget; set { areaTarget = value; RaisePropertyChanged(); } }
        public double SiteAreaPer { get => siteAreaPer; set { siteAreaPer = value; RaisePropertyChanged(); } }
        public double BuildingSiteAreaPer { get => campus.BuildingSiteAreaPer; set { campus.BuildingSiteAreaPer = value; RaisePropertyChanged(); } }
        public double SportsSiteAreaPer { get => campus.SportsSiteAreaPer; set { campus.SportsSiteAreaPer = value; RaisePropertyChanged(); } }
        public double BuildingSiteArea { get => buildingSiteArea; set { buildingSiteArea = value; RaisePropertyChanged(); } }
        public double SportsSiteArea { get => sportsSiteArea; set { sportsSiteArea = value; RaisePropertyChanged(); } }

        string reg;
        public string Reg { get => reg; set { reg = value; RaisePropertyChanged(); } }

        private void ExaminePopulation()
        {
            int t = SchoolType.Key;
            //ȷ����������
            double[] temp = SchoolType.PickNum(Population);
            SiteAreaPer_Limit = temp[0];//�淶���������õ�����
            double areaPerLimit = SchoolType.InsertAreaPer( Population,SchoolType.AreaPerList);

            //����1
            int pop_limit = (int)(SiteArea / SiteAreaPer_Limit);//������������
            ExaminePopulationResult = pop_limit < Population ?
                $"�˾��õ����{Math.Round(campus.SiteAreaPer, 2)}<{SiteAreaPer_Limit},������Ӧ������{ (int)(SiteArea / SiteAreaPer_Limit) }" : "PASS";
            SiteAreaPer = campus.SiteAreaPer;
            BuildingSiteAreaPer = temp[1]; //����У��
            SportsSiteAreaPer = temp[2]; //��������

            //����2
            double areaTotal = areaPerLimit * Population;
            ExamineRatioResult = campus.AreaTarget < areaTotal ? $"�ݻ��ʹ��Ͳ����������������Ҫ���ݻ���Ӧ����: { areaTotal / SiteArea}" : "PASS";
            AreaTarget = campus.AreaTarget;
        }

        private void Confirm()
        {
            if (BuildingSiteAreaPer + SportsSiteAreaPer < siteAreaPer)
            {
                BuildingSiteArea = campus.BuildingSiteArea;
                SportsSiteArea = campus.SportsSiteArea;
            }
            else
            {
                MessageBox.Show("����");
            }
        }

        //private ObservableCollection<Building> mustBuildings;
        //public ObservableCollection<Building> MustBuildings { get => mustBuildings; set { mustBuildings = value; RaisePropertyChanged(); } }
        //private ObservableCollection<Building> optionalBuildings;
        //public ObservableCollection<Building> OptionalBuildings { get => optionalBuildings; set { optionalBuildings = value; RaisePropertyChanged(); } }
        private void SelectCtypeChanged()
        {
            SetTypeList(reg);
            SchoolType = schoolTypeList[0];
        }

        private void OpenRegChart()
        {
            RegChart chartWindow = new RegChart(schoolTypeList);
            chartWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            chartWindow.Show();
        }

        #region ��ȡExcel����

        /// <summary>
        /// ��excel�е����ݵ��뵽DataTable��
        /// </summary>
        /// <param name="fileName">�ļ�·��</param>
        /// <param name="sheetName">excel������sheet������</param>
        /// <param name="isFirstRowColumn">��һ���Ƿ���DataTable��������true��</param>
        /// <returns>���ص�DataTable</returns>
        public static DataTable ExcelToDatatable(string fileName, string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            FileStream fs;
            IWorkbook workbook = null;
            int cellCount = 0;//����
            int rowCount = 0;//����
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read) ;
                if (fileName.IndexOf(".xlsx") > 0) // 2007�汾
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (fileName.IndexOf(".xls") > 0) // 2003�汾
                {
                    workbook = new HSSFWorkbook(fs);
                }
                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);//���ݸ�����sheet���ƻ�ȡ����
                }
                else
                {
                    //Ҳ���Ը���sheet�������ȡ����
                    sheet = workbook.GetSheetAt(0);//��ȡ�ڼ���sheet���˴���ʾ���û�и���sheet���ƣ�Ĭ���ǵ�һ��sheet��  
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    cellCount = firstRow.LastCellNum; //��һ�����һ��cell�ı�� ���ܵ�����
                    if (isFirstRowColumn)//�����һ���Ǳ�����
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)//��һ������ѭ��
                        {
                            DataColumn column = new DataColumn(firstRow.GetCell(i).StringCellValue);//��ȡ����
                            data.Columns.Add(column);//�����
                        }
                        startRow = sheet.FirstRowNum + 1;//1�����ڶ��У���һ��0�ӿ�ʼ��
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;//0
                    }
                    //���һ�еı��
                    rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)//ѭ������������
                    {
                        IRow row = sheet.GetRow(i);//�ڼ���
                        if (row == null||row.GetCell(0)==null)
                        {
                            continue; //û�����ݵ���Ĭ����null;
                        }
                        //��excel��ÿһ�е�������ӵ�datatable������
                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //ͬ��û�����ݵĵ�Ԫ��Ĭ����null
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                   int n =  data.Rows.Count;
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        private void SetTypeList(string path)
        {

            DataTable dt = ExcelToDatatable(path, "�������ָ��", true);
            int stIndex = 0;
            ObservableCollection<SchoolType>  _schoolTypeList = new ObservableCollection<SchoolType>();

            foreach (DataRow dr in dt.Rows)
            {

                string[] a = dr[1].ToString().Split('-');
                string[][] a1 = new string[3][];
                for (int j = 0; j < 3; j++)
                {
                    a1[j] = dr[j + 2].ToString().Split('-');
                }
                int[] class1 = new int[a.Length];
                double[][] sitePer = new double[3][];
                for (int j = 0; j < 3; j++)
                {
                    sitePer[j]= new double[a.Length];
                }
                for (int i = 0; i < a.Length; i++)
                {
                    class1[i] = Convert.ToInt32(a[i]);
                    for (int j = 0; j < 3; j++)
                    {
                        sitePer[j][i] = Convert.ToDouble(a1[j][i]);
                    }
                }

                string[] b = dr[5].ToString().Split('-');
                string[] b1 = dr[6].ToString().Split('-');
                int[] class2 = new int[b.Length];
                double[] areaPer = new double[b.Length];
                for (int i = 0; i < b.Length; i++)
                {
                    class2[i] = Convert.ToInt32(b[i]);
                    areaPer[i] = Convert.ToDouble(b1[i]);
                }

                SchoolType st = new SchoolType
                {
                    Key = stIndex,
                    Text = dr[0].ToString(),
                    Classify = class1,
                    SitePerList = sitePer,
                    PopClass = class2,
                    AreaPerList = areaPer
                };
                _schoolTypeList.Add(st);
                stIndex++;
            }

            SchoolTypeList = _schoolTypeList;
        }
        private void SetMustBuiding()
        {
            DataTable dt  = ExcelToDatatable("Resources/mustBuildings.xlsx", "sheet1", true);
            campus.SetMustBuildingList(dt);

            ChartChange chartWindow = new ChartChange(campus);
            chartWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            chartWindow.Show();
        }
        //private void SetOptionalBuiding()
        //{
        //    OptionalBuildings = new ObservableCollection<Building>();
        //    //OptionalBuildings.Add(new Building(0, "11", 1, 1, 1, "1"));
        //    ChartChange chartWindow = new ChartChange(campus);
        //    chartWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    chartWindow.Show();
        //}
        #endregion

    }
}
