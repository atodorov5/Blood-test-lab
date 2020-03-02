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
    /// Interaction logic for AddTest.xaml
    /// </summary>
    public partial class AddTest : Page
    {
        public AddTest()
        {
            InitializeComponent();
        }


        private void addTestB1tn()
        {
           
           

            }

        private void addTestBtn(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=root;database=bloodlab;"))
            {

                conn.Open();
                // 1.  create a command object identifying the stored procedure
                MySqlCommand cmd = new MySqlCommand("addTestType", conn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new MySqlParameter("p_name", MySqlDbType.VarChar));
                cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                cmd.Parameters.Add(new MySqlParameter("p_minV", MySqlDbType.Double));
                cmd.Parameters[1].Direction = System.Data.ParameterDirection.Input;
                cmd.Parameters.Add(new MySqlParameter("p_maxV", MySqlDbType.Double));
                cmd.Parameters[2].Direction = System.Data.ParameterDirection.Input;
                cmd.Parameters.Add(new MySqlParameter("p_unit", MySqlDbType.VarChar));
                cmd.Parameters[3].Direction = System.Data.ParameterDirection.Input;
                cmd.Parameters.Add(new MySqlParameter("p_price", MySqlDbType.Double));
                cmd.Parameters[4].Direction = System.Data.ParameterDirection.Input;

                cmd.Parameters[0].Value = testNameTB.Text;
                cmd.Parameters[1].Value = Convert.ToDouble( minValueTB.Text);
                cmd.Parameters[2].Value = Convert.ToDouble(maxValueTB.Text);
                cmd.Parameters[3].Value = testUnitTB.Text;
                cmd.Parameters[4].Value = Convert.ToDouble(priceTB.Text);

                // execute the command
                cmd.ExecuteNonQuery();

                MessageBox.Show("Успешно!");
                testNameTB.Clear();
                minValueTB.Clear();
                maxValueTB.Clear();
                testUnitTB.Clear();
                priceTB.Clear();
                
            }
        }
    }
    }

