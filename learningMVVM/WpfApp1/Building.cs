using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Building
    {
        public Building()
        {

        }
        double areaBias;
        public string Name { get; set; }
        public int Layer { get; set; }
        public double Area_per { get; set; }
        public double Density { get; set; }
        public double Area { get; set; }
        public double Site_area { get =>Math.Round( Area / Layer / Density * (1 + AreaBias),2); }
        public string District_name { get; set; }
        public double Floor_area { get => Area / Layer; }
        public int Index { get; set; }
        public double AreaBias
        {
            get =>areaBias; set
            {
                if (value > -1 && value < 1)
                    areaBias = value;
            }
        }
    }
}
