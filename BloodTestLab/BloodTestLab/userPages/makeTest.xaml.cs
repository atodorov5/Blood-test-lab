using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

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
            bloodTypeCBLoad();
            loadTestsInListView();
        }

        public void bloodTypeCBLoad()
        {
            bloodTypeCB.Items.Clear();
            var con = DBConfig.Connection;

            try
            {   con.Open();
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

        private void clearSelected(object sender, RoutedEventArgs e)
        {
            testList.UnselectAll();
        }

        private void loadTestsInListView()
        {
            using (var conn = DBConfig.Connection)
            {
                MySqlCommand sql_cmd = conn.CreateCommand();
                sql_cmd.CommandText = "SELECT idTestType,testtype.name FROM testtype";
                MySqlDataAdapter DB = new MySqlDataAdapter(sql_cmd.CommandText, conn);
                DataTable dt = new DataTable();
                DB.Fill(dt);
                testList.ItemsSource = dt.DefaultView;
            }
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

        private void testBtn(object sender, RoutedEventArgs e)
        {
            /*
             string a = "";
             foreach (DataRowView item in testList.SelectedItems)
             {
                a += item["idTestType"]+" ";
             }
             MessageBox.Show(a);


             foreach (object selectedItem in testList.SelectedItems)
             {
                 DataRowView dr = (DataRowView)selectedItem;
                 var item = dr["name"];
                 MessageBox.Show(item + Environment.NewLine);
             }
 */         int testId;
            if (!String.IsNullOrEmpty(pinTB.Text))
            {

                using (var conn = DBConfig.Connection)
                {
                    MySqlTransaction transaction;
                    conn.Open();
                    transaction = conn.BeginTransaction();
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand("insertTest", conn);
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
                        cmd.Parameters.Add(new MySqlParameter("o_testid", MySqlDbType.Int32));
                        cmd.Parameters[6].Direction = System.Data.ParameterDirection.Output;

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

                        cmd.ExecuteNonQuery();
                        testId = (int)cmd.Parameters["o_testid"].Value;

                        MySqlCommand cmd2 = new MySqlCommand("insertResult", conn);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new MySqlParameter("p_value", MySqlDbType.Double));
                        cmd2.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                        cmd2.Parameters.Add(new MySqlParameter("p_idTest", MySqlDbType.Int32));
                        cmd2.Parameters[1].Direction = System.Data.ParameterDirection.Input;
                        cmd2.Parameters.Add(new MySqlParameter("p_idTestType", MySqlDbType.Int32));
                        cmd2.Parameters[2].Direction = System.Data.ParameterDirection.Input;

                        BloodTestMachine machine = new BloodTestMachine();

                        foreach (DataRowView item in testList.SelectedItems)
                        {
                            cmd2.Parameters["p_value"].Value = BloodTestMachine.analyse((int)item["idTestType"]);
                            cmd2.Parameters["p_idTest"].Value = testId;
                            cmd2.Parameters["p_idTestType"].Value = item["idTestType"];
                            cmd2.ExecuteNonQuery();

                        }

                        transaction.Commit();
                        MessageBox.Show("Успешно направен тест! Дължима сума: " + calculatePrice(testId));

                    }
                    catch (MySqlException ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Грешка " + ex);
                    }
                }
                nameTB.Clear(); lastnameTB.Clear(); bloodTypeCB.SelectedIndex = -1; pinTB.Clear(); testList.UnselectAll();
            }
            else
                MessageBox.Show("Въведете ЕГН!");
        }


        private double calculatePrice(int testID)
        {
            using (var conn = new MySqlConnection("server=localhost;user id=root;password=root;database=bloodlab;"))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("calcPrice", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("p_testID", MySqlDbType.VarChar));
                    cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new MySqlParameter("o_total", MySqlDbType.Double));
                    cmd.Parameters[1].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters[0].Value = testID;
                    cmd.ExecuteNonQuery();
                    return (double)cmd.Parameters["o_total"].Value;
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Грешка " + ex);
                    return 0;
                }
            }
        }
    }
}
