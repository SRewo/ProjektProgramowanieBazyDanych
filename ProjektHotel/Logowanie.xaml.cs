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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

namespace ProjektHotel
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class Logowanie : Window
    {

        private string _login;
        private string _password;
        private SqlConnectionStringBuilder _builder;
        private DataSet _dataSet;
    
        public Logowanie()
        {
            InitializeComponent();          
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _login = login.Text;
            _password = password.Password;

            //pobieranie użytkownika o danym loginie z bazy

            DataTable users = _dataSet.Tables["uzytkownicy"]; 
            var dbuser = from user in users.AsEnumerable()
                where user.Field<string>("login") == _login
                select user;

            
            if (dbuser.Count() == 1) 
            {
                //pobranie hasła z wyniku wcześniejszego zapytania i zaszyfrowanie wpisanego hasła
                string dbpassword = dbuser.First().Field<string>("password");
                string localpassword = Encrypt(_password);


                if (dbpassword != localpassword)
                {
                    MessageBox.Show("Wprowadzono zły login/hasło.");
                }
                else
                {
                    Menu windowMenu = new Menu(_dataSet);
                    Close();
                    windowMenu.Show();
                }
            }
            else
            {
                MessageBox.Show("Wprowadzono zły login/hasło.");
            }

        }

        private void Logowanie_OnLoaded(object sender, RoutedEventArgs e)
        {

            _builder = new SqlConnectionStringBuilder();
            _builder.DataSource = "RW\\SQLEXPRESS";         //"RW\\SQLEXPRESS"
            _builder.InitialCatalog = "hotel";
            _builder.IntegratedSecurity = true;

            _dataSet = FillDataset(_builder);

        }

        //metoda to pobrania danych z bazy
        private DataSet FillDataset(SqlConnectionStringBuilder builder)
        {
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {

                adapter = new SqlDataAdapter("SELECT * FROM uzytkownicy", builder.ConnectionString);
                adapter.Fill(dataSet, "uzytkownicy");

            }
            catch (SqlException ex)
            {

                MessageBox.Show("Nie udało się połączyć z bazą danych. Aby rozwiązać ten problem skontaktuj się z administratorem systemu.","Błąd bazy danych.",MessageBoxButton.OK,MessageBoxImage.Error);
                Close();
                return null;

            }

            adapter.SelectCommand.CommandText = "SELECT * FROM goscie";
            adapter.Fill(dataSet, "goscie");

            adapter.SelectCommand.CommandText = "SELECT * FROM pokoje";
            adapter.Fill(dataSet, "pokoje");

            adapter.SelectCommand.CommandText = "SELECT * FROM rezerwacje";
            adapter.Fill(dataSet, "rezerwacje");

            adapter.SelectCommand.CommandText = "SELECT * FROM zarezerwowane_pokoje";
            adapter.Fill(dataSet, "zarezewowane");

            DataColumn idGosciaGoscie = dataSet.Tables["goscie"].Columns["id_goscia"];
            DataColumn idGosciaRezezwacje = dataSet.Tables["rezerwacje"].Columns["id_goscia"];
            DataColumn idRezerwacjiRezerwacje = dataSet.Tables["rezerwacje"].Columns["id_rezerwacji"];
            DataColumn idRezerwacjiZarezerwowane = dataSet.Tables["zarezewowane"].Columns["id_rezerwacji"];
            DataColumn idPokojuZarezerwowane = dataSet.Tables["zarezewowane"].Columns["id_pokoju"];
            DataColumn idPokojuPokoje = dataSet.Tables["pokoje"].Columns["id_pokoju"];

            DataRelation relationGoscie = new DataRelation("GoscieRezerwacje",idGosciaGoscie,idGosciaRezezwacje);
            DataRelation relationRezewacje = new DataRelation("RezerwacjeZarezerwowane",idRezerwacjiRezerwacje,idRezerwacjiZarezerwowane);
            DataRelation relationPokoje = new DataRelation("PokojeZarezewowane", idPokojuPokoje, idPokojuZarezerwowane);

            dataSet.Relations.Add(relationGoscie);
            dataSet.Relations.Add(relationRezewacje);
            dataSet.Relations.Add(relationPokoje);

            return dataSet;
        }

        //metoda służąca do zaszyfrowania hasła 
        public static string Encrypt(string password)
        {
            StringBuilder stringBuilder = new StringBuilder();
            byte[] data = Encoding.UTF8.GetBytes(password);
            data = new SHA256Managed().ComputeHash(data);
            foreach (byte b in data)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString().ToUpper();
        }
    }
}
