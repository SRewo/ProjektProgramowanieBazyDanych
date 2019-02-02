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
using MahApps.Metro.Controls;

namespace ProjektHotel
{
    /// <summary>
    /// Logika interakcji dla klasy Zarezerwuj.xaml
    /// </summary>
    public partial class Zarezerwuj : MetroWindow
    {
        private DataSet _dataSet;
        private int _wybranyGosc;
        private SqlDataAdapter _adapter;
        private DateTime _dataOd;
        private DateTime _dataDo;

        public Zarezerwuj()
        {
            InitializeComponent();
        }

        public Zarezerwuj(DataSet dataSet,SqlDataAdapter adapter) : this()
        {
            _dataSet = dataSet;
            _adapter = adapter;
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            DataTable pokoje = _dataSet.Tables["pokoje"];
            
            ListaPokoi.ItemsSource = pokoje.AsDataView();

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

            _wybranyGosc = 0;
            Imie.Text = String.Empty;
            Nazwisko.Text = String.Empty;
            NumerDowodu.Text = String.Empty;
            Ulica.Text = String.Empty;
            Miasto.Text = String.Empty;
            Telefon.Text = String.Empty;
            Email.Text = String.Empty;

            DataOd.SelectedDate = null;
            DataDo.SelectedDate = null;

            DataTable pokoje = _dataSet.Tables["pokoje"];

            ListaPokoi.ItemsSource = pokoje.AsDataView();

            ListaPokoi.UnselectAll();
        }

        private void Zatwierdz_OnClick(object sender, RoutedEventArgs e)
        {

            if (_wybranyGosc == 0)
            {

                if ( NumerDowodu.Text == "" || Imie.Text == "" || Nazwisko.Text == "" || Telefon.Text == "" ||
                    DataOd.SelectedDate < DateTime.Now || DataDo.SelectedDate < DataOd.SelectedDate ||
                    ListaPokoi.SelectedItems.Count == 0 || DataOd.SelectedDate == null || DataDo.SelectedDate == null)
                {

                    MessageBox.Show("Proszę wprowadzić poprawne dane gościa.");

                }
                else if (NumerDowodu.Text.Length != 9)
                {

                    MessageBox.Show("Wprowadzono zły numer dowodu osobistego");

                }
                else
                {

                    DataTable goscie = _dataSet.Tables["goscie"];

                    var id = from gosc in goscie.AsEnumerable()
                        where gosc.Field<string>("numer_dowodu") == NumerDowodu.Text.ToUpper()
                        select gosc.Field<int>("id_goscia");

                    if (id.Count() != 0)
                    {
                        MessageBox.Show($"Podany gość jest już w bazie id: {id.First().ToString()}");
                    }
                    else
                    {
                        reloadGoscie();
                        goscie = _dataSet.Tables["goscie"];
                        DataRow row = goscie.NewRow();
                        row[1] = Imie.Text;
                        row[2] = Nazwisko.Text;
                        row[3] = NumerDowodu.Text.ToUpper();
                        row[4] = Ulica.Text;
                        row[5] = Miasto.Text;
                        row[6] = Telefon.Text;
                        row[7] = Email.Text;

                        goscie.Rows.Add(row);

                        var polecenieUpdate = new SqlCommandBuilder(_adapter);
                        _adapter.InsertCommand = polecenieUpdate.GetInsertCommand(true);
                        _adapter.Update(_dataSet,"goscie");

                        reloadGoscie();

                        goscie = _dataSet.Tables["goscie"];

                        var idWpowadzonego = from gosc in goscie.AsEnumerable()
                            where gosc.Field<string>("numer_dowodu") == NumerDowodu.Text.ToUpper()
                            select gosc.Field<int>("id_goscia");

                        _wybranyGosc = idWpowadzonego.First();

                        MessageBox.Show("Dodano nowego gościa");

                        DodajRezerwacje();
                    }  
                }

            }
            else
            {

                if (DataOd.SelectedDate < DateTime.Now || DataDo.SelectedDate < DataOd.SelectedDate ||
                    ListaPokoi.SelectedItems.Count == 0 || DataOd.SelectedDate == null || DataDo.SelectedDate == null)
                {

                    MessageBox.Show("Proszę wprowadzić poprawne dane");

                }
                else
                {
                    DodajRezerwacje();
                }

            }

        }

        private void DataOd_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if(DataOd.SelectedDate != null)
                _dataOd = (DateTime) DataOd.SelectedDate;

            if (_dataOd != DateTime.MinValue && _dataDo != DateTime.MinValue)
            {

                ZmienDostepnePokoje();

            }

        }

        private void DataDo_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (DataDo.SelectedDate != null)
                _dataDo = (DateTime) DataDo.SelectedDate;

