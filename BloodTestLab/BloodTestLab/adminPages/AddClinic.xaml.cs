using MySql.Data.MySqlClient;
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

namespace BloodTestLab.adminPages
{
    /// <summary>
    /// Interaction logic for AddClinic.xaml
    /// </summary>
    public partial class AddClinic : Page
    {
        public AddClinic()
        {
            InitializeComponent();
        }


        private void addClinicBranchBtn(object sender, RoutedEventArgs e)
        {
            string town = townTB.Text;
            string address = addressTB.Text;
            string license = licenseTB.Text;

            try
            {
                using (var connection = new MySqlConnection("server=localhost;user id=root;password=root;database=bloodlab;"))
                {

                    connection.Open();

             
                    var sqlCommand = "INSERT INTO clinicbranch (town,address,license) VALUES (@town1,@address1,@license1)";


                    using (var command = new MySqlCommand(sqlCommand, connection))
                    {
                        command.Parameters.Add("@town1", MySqlDbType.VarChar).Value = town;
                        command.Parameters.Add("@address1", MySqlDbType.VarChar).Value = address;
                        command.Parameters.Add("@license1", MySqlDbType.VarChar).Value = license;

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    MessageBox.Show("Успешно добавена клиника!");
                    addressTB.Clear();
                    licenseTB.Clear();
                    townTB.Clear();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error Message: " + ex.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
