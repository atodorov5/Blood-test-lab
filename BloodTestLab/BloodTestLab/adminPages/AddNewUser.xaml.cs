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

namespace BloodTestLab
{
    /// <summary>
    /// Interaction logic for AddNewUser.xaml
    /// </summary>
    public partial class AddNewUser : Page
    {
        public AddNewUser()
        {
            InitializeComponent();
            loadlabComboBox();
        }

        private void addUser(object sender, RoutedEventArgs e)
        {
            using (var conn = DBConfig.Connection)
            {
                String salt = CreateSalt(10);
                String hashedpass = GenerateSHA256Hash(labPasswrdTB.Text, salt);
                try
                {
                    conn.Open();
                    // 1.  create a command object identifying the stored procedure
                    MySqlCommand cmd = new MySqlCommand("addLabAssistant", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new MySqlParameter("p_labname", MySqlDbType.VarChar));
                    cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new MySqlParameter("p_labLastName", MySqlDbType.VarChar));
                    cmd.Parameters[1].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new MySqlParameter("p_username", MySqlDbType.VarChar));
                    cmd.Parameters[2].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new MySqlParameter("p_hashedPaswrd", MySqlDbType.VarChar));
                    cmd.Parameters[3].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new MySqlParameter("p_salt", MySqlDbType.VarChar));
                    cmd.Parameters[4].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new MySqlParameter("p_clinic", MySqlDbType.VarChar));
                    cmd.Parameters[5].Direction = System.Data.ParameterDirection.Input;

                    cmd.Parameters[0].Value = labNameTB.Text;
                    cmd.Parameters[1].Value = labLastNameTB.Text;
                    cmd.Parameters[2].Value = labUsernameTB.Text;
                    cmd.Parameters[3].Value = hashedpass;
                    cmd.Parameters[4].Value = salt;
                    cmd.Parameters[5].Value = labCB.Text;

                    // execute the command
                    cmd.ExecuteNonQuery();
                } catch (MySqlException ex) {
                    MessageBox.Show("Грешка "+ ex);
                }
                MessageBox.Show("Успешно добавен лаборант!");

            }
        }

        public void loadlabComboBox()
        {
            labCB.Items.Clear();
            var con = DBConfig.Connection;
                try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            try
            {
                MySqlDataAdapter category_data = new MySqlDataAdapter("SELECT * FROM clinicbranch", con);
                DataSet ds = new DataSet();
                category_data.Fill(ds, "clinicbranch");

                labCB.DataContext = ds.Tables["clinicbranch"].DefaultView;
               // labCB.DisplayMemberPath = "idClinicBranch";
                labCB.DisplayMemberPath = "address";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public String CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public String GenerateSHA256Hash(String input, String salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            System.Security.Cryptography.SHA256Managed sha256hashsring = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256hashsring.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
