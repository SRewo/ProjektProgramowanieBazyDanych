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
    /// Logika interakcji dla klasy Pokoje.xaml
    /// </summary>
    public partial class Pokoje : MetroWindow
    {
        private DataSet _dataSet;

        public Pokoje()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }

        public Pokoje(DataSet dataSet) : this()
        {
            _dataSet = dataSet;

            DataTable pokojeTable = _dataSet.Tables["pokoje"];

            var view = from pokoj in pokojeTable.AsEnumerable()
                select new
                {
                    id_pokoju = pokoj.Field<int>("id_pokoju"),
                    numer = pokoj.Field<Int16>("numer"),
                    typ = pokoj.Field<string>("typ"),
                    cena_za_dobe = Math.Round(pokoj.Field<decimal>("cena_za_dobe"), 2) + " PLN"
                };

            PokojeGrid.ItemsSource = view;
        }
    }
}
