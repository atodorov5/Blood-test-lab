using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;

namespace BloodTestLab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

       
        public String CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public String GenerateSHA256Hash(String input,String salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            System.Security.Cryptography.SHA256Managed sha256hashsring = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256hashsring.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private void login(object sender, RoutedEventArgs e)
        {
            string salt="", hashedpass="";
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;password=root;database=bloodlab;"))
            {
                try
                {

                    if (usernameTB.Text == "admin" && password.Password.ToString() == "admin")
                    {
                        AdminWindow adminW = new AdminWindow();
                        adminW.Show();
                         this.Close();
                    }
                    else if (usernameTB.Text!=""){


                        conn.Open();
                        // 1.  create a command object identifying the stored procedure
                        MySqlCommand cmd = new MySqlCommand("loginProcedure", conn);

                        // 2. set the command object so it knows to execute a stored procedure
                        cmd.CommandType = CommandType.StoredProcedure;

                        // 3. add parameter to command, which will be passed to the stored procedure
                        cmd.Parameters.Add(new MySqlParameter("p_usrname", MySqlDbType.VarChar));
                        cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("o_salt", MySqlDbType.VarChar));
                        cmd.Parameters[1].Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(new MySqlParameter("o_hashedPass", MySqlDbType.VarChar));
                        cmd.Parameters[2].Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(new MySqlParameter("o_id", MySqlDbType.Int32));
                        cmd.Parameters[3].Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters[0].Value = usernameTB.Text;
                        cmd.ExecuteReader(); // execute the command
                       
                            salt = (string)cmd.Parameters["o_salt"].Value;
                            hashedpass = (string)cmd.Parameters["o_hashedPass"].Value;
                            //int usrID = (int)cmd.Parameters["o_id"].Value;

                        if (GenerateSHA256Hash(password.Password.ToString(), salt) == hashedpass){
                            GlobalInfo.CurrentUser = new UserInfo((int)cmd.Parameters["o_id"].Value);
                            UserWindow userW = new UserWindow();
                            userW.Show();
                            this.Close();
                    }
                        else
                            loginError.Content = "Грешно потребителско име или парола!";
 
                    }
                    else
                        loginError.Content = "Грешно потребителско име или парола!";

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Грешка " + ex);
                }

            }
        }
    }
}
