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
    /// Interaction logic for EditPassword.xaml
    /// </summary>
    public partial class EditPassword : Page
    {

        private string salt;
        private string oldHashedPasswrd;

        public EditPassword()
        {
            InitializeComponent();
            loadInfo();
        }

        private void Button_EditPassword(object sender, RoutedEventArgs e)
        {
            UserInfo userInfo = GlobalInfo.CurrentUser;
            if (Password.GenerateSHA256Hash(oldpassword.Password.ToString(), salt) == oldHashedPasswrd)
            {
                using (var conn = DBConfig.Connection)
                {

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("changePassword", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new MySqlParameter("p_userID", MySqlDbType.Int32));
                    cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new MySqlParameter("p_newPassword", MySqlDbType.VarChar));
                    cmd.Parameters[1].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters[0].Value = userInfo.UserID;
                    cmd.Parameters[1].Value = Password.GenerateSHA256Hash(newpassword.Password.ToString(), salt);
                    cmd.ExecuteReader(); // execute the command
                    MessageBox.Show("Успешно сменена парола!");
                  
                }

                }else
            {
                MessageBox.Show("Навилдна стара парола!");
            }
              loadInfo();


        }

        private void loadInfo()
        {
            using (var conn = DBConfig.Connection)
            {
                UserInfo userInfo = GlobalInfo.CurrentUser;
                MySqlCommand sql_cmd = conn.CreateCommand();
                sql_cmd.CommandText = "SELECT username,hashedPassWrd,salt FROM labassistant where idLabAssistant='" + userInfo.UserID + "';";
                MySqlDataAdapter DB = new MySqlDataAdapter(sql_cmd.CommandText, conn);
                DataTable dt = new DataTable();
                DB.Fill(dt);
                usernameLabel.Content = dt.Rows[0]["username"].ToString();
                salt = dt.Rows[0]["salt"].ToString();
                oldHashedPasswrd = dt.Rows[0]["hashedPassWrd"].ToString();
                conn.Clone();
            }
        }
    }
}
