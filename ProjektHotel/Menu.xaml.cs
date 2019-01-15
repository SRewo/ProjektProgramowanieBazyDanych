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
        private DataSet _ds;

        public Menu()
        {
            InitializeComponent();
        }

        public Menu(DataSet ds)
        {
            InitializeComponent();
            _ds = ds;
        }

        private void Zarezerwuj_OnClick(object sender, RoutedEventArgs e)
        {
            Rezerwacje rezerwacje = new Rezerwacje();
            rezerwacje.ShowDialog();
        }

        private void Lista_OnClick(object sender, RoutedEventArgs e)
        {
           
        }

        private void Pokoje_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Goscie_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
