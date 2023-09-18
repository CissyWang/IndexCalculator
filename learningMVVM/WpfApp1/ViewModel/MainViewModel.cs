using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.IO;
using System.Text;
using System.Windows;

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
        private int[,][] sitePerList;
        private int[] classify;
        internal static int[][] popClass;
        double[][] areaPerList;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            campus = new Campus();
            SetSitePerList("生均用地指标-江苏省.csv");
            ExaminePopulationCommand = new RelayCommand(this.ExaminePopulation);
            ExamineRatioCommand = new RelayCommand(this.ExamineRatio);
            ConfirmCommand = new RelayCommand(this.Confirm);
        }
        public RelayCommand ExaminePopulationCommand { get; set; }
        public RelayCommand ExamineRatioCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }

        public int Type { get => campus.Type; set { campus.Type = value; } }
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

        public double AreaTarget { get => areaTarget; set { areaTarget = value; RaisePropertyChanged(); } }
        public double SiteAreaPer { get => siteAreaPer; set { siteAreaPer = value; RaisePropertyChanged(); } }
        public double BuildingSiteAreaPer { get => campus.BuildingSiteAreaPer; set { campus.BuildingSiteAreaPer = value; RaisePropertyChanged(); } }
        public double SportsSiteAreaPer { get => campus.SportsSiteAreaPer; set { campus.SportsSiteAreaPer = value; RaisePropertyChanged(); } }
        public double BuildingSiteArea { get => buildingSiteArea; set { buildingSiteArea = value; RaisePropertyChanged(); } }
        public double SportsSiteArea { get => sportsSiteArea; set { sportsSiteArea = value; RaisePropertyChanged(); } }

        private void ExaminePopulation()
        {
            SiteAreaPer_Limit = PickNum(sitePerList[Type, 0], classify, Population); //规范的生均总用地限制
            int pop_limit = (int)(SiteArea / SiteAreaPer_Limit);//倒推人数限制
            ExaminePopulationResult = pop_limit < Population ?
                $"人均用地面积{Math.Round(campus.SiteAreaPer, 2)}<{SiteAreaPer_Limit},总人数应限制在{SiteAreaPer_Limit * SiteArea}" : "PASS";
            SiteAreaPer = campus.SiteAreaPer;
            BuildingSiteAreaPer = PickNum(sitePerList[Type, 1], classify, Population); //规范的生均校舍总用地限制
            SportsSiteAreaPer = PickNum(sitePerList[Type, 2], classify, Population); //规范的生均体育总用地限制
        }   
        private void ExamineRatio()
        {
            double areaPerLimit = InsertNum(areaPerList[Type], popClass[Type], Population);
            double areaTotal = areaPerLimit * Population;
            ExamineRatioResult = campus.AreaTarget < areaTotal ? $"容积率过低不满足生均建筑面积要求，容积率应大于: { areaTotal / SiteArea}" : "PASS";
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
                MessageBox.Show("超出");
            }
        }
        private void SetSitePerList(string siteAreaPerCsv)
        {
            //此处需改为从表格输入得到三维数组//
            sitePerList = new int[1, 3][] { { new int[] { 45, 51, 54 }, new int[] { 30, 33, 36 }, new int[] { 8, 11, 11 } } };
            classify = new int[] { 0, 5000, 10000 };

            popClass = new int[][]{ new int[] { 5000, 8000, 10000 }, new int[] { 5000, 8000, 10000 },
                new int[] { 5000, 8000, 10000 },new int[] { 1000, 2000, 3000 }, new int[] { 5000, 8000, 10000 },
                new int[] { 5000, 8000, 10000 }, new int[] { 5000, 8000, 10000 }, new int[] { 1000, 2000, 3000 } };
            areaPerList = new double[][] { new double[] { 24.56, 23.52, 22.49 }, new double[] { 26.38, 25, 24.29 },
                new double[] { 21.85, 20.69, 20.07 }, new double[] { 31.28, 29.21, 28.08 }, new double[] { 25.68, 24.18, 23.40 },
                new double[] { 26.15, 24.75, 24.04 }, new double[] { 22.63, 21.39, 20.74 }, new double[]{ 35.25, 31.54, 29.27 } };
        }

        private double PickNum(int[] list, int[]calssify,int population)
        {
            double num=0;
            for (int i = 0; i < classify.Length; i++)
            {
                if (population <= classify[i])
                {
                    num = list[i - 1];
                    break;
                }
                else
                    num = list[i];
                }
                return num;
        }
        private double InsertNum(double[] list,int[]classify, int population)
        {
            double num=0;
            if (population < classify[0])
               return num = list[0];
            for (int i = 1; i < classify.Length; i++)
            {
                if (population < classify[i])
                {
                    num = list[i]-(classify[i]-population)/(list[i]-list[i-1]);
                    break;
                }
                else
                {
                    num = list[i];
                }
            }
            return num;
        }
    }
}
