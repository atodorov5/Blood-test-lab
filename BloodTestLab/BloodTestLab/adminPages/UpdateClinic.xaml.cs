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

namespace BloodTestLab.adminPages
{
    /// <summary>
    /// Interaction logic for UpdateClinic.xaml
    /// </summary>
    public partial class UpdateClinic : Page
    {
        public UpdateClinic()
        {
            InitializeComponent();
            loadClinicInDataGrid();
        }

        private void Button_UpdateClinic(object sender, RoutedEventArgs e)
        {
           
            DataRowView row = (DataRowView)clinicDG.SelectedItem;
            if (row != null)
            {
                using (var conn = DBConfig.Connection)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("UpdateClinic", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new MySqlParameter("p_clinicID", MySqlDbType.Int32));
                        cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_town", MySqlDbType.VarChar));
                        cmd.Parameters[1].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_address", MySqlDbType.VarChar));
                        cmd.Parameters[2].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_license", MySqlDbType.VarChar));
                        cmd.Parameters[3].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters[0].Value = (int)row["№"];
                        if (!string.IsNullOrWhiteSpace(townTB.Text))
                            cmd.Parameters[1].Value = townTB.Text;
                        if (!string.IsNullOrWhiteSpace(addressTB.Text))
                            cmd.Parameters[2].Value = addressTB.Text;
                        if (!string.IsNullOrWhiteSpace(licenseTB.Text))
                            cmd.Parameters[3].Value = licenseTB.Text;

                        cmd.ExecuteNonQuery();
                        loadClinicInDataGrid();
                        townTB.Clear();
                        licenseTB.Clear();
                        addressTB.Clear();

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Грешка " + ex);
                    }

                }
            }
            else
                MessageBox.Show("Изберете клиника!");
        }

        private void loadClinicInDataGrid()
        {
            string sql = "SELECT * from clinicbranch";

            var connection = DBConfig.Connection;
            MySqlCommand cmdSel = new MySqlCommand(sql, connection);
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            da.Fill(dt);
            dt.Columns["idClinicBranch"].ColumnName = "№";
            dt.Columns["town"].ColumnName = "Град";
            dt.Columns["address"].ColumnName = "Адрес";
            dt.Columns["license"].ColumnName = "Линенз";
            clinicDG.DataContext = dt;
        }
    }
}
