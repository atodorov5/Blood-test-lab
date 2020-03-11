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
    /// Interaction logic for donationrReference.xaml
    /// </summary>
    public partial class donationrReference : Page
    {
        public donationrReference()
        {
            InitializeComponent();
        }

        private void loadDonationsDataGrid(object sender, RoutedEventArgs e)
        {
            using (var conn = DBConfig.Connection)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("donationRef", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("p_pin", MySqlDbType.VarChar));
                    cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters[0].Value = pinTB.Text;

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd.CommandText, conn);
                    DataTable dt = new DataTable();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    dt.Columns["idDonation"].ColumnName = "№";
                    dt.Columns["donationDate"].ColumnName = "Дата";
                    dt.Columns["destination"].ColumnName = "Дeстинация";
                    dt.Columns["labLastName"].ColumnName = "Лаборант";

                    //dt.DefaultView.Sort = "Дата на тест";
                    DonationsDG.ItemsSource = dt.DefaultView;
                }
                catch (MySqlException ex)
                {
                    if (ex.ErrorCode == -2147467259)
                    {
                        MessageBox.Show("Некоректно въведено ЕГН!");
                    }
                    else
                        MessageBox.Show("Грешка " + ex);
                }

            }

        }
    }
}
