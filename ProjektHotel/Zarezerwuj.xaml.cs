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
    /// Logika interakcji dla klasy Zarezerwuj.xaml
    /// </summary>
    public partial class Zarezerwuj : Window
    {
        private DataSet _dataSet;
        private int _wybranyGosc;
        private SqlDataAdapter _adapter;

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
            DataTable zarazerwowanepokoje = _dataSet.Tables["zarezewowane"];
            DataTable rezerwacje = _dataSet.Tables["rezerwacje"];

            var niedostepnePokoje = from pokoj in pokoje.AsEnumerable()
                join zarezerwowane in zarazerwowanepokoje.AsEnumerable() on pokoj.Field<int>("id_pokoju") equals
                    zarezerwowane.Field<int>("id_pokoju")
                join rezerwacja in rezerwacje.AsEnumerable() on zarezerwowane.Field<int>("id_rezerwacji") equals
                    rezerwacja.Field<int>("id_rezerwacji")
                where rezerwacja.Field<DateTime?>("od") < DateTime.Now ||
                      rezerwacja.Field<DateTime?>("do") > DateTime.Now
                select pokoj.Field<int>("id_pokoju");

            var view = from pokoj in pokoje.AsEnumerable()
                where !(niedostepnePokoje.Contains<int>(pokoj.Field<int>("id_pokoju")))
                select pokoj;

            ListaPokoi.ItemsSource = view.AsDataView();

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
            ListaPokoi.UnselectAll();
        }

        private void Zatwierdz_OnClick(object sender, RoutedEventArgs e)
        {

            if (_wybranyGosc == 0)
            {

                if (NumerDowodu.Text == "" || Imie.Text == "" || Nazwisko.Text == "" || Telefon.Text == "") 
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
                    DataRow row = goscie.NewRow();

                    row[1] = Imie.Text;
                    row[2] = Nazwisko.Text;
                    row[3] = NumerDowodu.Text.ToUpper();
                    row[4] = Ulica.Text;
                    row[5] = Miasto.Text;
                    row[6] = Telefon.Text;
                    row[7] = Email.Text;

                    

                    var id = from gosc in goscie.AsEnumerable()
                        where gosc.Field<string>("numer_dowodu") == NumerDowodu.Text.ToUpper()
                        select gosc.Field<int>("id_goscia");

                    if (id.Count() != 0)
                    {
                        MessageBox.Show($"Podany gość jest już w bazie id: {id.First().ToString()}");
                    }
                    else
                    {
                        goscie.Rows.Add(row);

                        var polecenieUpdate = new SqlCommandBuilder(_adapter);
                        _adapter.InsertCommand = polecenieUpdate.GetInsertCommand(true);
                        _adapter.Update(_dataSet,"goscie");

                        _dataSet.Tables.Remove(goscie);
                        _adapter.SelectCommand.CommandText = "SELECT * FROM goscie";
                        _adapter.Fill(_dataSet, "goscie");

                        var idWpowadzonego = from gosc in goscie.AsEnumerable()
                            where gosc.Field<string>("numer_dowodu") == NumerDowodu.Text.ToUpper()
                            select gosc.Field<int>("id_goscia");

                        _wybranyGosc = idWpowadzonego.First();

                    }  
                }

            }
            else
            {
                MessageBox.Show(_wybranyGosc.ToString());
            }

        }
    }
}
