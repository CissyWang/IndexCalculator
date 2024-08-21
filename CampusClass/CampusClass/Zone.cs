using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusClass
{
    public class Zone
    {
        public Zone()
        {
        }

        BuildingList buildings;

        double site_area;
        public double Site_area
        {
            get
            {
                if (Buildings != null)
                    return site_area = Buildings.SiteArea;
                return site_area;
            }
            set { site_area = value; }
        }

        public int Index { get; set; }
        public string Name { get; set; }
        public BuildingList Buildings { get => buildings; set => buildings = value; }
        public double BuildingArea {get
        {
                if (buildings == null)
                {
                    return 0;
                }
                return buildings.Area;
            }
        }
        public double Density
        {
            get
            {
                if (Buildings != null)
                    return Math.Round(Buildings.FloorArea / Site_area,2);
                else
                    return 0;
            }
        }

        public double PlotRatio
        {
            get
            {
                if (Buildings != null)
                    return Math.Round(BuildingArea / Site_area,2);
                else
                    return 0;
            }
        }
    }
}
