﻿using System;
using System.Collections.Generic;
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
using System.Data;

namespace ProjektHotel
{
    /// <summary>
    /// Logika interakcji dla klasy Goscie.xaml
    /// </summary>
    public partial class Goscie : Window
    {
        private DataSet _dataSet;

        public Goscie()
        {
            InitializeComponent();
        }

        public Goscie(DataSet dataSet) : this()
        {
            _dataSet = dataSet;
        }
    }
}
