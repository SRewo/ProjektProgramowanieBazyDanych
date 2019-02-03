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
using MahApps.Metro.Controls;

namespace ProjektHotel
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class Logowanie : MetroWindow
    {

        private string _login;
        private string _password;
        private SqlConnectionStringBuilder _builder;
        private DataSet _dataSet;
        private SqlDataAdapter _adapter;
    
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
                    Menu windowMenu = new Menu(_dataSet,_adapter);
                    windowMenu.Show();
                    Close();
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
            _builder.DataSource = "localhost";         //"RW\\SQLEXPRESS"  // MAREK0E2D\WINDOWSSERVER
            _builder.InitialCatalog = "hotel";          // hotel2 - database-name Marek
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
            catch (SqlException)
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
            adapter.Fill(dataSet, "zarezerwowane");

            _adapter = adapter;

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
