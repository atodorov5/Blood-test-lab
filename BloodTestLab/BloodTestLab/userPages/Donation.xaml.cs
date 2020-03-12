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
    /// Interaction logic for Donation.xaml
    /// </summary>
    public partial class Donation : Page
    {
        public Donation()
        {
            InitializeComponent();
            bloodTypeCBLoad();
        }

        private void checkPatient(object sender, RoutedEventArgs e)
        {
            nameTB.Clear(); lastnameTB.Clear(); bloodTypeCB.SelectedIndex = -1;
            using (var conn = DBConfig.Connection)
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("checkPatient", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("p_pin", MySqlDbType.VarChar));
                    cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new MySqlParameter("o_name", MySqlDbType.VarChar));
                    cmd.Parameters[1].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(new MySqlParameter("o_lastname", MySqlDbType.VarChar));
                    cmd.Parameters[2].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add("o_sex", MySqlDbType.Bit, 1);
                    cmd.Parameters[3].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(new MySqlParameter("o_bloodType", MySqlDbType.Int32));
                    cmd.Parameters[4].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters[0].Value = pinTB.Text;


                    // execute the command
                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        nameTB.Text = (string)cmd.Parameters["o_name"].Value;
                        lastnameTB.Text = (string)cmd.Parameters["o_lastname"].Value;
                        if (Convert.ToBoolean(cmd.Parameters["o_sex"].Value))
                            femaleRB.IsChecked = true;
                        else
                            maleRB.IsChecked = true;
                        bloodTypeCB.SelectedIndex = (int)cmd.Parameters["o_bloodType"].Value - 1;

                    }
                    else
                        MessageBox.Show("Добявете пациент!");

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Грешка " + ex);
                }

            }
        }

        public void bloodTypeCBLoad()
        {
            bloodTypeCB.Items.Clear();
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
            con.Close();
        }


        private void donateBtn(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(pinTB.Text))
            {
                using (var conn = DBConfig.Connection)
                {
                    MySqlTransaction transaction;
                    conn.Open();
                    transaction = conn.BeginTransaction();
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand("insertDonation", conn);
                        cmd.Transaction = transaction;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new MySqlParameter("p_name", MySqlDbType.VarChar));
                        cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_lastName", MySqlDbType.VarChar));
                        cmd.Parameters[1].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_pin", MySqlDbType.VarChar));
                        cmd.Parameters[2].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add("p_sex", MySqlDbType.Bit, 1);
                        cmd.Parameters[3].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_bloodType", MySqlDbType.Int32));
                        cmd.Parameters[4].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_LabAssistantID", MySqlDbType.Int32));
                        cmd.Parameters[5].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_destination", MySqlDbType.VarChar));
                        cmd.Parameters[6].Direction = System.Data.ParameterDirection.Input;

                        cmd.Parameters[0].Value = nameTB.Text;
                        cmd.Parameters[1].Value = lastnameTB.Text;
                        cmd.Parameters[2].Value = pinTB.Text;
                        if (femaleRB.IsChecked == true)
                            cmd.Parameters[3].Value = true;
                        else
                            cmd.Parameters[3].Value = false;
                        cmd.Parameters[4].Value = bloodTypeCB.SelectedIndex + 1;
                        UserInfo userInfo = GlobalInfo.CurrentUser;
                        cmd.Parameters[5].Value = userInfo.UserID;
                        cmd.Parameters[6].Value = destinationTB.Text;
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        MessageBox.Show("Успешно записано даряване!");



                    }
                    catch (MySqlException ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Грешка " + ex);
                    }

                }
                nameTB.Clear(); lastnameTB.Clear(); bloodTypeCB.SelectedIndex = -1; destinationTB.Clear(); pinTB.Clear();
            }
            else
                MessageBox.Show("Въведете данни!");
        }
    }
}
