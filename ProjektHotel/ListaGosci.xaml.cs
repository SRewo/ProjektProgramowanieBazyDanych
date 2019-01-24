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
    /// Logika interakcji dla klasy ListaGosci.xaml
    /// </summary>
    public partial class ListaGosci : Window
    {
        private DataSet _dataSet;
        public DataRowView WybranyGosc { private set; get; }

        public ListaGosci()
        {
            InitializeComponent();
        }

        public ListaGosci(DataSet ds) : this()
        {
            _dataSet = ds;
            GoscieGrid.ItemsSource = ds.Tables["goscie"].DefaultView;
        }

        private void Zatwierdz_OnClick(object sender, RoutedEventArgs e)
        {
            DataRowView wybrany = (DataRowView)GoscieGrid.SelectedItems[0];
            WybranyGosc = wybrany;
            Close();
        }
    }
}
