using System;
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
    /// Logika interakcji dla klasy Rezerwacje.xaml
    /// </summary>
    public partial class Rezerwacje : Window
    {
        private DataSet _dataSet;

        public Rezerwacje()
        {
            InitializeComponent();
        }

        public Rezerwacje(DataSet dataSet) : this()
        {
            _dataSet = dataSet;
        }


        private void Rezerwacje_OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