            if (_dataOd != null && _dataDo != null)
            {

                ZmienDostepnePokoje();

            }

        }

        private void ZmienDostepnePokoje()
        {
            DataTable pokoje = _dataSet.Tables["pokoje"];
            DataTable zarazerwowanepokoje = _dataSet.Tables["zarezerwowane"];
            DataTable rezerwacje = _dataSet.Tables["rezerwacje"];

            var niedostepnePokoje = from pokoj in pokoje.AsEnumerable()
                join zarezerwowane in zarazerwowanepokoje.AsEnumerable() on pokoj.Field<int>("id_pokoju") equals
                    zarezerwowane.Field<int>("id_pokoju")
                join rezerwacja in rezerwacje.AsEnumerable() on zarezerwowane.Field<int>("id_rezerwacji") equals
                    rezerwacja.Field<int>("id_rezerwacji")
                where (rezerwacja.Field<DateTime?>("od") >= _dataOd &&
                      rezerwacja.Field<DateTime?>("do") >= _dataDo &&
                      rezerwacja.Field<DateTime?>("od") <= _dataDo) ||

                      (rezerwacja.Field<DateTime?>("od") >= _dataOd &&
                       rezerwacja.Field<DateTime?>("do") <= _dataDo) ||

                      (rezerwacja.Field<DateTime?>("od") <= _dataOd &&
                       rezerwacja.Field<DateTime?>("do") >= _dataDo) ||

                      (rezerwacja.Field<DateTime?>("od") <= _dataOd &&
                       rezerwacja.Field<DateTime?>("do") <= _dataDo &&
                       rezerwacja.Field<DateTime?>("do") >= _dataOd)
                                    select pokoj.Field<int>("id_pokoju");

            var view = from pokoj in pokoje.AsEnumerable()
                where !(niedostepnePokoje.Contains<int>(pokoj.Field<int>("id_pokoju")))
                select pokoj;

            ListaPokoi.ItemsSource = view.AsDataView();
        }

        public void DodajRezerwacje()
        {
            reloadRezerwacje();

            DataTable rezerwacje = _dataSet.Tables["rezerwacje"];
            DataTable pokoje = _dataSet.Tables["pokoje"];

            List<int> idPokoji = new List<int>();

            foreach (DataRowView pokoj in ListaPokoi.SelectedItems)
            {

                idPokoji.Add((int)pokoj["id_pokoju"]);

            }

            var doZaplaty = from pok in pokoje.AsEnumerable()
                where idPokoji.Contains(pok.Field<int>("id_pokoju"))
                select pok.Field<decimal>("cena_za_dobe");

            decimal wartoscDoZaplaty = doZaplaty.Sum();
            
            DataRow rezerwacja = rezerwacje.NewRow();

            rezerwacja["id_goscia"] = _wybranyGosc;
            rezerwacja["data_rezerwacji"] = DateTime.Now;
            rezerwacja["od"] = _dataOd;
            rezerwacja["do"] = _dataDo;
            rezerwacja["cena_laczna"] = wartoscDoZaplaty;

            rezerwacje.Rows.Add(rezerwacja);

            var polecenieUpdate = new SqlCommandBuilder(_adapter);
            _adapter.InsertCommand = polecenieUpdate.GetInsertCommand(true);
            _adapter.Update(_dataSet, "rezerwacje");

            reloadRezerwacje();
            rezerwacje = _dataSet.Tables["rezerwacje"];

            var idRezerwacji = from rez in rezerwacje.AsEnumerable()
                where rez.Field<int>("id_goscia") == _wybranyGosc && rez.Field<decimal>("cena_laczna") ==
                    doZaplaty.Sum() && rez.Field<DateTime?>("od") == _dataOd && rez.Field<DateTime?>("do") == _dataDo
                select rez.Field<int>("id_rezerwacji");

            if (idRezerwacji.Count() == 1)
            {

                reloadZarezerwowane();
                DataTable zarezerwowane = _dataSet.Tables["zarezerwowane"];

                int idRez = idRezerwacji.First();

                foreach (var idpokoju in idPokoji)
                {

                    DataRow row = zarezerwowane.NewRow();
                    row[0] = idpokoju;
                    row[1] = idRez;

                    zarezerwowane.Rows.Add(row);
                }

                var polecenieUpdate2 = new SqlCommandBuilder(_adapter);
                _adapter.InsertCommand = polecenieUpdate2.GetInsertCommand(true);
                _adapter.Update(_dataSet, "zarezerwowane");

                reloadZarezerwowane();

                PodsumowanieRezerwacji window = new PodsumowanieRezerwacji(_dataSet,idRez);

                window.ShowDialog();
            }

        }

        private void reloadRezerwacje()
        {
            DataTable rezerwacje = _dataSet.Tables["rezerwacje"];
            _dataSet.Tables.Remove(rezerwacje);
            _adapter.SelectCommand.CommandText = "SELECT * FROM rezerwacje";
            _adapter.Fill(_dataSet, "rezerwacje");

        }

        private void reloadGoscie()
        {

            DataTable goscie = _dataSet.Tables["goscie"];
            _dataSet.Tables.Remove(goscie);
            _adapter.SelectCommand.CommandText = "SELECT * FROM goscie";
            _adapter.Fill(_dataSet, "goscie");

        }

        private void reloadZarezerwowane()
        {

            DataTable zarezerwowane = _dataSet.Tables["zarezerwowane"];
            _dataSet.Tables.Remove(zarezerwowane);
            _adapter.SelectCommand.CommandText = "SELECT * FROM zarezerwowane_pokoje";
            _adapter.Fill(_dataSet, "zarezerwowane");

        }
    }
}
