using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace BloodTestLab.userPages
{
    /// <summary>
    /// Interaction logic for DonationPeriod.xaml
    /// </summary>
    public partial class DonationPeriod : Page
    {
        public DonationPeriod()
        {
            InitializeComponent();
        }

        private void Button_DonationPeriod(object sender, RoutedEventArgs e)
        {
            using (var conn = DBConfig.Connection)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("donationPeriod", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("p_from", MySqlDbType.DateTime));
                    cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new MySqlParameter("p_to", MySqlDbType.DateTime));
                    cmd.Parameters[1].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters[0].Value = dateFrom.SelectedDate;
                    cmd.Parameters[1].Value = dateTo.SelectedDate;

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd.CommandText, conn);
                    DataTable dt = new DataTable();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    dt.Columns["idDonation"].ColumnName = "№";
                    dt.Columns["donationDate"].ColumnName = "Дата";
                    dt.Columns["destination"].ColumnName = "Дeстинация";
                    dt.Columns["username"].ColumnName = "Лаборант";
                    dt.Columns["name"].ColumnName = "Донор";
                    dt.Columns["pin"].ColumnName = "ЕГН";

                    //dt.DefaultView.Sort = "Дата на тест";
                    DonationsDG.ItemsSource = dt.DefaultView;
                }
                catch (MySqlException ex)
                {
                   
                        MessageBox.Show("Грешка " + ex);
                }

            }
        }
    }
}
