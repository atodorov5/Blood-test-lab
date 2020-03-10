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
            using (var conn = DBConfig.Connection)
            {
                MySqlCommand sql_cmd = conn.CreateCommand();
                sql_cmd.CommandText = "SELECT * FROM labassistant join clinicbranch on ClinicBranch_idClinicBranch=idClinicBranch;";
                MySqlDataAdapter DB = new MySqlDataAdapter(sql_cmd.CommandText, conn);
                DataTable dt = new DataTable();
                DB.Fill(dt);
                usrnameL.Content = dt.Rows[0]["username"].ToString();
                nameL.Content = dt.Rows[0]["labName"].ToString();
                lastNameL.Content = dt.Rows[0]["labLastName"].ToString();
                clinicLicenseL.Content = dt.Rows[0]["license"].ToString();
                clinictownL.Content = dt.Rows[0]["town"].ToString();
                clinicaddressL.Content = dt.Rows[0]["address"].ToString();
                
            }
        }
    }
}
