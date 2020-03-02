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
            MySqlConnection conn = new MySqlConnection("server=localhost;database=bloodlab;uid=root;pwd=root");

            MySqlCommand cmd = null;
            string cmdString = "";
            conn.Open();

            String salt = CreateSalt(10);
            String hashedpass = GenerateSHA256Hash(labPasswrdTB.Text, salt);

            cmdString = "insert into labassistant(labName,labLastName,username,hashedPassWrd,salt,ClinicBranch_idClinicBranch) values('" + labNameTB.Text + "','" + labLastNameTB.Text +  "','" + labUsernameTB.Text+ "','" + hashedpass + "','" + salt + "','" + labCB.SelectedItem.ToString() + "')";

            cmd = new MySqlCommand(cmdString, conn);

            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Data Stored Successfully");


        }

        public void loadlabComboBox()
        {
            labCB.Items.Clear();
            MySqlConnection con = new MySqlConnection("server=localhost;database=bloodlab;uid=root;pwd=root");
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
                labCB.DisplayMemberPath = "idClinicBranch";
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
