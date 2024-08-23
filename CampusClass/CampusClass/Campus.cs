using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusClass
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
            PlotRatioT = 1;
        }
        //double restArea;
        //double restBuildingSiteArea;
        BuildingList buildings = new BuildingList();
        private List<string> zoneNames;
        public int Population { get; set; }
        public double PlotRatio { get=>Area / SiteArea;
            }
        public double SiteArea { get; set; }

        public void BuildingsUpdate()
        {
            buildings.Buildings.Clear();
            foreach (Building b in MustBuildings)
            {
                if ( !buildings.Contains(b))
                    buildings.Add(b);
            }
            foreach (Building b in OptionalBuildings)
            {
                if (!buildings.Contains(b))
                    buildings.Add(b);
            }
        }
   

        public BuildingList MustBuildings { get; set; }
        public BuildingList OptionalBuildings { get; set; }
        public double AreaTarget { get => SiteArea * PlotRatioT; }
        public double SiteAreaPer { get => SiteArea / Population; }
        public double Area
        {
            get => buildings.Area;
        }
        public double Density
        {
            get => buildings.FloorArea / SiteArea;
        }
        public double SiteAreaBias
        {
            get => (BuildingSiteArea - buildings.SiteArea) / buildings.SiteArea;
        }
        public double PlotRatioT{get;set;}
        public double SiteAreaPer_Limit { set; get; }
        public double BuildingSiteAreaPer { set; get; }
        public double SportsSiteAreaPer { set; get; }
        public double BuildingSiteArea { get => BuildingSiteAreaPer * Population; }
        public double SportsSiteArea { get => SportsSiteAreaPer * Population; }
        public SchoolType Type { get; set; }

        public double RestBuildingSiteArea { get 
            {
                double t= BuildingSiteArea;
                if (MustBuildings!=null)
                     t  -= MustBuildings.SiteArea;
                if (OptionalBuildings != null)
                    t -= OptionalBuildings.SiteArea;
                return t;}
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

        }

        public ObservableCollection<Zone> Zones { get; set; }
        public BuildingList Buildings { get => buildings; }

        public BuildingList SetMustBuildingList(DataTable dt)
        {
            MustBuildings = new BuildingList();
            int lastIndex = dt.Columns.Count - 1;
            int typeIndex=0;
            int index = 1;
            for (int i = 2; i <= lastIndex - 3;i++)
            {
                if (dt.Columns[i].ColumnName == Type.Text)
                {
                    typeIndex = i;
                    break;
                }
            }
            foreach (DataRow dr in dt.Rows)
            {                
                double[] area1 = new double[3];
                for (int i = 0; i < area1.Length; i++)
                {
                    area1[i] = Convert.ToDouble(dr[typeIndex].ToString().Replace(" ","").Split('-')[i]);
                }
                double areaPer = Type.InsertAreaPer(Population, area1);
                string dName = dr[lastIndex - 2].ToString();
                Building _building = new Building
                {
                    Index = (dr[0] != DBNull.Value && dr[0] != null && Convert.ToInt32(dr[0]) > 0) ? Convert.ToInt32(dr[0]) : index,
                    Name = dr[1].ToString(),
                    Zone_name = dName,
                    Layer = Convert.ToInt32(dr[lastIndex - 1]),
                    Density = Convert.ToDouble(dr[lastIndex]),
                    Area_per = areaPer,
                    Area = Population * areaPer,
                };

                MustBuildings.Add(_building);

                index++;
            }
            return MustBuildings;
        }
        public BuildingList SetOptionalBuildingList(DataTable dt)
        {
            OptionalBuildings = new BuildingList();
            int index = 1;
            int lastIndex = dt.Columns.Count - 1;
            
            foreach (DataRow dr in dt.Rows)
            {

                Building _building = new Building
                {
                    Index = (dr[0] != DBNull.Value && dr[0] != null && Convert.ToInt32(dr[0]) > 0) ? Convert.ToInt32(dr[0]) : index,
                    Name = dr[1].ToString(),
                    Zone_name = dr[lastIndex - 2].ToString(),
                    Layer = Convert.ToInt32(dr[lastIndex - 1]),
                    Density = Convert.ToDouble(dr[lastIndex]),
                    Area = Convert.ToDouble(dr[2]),
                };

                OptionalBuildings.Add(_building);
                index++;
            }

            return OptionalBuildings;
        }

        public ObservableCollection<Zone> SetZone()
        {
            Zones = new ObservableCollection<Zone>();
            zoneNames = new List<string>();

            int index = 0;
            foreach (Building b in buildings)
            {
                if (!zoneNames.Contains(b.Zone_name))
                {
                    zoneNames.Add(b.Zone_name);
                    var d = new Zone{
                       Index =  index,
                       Name = b.Zone_name};
                    d.Buildings = new BuildingList();
                    d.Buildings.Add(b);
                    Zones.Add(d);
                    index++;
                }
                else
                {
                    int i = zoneNames.IndexOf(b.Zone_name);
                    Zones[i].Buildings.Add(b);//
                }
            }
            if (!zoneNames.Contains("户外体育区"))
            {

                Zones.Add(new Zone
                {
                    Index = Zones.Count,
                    Name = "户外体育区",
                    Site_area = SportsSiteArea
                });
                zoneNames.Add("户外体育区");
            }
            return Zones;
        }

    }
}
