using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Building { 
        public Building(int index, string name, int layer, double area, double density, string district)
    {
        this.Index = index;
        this.Name = name;
        this.Layer = layer;
        this.Area = area;
        this.Density = density;
        this.District_name = district;
    }

    public string Name { get; set; }
    public int Layer { get; set; }
    public double Area_per { get; set; }
    public double Density { get; set; }
    public double Area { get; set; }
    public double Site_area { get => Area / Layer / Density; }
    public string District_name { get; set; }
    public double Floor_area { get => Area / Layer; }
    public int Index { get; set; }
}
}
