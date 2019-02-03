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
using MahApps.Metro.Controls;

namespace ProjektHotel
{
    /// <summary>
    /// Logika interakcji dla klasy Goscie.xaml
    /// </summary>
    public partial class Goscie : MetroWindow
    {
        private DataSet _dataSet;

        public Goscie()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }

        public Goscie(DataSet dataSet) : this()
        {
            _dataSet = dataSet;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            DataTable goscieTabela = _dataSet.Tables["goscie"];
            GoscieGrid.ItemsSource = goscieTabela.DefaultView;

        }
    }
}
