using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class BuildingList: IEnumerable<Building>
    {
        private ObservableCollection<Building> buildings;

        public BuildingList()
        {
            this.Buildings = new ObservableCollection<Building>();
        }

        internal int AddBuilding(Building addBuilding)
        {
            //buildings.Add(addBuilding);
            Buildings.Insert(this.Count, addBuilding);
            return this.Count;
        }
        internal void Add(Building addBuilding)
        {
            Buildings.Add(addBuilding);
        }

        public double Area
        {
            get
            {
                double area_all = 0;
                for (int i = 0; i < this.Count; i++)
                {
                    area_all += this[i].Area;
                }
                return area_all;
            }
            }

        //像数组一样返回元素
        public Building this[int index]
        {
            get
            {
                return this.Buildings[index];
            }
            internal set
            {
                this.Buildings[index] = value;
            }
        }

        //返回数量
        public int Count
        {
            get
            {
                return this.Buildings.Count;
            }
        }

        public ObservableCollection<Building> Buildings { get => buildings; set => buildings = value; }

        public IEnumerator<Building> GetEnumerator()
        {
            return this.Buildings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Buildings.GetEnumerator();
        }
    }
}
