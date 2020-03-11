using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
    /// Interaction logic for removeClinic.xaml
    /// </summary>
    public partial class removeClinic : Page
    {
        public removeClinic()
        {
            InitializeComponent();
            loadClinics();
        }


        private void loadClinics()
        {
            using (var conn = DBConfig.Connection)
            {
                MySqlCommand sql_cmd = conn.CreateCommand();
                sql_cmd.CommandText = "SELECT * FROM clinicbranch";
                MySqlDataAdapter DB = new MySqlDataAdapter(sql_cmd.CommandText, conn);
                DataTable dt = new DataTable();
                DB.Fill(dt);
                dt.Columns["idClinicBranch"].ColumnName = "№";
                dt.Columns["town"].ColumnName = "Град";
                dt.Columns["address"].ColumnName = "Адрес";
                dt.Columns["license"].ColumnName = "Лиценз";
                clinicDG.DataContext = dt.DefaultView;
            }
        }

        private void Button_removeClinic(object sender, RoutedEventArgs e)
        {
            DataRowView rowview = clinicDG.SelectedItem as DataRowView;
          
            using (var conn = DBConfig.Connection)
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("removeClinic", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("p_clinicID", MySqlDbType.Int32));
                    cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters[0].Value = (int)rowview.Row[0];
                    if(cmd.ExecuteNonQuery()>0)
                    MessageBox.Show("Успешно премахната клиника ");
                    else
                        MessageBox.Show("Клиниката не може да бъде премахната");

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Грешка " + ex);
                }
                
                loadClinics();
            }
            
        }
    }
}
