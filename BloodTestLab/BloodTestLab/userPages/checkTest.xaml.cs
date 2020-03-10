using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BloodTestLab.userPages
{
    /// <summary>
    /// Interaction logic for checkTest.xaml
    /// </summary>
    public partial class checkTest : Page
    {
        public checkTest()
        {
            InitializeComponent();
        }
        
        private void checkTestsByPin(object sender, RoutedEventArgs e)
        {
            using (var conn = DBConfig.Connection)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("checkPinTest", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("p_pin", MySqlDbType.VarChar));
                    cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters[0].Value = pinTB.Text;

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd.CommandText, conn);
                    DataTable dt = new DataTable();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    dt.Columns["idTest"].ColumnName = "№";
                    dt.Columns["testDate"].ColumnName = "Дата на тест";
                    testsDG.ItemsSource = dt.DefaultView;
                }
                catch (MySqlException ex)
                {
                    if (ex.ErrorCode == -2147467259)
                    {
                        MessageBox.Show("Некоректно въведено ЕГН!");
                    }
                    else
                    MessageBox.Show("Грешка " + ex);
                }

            }
        }

        private void resultCheck(object sender, MouseButtonEventArgs e)
        {
            
             DataRowView rowview = testsDG.SelectedItem as DataRowView;
            // MessageBox.Show(rowview.Row[0].ToString());
            loadResults((int)rowview.Row[0]);
            /*
             var newMyWindow2 = new Result("Your id name: "+rowview.Row[1].ToString());
             newMyWindow2.Show();
            //RABOTI

           



            // Some operations with this row
            /*
                        DataRowView dataRowView = (DataRowView)dt.SelectedItem;
                         int ID = Convert.ToInt32(dataRowView.Row[0]);

                      DataGridRow row = (DataGridRow)sender;
                        DataRow dr = (DataRow)row.DataContext;

                        string value = dr[1].ToString();
                        MessageBox.Show(value);*/
        }

        private void loadResults(int testId)
        {
            using (var conn = DBConfig.Connection)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("loadResults", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("p_testid", MySqlDbType.VarChar));
                    cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters[0].Value = testId;

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd.CommandText, conn);
                    DataTable dt = new DataTable();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    dt.Columns["name"].ColumnName = "Тест";
                    dt.Columns["value"].ColumnName = "Стойност";
                    dt.Columns["minValue"].ColumnName = "Минимално";
                    dt.Columns["maxValue"].ColumnName = "Максимално";
                    dt.Columns["unit"].ColumnName = "Единица";

                    dt.Columns.Add("Статус", typeof(string));

                    int columnNumber = 5; 

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if ((double)dt.Rows[i][1] > (double)dt.Rows[i][3])
                            dt.Rows[i][columnNumber] = "+";
                        else if ((double)dt.Rows[i][1] < (double)dt.Rows[i][2])
                            dt.Rows[i][columnNumber] = "-";
                    }

                    resultDG.ItemsSource = dt.DefaultView;
                }
                catch (MySqlException ex)
                {
                    if (ex.ErrorCode == -2147467259)
                    {
                        MessageBox.Show("Некоректно въведено ЕГН!");
                    }
                    else
                        MessageBox.Show("Грешка " + ex);
                }

            }
        }


    }
}
