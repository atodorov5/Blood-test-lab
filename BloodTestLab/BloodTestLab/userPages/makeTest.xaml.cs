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
    /// Interaction logic for makeTest.xaml
    /// </summary>
    public partial class makeTest : Page
    {
        public makeTest()
        {
            InitializeComponent();
            testList.Items.Add("ПКК");
            testList.Items.Add("Биохимия");
            testList.Items.Add("Йонограма");
            testList.Items.Add("Туморни маркери");
            bloodTypeCBLoad();
        }

        public void bloodTypeCBLoad()
        {
            bloodTypeCB.Items.Clear();
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
                MySqlDataAdapter category_data = new MySqlDataAdapter("SELECT * FROM bloodtype", con);
                DataSet ds = new DataSet();
                category_data.Fill(ds, "bloodtype");

                bloodTypeCB.DataContext = ds.Tables["bloodtype"].DefaultView;
                // labCB.DisplayMemberPath = "idClinicBranch";
                bloodTypeCB.DisplayMemberPath = "typeOfBlood";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
