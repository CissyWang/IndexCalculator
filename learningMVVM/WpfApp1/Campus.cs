using System;
using System.Collections.Generic;
using System.Data;
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
        public Campus()
        {

        }

        //public int Type { get => (int)type; set => type = (schoolType)value; }
        public int Population { get; set; }
        public double PlotRatio { get; set; }
        public double SiteArea { get; set; }

        public BuildingList MustBuildings{get;set;}
        public BuildingList OptionalBuildings { get; set; }
        public double AreaTarget { get => SiteArea * PlotRatio; }
        public double SiteAreaPer { get => SiteArea / Population; }

        public double SiteAreaPer_Limit { set; get; }
        public double BuildingSiteAreaPer { set; get; }
        public double SportsSiteAreaPer { set; get; }
        public double BuildingSiteArea { get => BuildingSiteAreaPer * Population; }
        public double SportsSiteArea { get => SportsSiteAreaPer * Population; }
        public SchoolType Type { get; set; }

        public void SetMustBuildingList( DataTable dt)
        {
            MustBuildings = new BuildingList();
            int index = 0;
            int lastIndex = dt.Columns.Count - 1;
            foreach (DataRow dr in dt.Rows)
            {
                double[] area1 = new double[lastIndex - 3];
                for (int i = 1; i <= area1.Length; i++)
                {
                    area1[i-1] = Convert.ToDouble(dr[i]);
                }
                double areaPer = Type.InsertAreaPer(Population, area1);
                Building _building = new Building
                {
                    Name = dr[0].ToString(),
                    District_name = dr[lastIndex - 2].ToString(),
                    Layer = Convert.ToInt32(dr[lastIndex - 1]),
                    Density = Convert.ToDouble(dr[lastIndex]),
                    Area_per = areaPer,
                    Area = Population * areaPer,
                };

                MustBuildings.Add(_building);
                index++;
            }
        }
    }
}
