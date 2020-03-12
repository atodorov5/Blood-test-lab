using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

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
            if (String.IsNullOrEmpty(labNameTB.Text) || String.IsNullOrEmpty(labLastNameTB.Text) || String.IsNullOrEmpty(labUsernameTB.Text) || String.IsNullOrEmpty(labPasswrdTB.Text) || labCB.SelectedItem == null)
                MessageBox.Show("Въведете данни!");
            else
            {
                using (var conn = DBConfig.Connection)
                {
                    String salt = Password.CreateSalt(10);
                    String hashedpass = Password.GenerateSHA256Hash(labPasswrdTB.Text, salt);
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("addLabAssistant", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

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
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Грешка " + ex);
                    }
                    MessageBox.Show("Успешно добавен лаборант!");
                }
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
            con.Close();
        }
    }
}
