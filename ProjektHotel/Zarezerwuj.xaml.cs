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
    /// Logika interakcji dla klasy Zarezerwuj.xaml
    /// </summary>
    public partial class Zarezerwuj : Window
    {
        private DataSet _dataSet;
        private int _wybranyGosc;

        public Zarezerwuj()
        {
            InitializeComponent();
        }

        public Zarezerwuj(DataSet dataSet) : this()
        {
            _dataSet = dataSet;
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            DataTable pokoje = _dataSet.Tables["pokoje"];
            DataView view = pokoje.DefaultView;
            ListaPokoi.ItemsSource = view;

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            ListaGosci window = new ListaGosci(_dataSet);
            window.ShowDialog();
            if (window.WybranyGosc != null)
            {

                DataRowView view = window.WybranyGosc;
                zmienStatus(true);

                Imie.Text = view["imie"].ToString();
                Nazwisko.Text = view["nazwisko"].ToString();
                NumerDowodu.Text = view["numer_dowodu"].ToString();
                Ulica.Text = view["ulica"].ToString();
                Miasto.Text = view["miasto"].ToString();
                Telefon.Text = view["Telefon"].ToString();
                Email.Text = view["email"].ToString();

                _wybranyGosc = (int) view["id_goscia"];
            }
            else
            {

                MessageBox.Show("Nie wybrano gościa");

            }

        }

        private void zmienStatus(bool isGoscSelected)
        {

            isGoscSelected = !isGoscSelected;

            Imie.IsEnabled = isGoscSelected;
            Nazwisko.IsEnabled = isGoscSelected;
            NumerDowodu.IsEnabled = isGoscSelected;
            Ulica.IsEnabled = isGoscSelected;
            Miasto.IsEnabled = isGoscSelected;
            Telefon.IsEnabled = isGoscSelected;
            Email.IsEnabled = isGoscSelected;

        }

        private void Wyczysc_OnClick(object sender, RoutedEventArgs e)
        {

            zmienStatus(false);

            Imie.Text = String.Empty;
            Nazwisko.Text = String.Empty;
            NumerDowodu.Text = String.Empty;
            Ulica.Text = String.Empty;
            Miasto.Text = String.Empty;
            Telefon.Text = String.Empty;
            Email.Text = String.Empty;

            DataOd.SelectedDate = null;
            DataDo.SelectedDate = null;
            ListaPokoi.UnselectAll();
        }
    }
}
