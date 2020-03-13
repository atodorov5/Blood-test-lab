using MySql.Data.MySqlClient;
using System.ComponentModel;
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
            resultDG.ItemsSource = null;
            name.Content = "";
            lastName.Content = "";
            using (var conn = DBConfig.Connection)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("checkPinTest", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("p_pin", MySqlDbType.VarChar));
                    cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new MySqlParameter("p_name", MySqlDbType.VarChar));
                    cmd.Parameters[1].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters[0].Value = pinTB.Text;

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd.CommandText, conn);
                    DataTable dt = new DataTable();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    dt.Columns["idTest"].ColumnName = "№";
                    dt.Columns["testDate"].ColumnName = "Дата на тест";
                    testsDG.ItemsSource = dt.DefaultView;
                    testsDG.Columns[2].Visibility = Visibility.Collapsed;
                    testsDG.Columns[3].Visibility = Visibility.Collapsed;
                        SortDataGrid(testsDG);
                    if (dt.Rows.Count > 0)
                    {
                        name.Content = dt.Rows[0]["name"].ToString();
                        lastName.Content = dt.Rows[0]["lastName"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Грешно ЕГН!");
                    }
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
            loadResults((int)rowview.Row[0]);
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


        public static void SortDataGrid(DataGrid dataGrid, int columnIndex = 1, ListSortDirection sortDirection = ListSortDirection.Descending)
        {
            var column = dataGrid.Columns[columnIndex];
            dataGrid.Items.SortDescriptions.Clear();
            dataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, sortDirection));
            foreach (var col in dataGrid.Columns)
            {
                col.SortDirection = null;
            }
            column.SortDirection = sortDirection;
            dataGrid.Items.Refresh();
        }

    }
}
