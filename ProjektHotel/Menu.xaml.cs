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
using System.Data.SqlClient;

namespace ProjektHotel
{
    /// <summary>
    /// Logika interakcji dla klasy Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private DataSet _dataSet;
        private SqlDataAdapter _adapter;

        public Menu()
        {
            InitializeComponent();
        }

        public Menu(DataSet dataSet, SqlDataAdapter adapter) : this()
        {
            _dataSet = dataSet;
            _adapter = adapter;
        }

        private void Zarezerwuj_OnClick(object sender, RoutedEventArgs e)
        {
            Zarezerwuj zarezerwuj = new Zarezerwuj(_dataSet,_adapter);
            zarezerwuj.ShowDialog();
        }

        private void Lista_OnClick(object sender, RoutedEventArgs e)
        {
            Rezerwacje rezerwacje = new Rezerwacje(_dataSet);
            rezerwacje.ShowDialog();
        }

        private void Pokoje_OnClick(object sender, RoutedEventArgs e)
        {
            Pokoje pokoje = new Pokoje(_dataSet);
            pokoje.ShowDialog();
        }

        private void Goscie_OnClick(object sender, RoutedEventArgs e)
        {
            Goscie goscie = new Goscie(_dataSet);
            goscie.ShowDialog();
        }

    }
}
