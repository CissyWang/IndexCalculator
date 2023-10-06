using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Population = 8000;
            SiteArea = 500000;
            PlotRatio = 1;
        }
        BuildingList buildings = new BuildingList();
        private List<string> districtNames;
        public int Population { get; set; }
        public double PlotRatio { get; set; }
        public double SiteArea { get; set; }

        public BuildingList Buildings { get{
                foreach (Building b in MustBuildings)
                {
                    if(!buildings.Contains(b))
                    buildings.Add(b);
                }
                foreach(Building b in OptionalBuildings)
                {
                    if (!buildings.Contains(b))
                        buildings.Add(b);
                }
                return buildings;
            } }
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
        double restBuildingSiteArea;
        double restArea;
        public double RestBuildingSiteArea { get 
            {
                double t= BuildingSiteArea;
                if (MustBuildings!=null)
                     t  -= MustBuildings.SiteArea;
                if (OptionalBuildings != null)
                    t -= OptionalBuildings.SiteArea;
                return t;}set=>restBuildingSiteArea=value;
        }

        public double RestArea
        {
            get
            {
                double t = AreaTarget;
                if (MustBuildings != null)
                    t -= MustBuildings.Area;
                if (OptionalBuildings != null)
                    t -= OptionalBuildings.Area;
                return t;
            }
            set => restArea = value;
        }

        public ObservableCollection<District> Districts { get; set; } 

        public BuildingList SetMustBuildingList(DataTable dt)
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
                string dName = dr[lastIndex - 2].ToString();
                Building _building = new Building
                {
                    Index=index,
                    Name = dr[0].ToString(),
                    District_name = dName,
                    Layer = Convert.ToInt32(dr[lastIndex - 1]),
                    Density = Convert.ToDouble(dr[lastIndex]),
                    Area_per = areaPer,
                    Area = Population * areaPer,
                };

                MustBuildings.Add(_building);
                //if(!districtNames.Contains(dName))
                //{
                //    districtNames.Add(dName);
                //}

                index++;
            }
            return MustBuildings;
        }
        public BuildingList SetOptionalBuildingList(DataTable dt)
        {
            OptionalBuildings = new BuildingList();
            int index = 0;
            int lastIndex = dt.Columns.Count - 1;
            
            foreach (DataRow dr in dt.Rows)
            {
                Building _building = new Building
                {
                    Name = dr[0].ToString(),
                    District_name = dr[lastIndex - 2].ToString(),
                    Layer = Convert.ToInt32(dr[lastIndex - 1]),
                    Density = Convert.ToDouble(dr[lastIndex]),
                    Area = Convert.ToDouble(dr[1]),
                };

                OptionalBuildings.Add(_building);
                index++;
            }

            return OptionalBuildings;
        }

        public ObservableCollection<District> SetDistrict()
        {
            Districts = new ObservableCollection<District>();
            districtNames = new List<string>();

            int index = 0;
            foreach (Building b in Buildings)
            {
                if (!districtNames.Contains(b.District_name))
                {
                    districtNames.Add(b.District_name);
                    var d = new District{
                       Index =  index,
                       Name = b.District_name};
                    d.Buildings = new BuildingList();
                    d.Buildings.Add(b);
                    Districts.Add(d);
                    index++;
                }
                else
                {
                    int i = districtNames.IndexOf(b.District_name);
                    Districts[i].Buildings.Add(b);//
                }
            }
            if (!districtNames.Contains("户外体育区"))
            {

                Districts.Add(new District
                {
                    Index = Districts.Count,
                    Name = "户外体育区",
                    Site_area = SportsSiteArea
                });
                districtNames.Add("户外体育区");
            }
            return Districts;
        }
    }
}
