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

            DataTable goscie = _ds.Tables["goscie"];
            DataTable rezerwacje = _ds.Tables["rezerwacje"];
            DataTable zarpokoje = _ds.Tables["zarezerwowane"];
            DataTable pokoje = _ds.Tables["pokoje"];

            var idGoscia = from rezerwacja in rezerwacje.AsEnumerable()
                where rezerwacja.Field<int>("id_rezerwacji") == _idRezerwacji
                select rezerwacja.Field<int>("id_goscia");

            var rezerwacjaGosc = from gosc in goscie.AsEnumerable()
                where gosc.Field<int>("id_goscia") == idGoscia.First()
                select gosc;

            var rezerwacjaDoZaplaty = from rezerwacja in rezerwacje.AsEnumerable()
                where rezerwacja.Field<int>("id_rezerwacji") == _idRezerwacji
                select rezerwacja.Field<decimal>("cena_laczna");

            var idPokoi = from zarpokoj in zarpokoje.AsEnumerable()
                where zarpokoj.Field<int>("id_rezerwacji") == _idRezerwacji
                select zarpokoj.Field<int>("id_pokoju");

            var numeryPokoi = from pokoj in pokoje.AsEnumerable()
                where idPokoi.Contains(pokoj.Field<int>("id_pokoju"))
                select pokoj.Field<Int16>("numer");

            var datyOdDo = from rezerwacja in rezerwacje.AsEnumerable()
                where rezerwacja.Field<int>("id_rezerwacji") == _idRezerwacji
                select new {
                    dataOd = rezerwacja.Field<DateTime>("od"),
                    dataDo = rezerwacja.Field<DateTime>("do")

                };

            string listaPokoi = "";

            if(numeryPokoi.Count() != 0)
                foreach (var pokoj in numeryPokoi)
                {

                    listaPokoi += pokoj + ", ";

                }

            DataRow wybranyGosc = rezerwacjaGosc.First();


            Imie.Text = (string) wybranyGosc[1];
            Nazwisko.Text = (string) wybranyGosc[2];
            NumerDowodu.Text = (string) wybranyGosc[3];
            Ulica.Text = (string) wybranyGosc[4];
            Miasto.Text = (string) wybranyGosc[5];
            Telefon.Text = (string) wybranyGosc[6];
            Email.Text = (string) wybranyGosc[7];
            DataOd.Text = datyOdDo.FirstOrDefault().dataOd.ToShortDateString();
            DataDo.Text = datyOdDo.FirstOrDefault().dataDo.ToShortDateString();
            ZarPokoje.Text = listaPokoi;
            Kwota.Text = rezerwacjaDoZaplaty.First() + " " + "PLN";

        }
    }
}
