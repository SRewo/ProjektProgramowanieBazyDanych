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
    /// Logika interakcji dla klasy Goscie.xaml
    /// </summary>
    public partial class Goscie : MetroWindow
    {
        private DataSet _dataSet;
        private SqlDataAdapter _adapter;

        public Goscie()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }

        public Goscie(DataSet dataSet, SqlDataAdapter adapter) : this()
        {
            _dataSet = dataSet;
            _adapter = adapter;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            reloadGoscie();
            DataTable goscieTabela = _dataSet.Tables["goscie"];
            GoscieGrid.ItemsSource = goscieTabela.DefaultView;

        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            SqlCommandBuilder updateCommand = new SqlCommandBuilder(_adapter);
            try
            {
                _adapter.Update(_dataSet, "goscie");
                MessageBox.Show("Pomyślnie zaktualizowane dane.");
            }
            catch (SqlException)
            {

                MessageBox.Show("Podano nieprawidłowe dane!");
            }
        }

        private void reloadGoscie()
        {

            DataTable goscie = _dataSet.Tables["goscie"];
            _dataSet.Tables.Remove(goscie);
            _adapter.SelectCommand.CommandText = "SELECT * FROM goscie";
            _adapter.Fill(_dataSet, "goscie");

        }
    }
}
