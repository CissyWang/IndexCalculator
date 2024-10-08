﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CampusClass;

namespace WpfApp1.Xamls
{
    /// <summary>
    /// xaml.xaml 的交互逻辑
    /// </summary>
    public partial class RegChart : Window
    {
        public RegChart(ObservableCollection<SchoolType> schoolTypeList)
        {
            InitializeComponent();
            this.DataContext = new
            {
                Model = schoolTypeList
            };
        }
    }
}
