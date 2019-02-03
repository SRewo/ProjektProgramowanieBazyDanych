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
    /// Logika interakcji dla klasy Rezerwacje.xaml
    /// </summary>
    public partial class Rezerwacje : MetroWindow
    {
        private DataSet _dataSet;

        public Rezerwacje()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }

        public Rezerwacje(DataSet dataSet) : this()
        {
            _dataSet = dataSet;
        }


        private void Rezerwacje_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataTable rezerwacjeTable = _dataSet.Tables["rezerwacje"];
            DataTable goscieTable = _dataSet.Tables["Goscie"];
            DataTable zarezerwowaneTable = _dataSet.Tables["zarezerwowane"];
            DataTable pokojeTable = _dataSet.Tables["pokoje"];

            var idRezerwacjiLista = from rezerwacja in rezerwacjeTable.AsEnumerable()
                select rezerwacja.Field<int>("id_rezerwacji");

            List<RezerwacjePokoje> rezerwacjeZPokojami = new List<RezerwacjePokoje>();

            foreach (var idRezerwacji in idRezerwacjiLista)
            {

                var idPokoiLista = from zarezerwowane in zarezerwowaneTable.AsEnumerable()
                    where zarezerwowane.Field<int>("id_rezerwacji") == idRezerwacji
                    select zarezerwowane.Field<int>("id_pokoju");
                RezerwacjePokoje rezerwacjePokoje = new RezerwacjePokoje()
                {
                    id_rezerwacji = idRezerwacji,
                    NumeryPokoi = ""
                };

                var numeryPokoi = from pokoj in pokojeTable.AsEnumerable()
                    where idPokoiLista.Contains(pokoj.Field<int>("id_pokoju"))
                    select pokoj.Field<Int16>("numer");

                foreach (var numer in numeryPokoi)
                {
                    rezerwacjePokoje.NumeryPokoi += numer + ", ";
                }

                rezerwacjeZPokojami.Add(rezerwacjePokoje);
            }


            var view = from rezerwacja in rezerwacjeTable.AsEnumerable()
                join goscieLista in goscieTable.AsEnumerable() on rezerwacja.Field<int>("id_goscia") equals goscieLista
                    .Field<int>("id_goscia")
                join rez in rezerwacjeZPokojami on rezerwacja.Field<int>("id_rezerwacji") equals rez.id_rezerwacji
                select new
                {
                    id_rezerwacji = rezerwacja.Field<int>("id_rezerwacji"),
                    gosc = goscieLista.Field<string>("imie") + " " + goscieLista.Field<string>("nazwisko"),
                    data_rezerwacji = rezerwacja.Field<DateTime>("data_rezerwacji").ToShortDateString(),
                    data_od = rezerwacja.Field<DateTime>("od").ToShortDateString(),
                    data_do = rezerwacja.Field<DateTime>("do").ToShortDateString(),
                    cena_laczna = Math.Round(rezerwacja.Field<decimal>("cena_laczna"),2) + " PLN",
                    numery_pokoi = rez.NumeryPokoi
                };


            RezerwacjeGrid.ItemsSource = view;


        }
    }
}

class RezerwacjePokoje
{
    public int id_rezerwacji { get; set; }
    public string NumeryPokoi { get; set; }
}
