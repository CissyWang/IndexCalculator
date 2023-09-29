using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    //增加功能可以查当前的总体规范
    public class SchoolType
    {
        private double[][] sitePerList;
        private int[] classify;

        int[] popClass;
        double[] areaPerList;

        public int Key { get; set; }
        public string Text { get; set; }

        public int[] Classify { get => classify; set => classify = value; }
        public int[] PopClass { get => popClass; set => popClass = value; }
        public double[] AreaPerList { get => areaPerList; set => areaPerList = value; }
        public double[][] SitePerList { get => sitePerList; set => sitePerList = value; }

        //按等级选择生均用地面积
        public double[] PickNum(int population)
        {
            var numArray = new double[3];
            for (int j = 0; j < 3; j++)
            {
                double[] list = sitePerList[j];

                for (int i = 0; i < classify.Length; i++)
                {
                    if (population <= classify[i])
                    {
                        numArray[j] = list[i - 1];
                        break;
                    }
                    else
                        numArray[j] = list[i];
                }
            }
            return numArray;
        }

        //插值法计算生均建筑面积
        public double InsertAreaPer(int population,double[] areaPerList)
        {
            double num = 0;
            if (population < popClass[0])
                return num = areaPerList[0];
            for (int i = 1; i < popClass.Length; i++)
            {
                if (population < popClass[i])
                {
                    num = areaPerList[i] - (popClass[i] - population)* (areaPerList[i] - areaPerList[i - 1]) / (popClass[i]- popClass[i-1]);
                    break;
                }
                else
                {
                    num = areaPerList[i];
                }
            }
            return num;
        }
    }
}
