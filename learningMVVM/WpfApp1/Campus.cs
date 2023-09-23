using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    //public enum schoolType
    //{
    //    综合一类 = 0,
    //    工业类 = 1,
    //    财经 = 2, 政法 = 2, 管理类 = 2,
    //    体育类 = 3,
    //    综合二类 = 4, 师范类 = 4,
    //    农林 = 5, 医药类 = 5,
    //    外语类 = 6,
    //    艺术类 = 7,
    //}
    public class Campus
    {
        SchoolType type;
        int population;
        double plotRatio;
        double siteArea;

        public Campus()
        {

        }

        //public int Type { get => (int)type; set => type = (schoolType)value; }
        public int Population { get => population; set => population = value; }
        public double PlotRatio { get => plotRatio; set => plotRatio = value; }
        public double SiteArea { get => siteArea; set => siteArea = value; }


        public double AreaTarget { get => siteArea * plotRatio; }
        public double SiteAreaPer { get => siteArea / population; }

        public double SiteAreaPer_Limit { set; get; }
        public double BuildingSiteAreaPer { set; get; }
        public double SportsSiteAreaPer { set; get; }
        public double BuildingSiteArea { get => BuildingSiteAreaPer * Population; }
        public double SportsSiteArea { get => SportsSiteAreaPer * Population; }
        public SchoolType Type { get => type; set => type = value; }
    }
}
