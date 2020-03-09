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
    /// Interaction logic for ProfileInfo.xaml
    /// </summary>
    public partial class ProfileInfo : Page
    {
        public ProfileInfo()
        {
            InitializeComponent();
            loadInfo();
     
        }


        private void loadInfo()
        {
            using (var conn = new MySqlConnection("server=localhost;user id=root;password=root;database=bloodlab;"))
            {
                /*MySqlCommand sql_cmd = conn.CreateCommand();
                sql_cmd.CommandText = "SELECT * FROM labassistant join test on ClinicBranch_idClinicBranch=idClinicBranch;";
                MySqlDataAdapter DB = new MySqlDataAdapter(sql_cmd.CommandText, conn);
                DataTable dt = new DataTable();
                DB.Fill(dt);*/
              
            }
        }
    }
}
