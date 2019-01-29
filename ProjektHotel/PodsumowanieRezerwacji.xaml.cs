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
using MahApps.Metro.Controls;
using System.Data;

namespace ProjektHotel
{
    /// <summary>
    /// Logika interakcji dla klasy PodsumowanieRezerwacji.xaml
    /// </summary>
    public partial class PodsumowanieRezerwacji : MetroWindow
    {
        private DataSet _ds;
        private int _idRezerwacji;

        public PodsumowanieRezerwacji()
        {
            InitializeComponent();
        }

        public PodsumowanieRezerwacji(DataSet ds,int idRezerwacji):this()
        {
            _ds = ds;
            _idRezerwacji = idRezerwacji;
        }

        private void Wroc_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
