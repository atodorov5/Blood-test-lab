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

namespace BloodTestLab.adminPages
{
    /// <summary>
    /// Interaction logic for RemoveTest.xaml
    /// </summary>
    public partial class RemoveTest : Page
    {
        public RemoveTest()
        {
            InitializeComponent();
            loadClinics();
        }

        private void loadClinics()
        {
            using (var conn = DBConfig.Connection)
            {
                MySqlCommand sql_cmd = conn.CreateCommand();
                sql_cmd.CommandText = "SELECT * FROM testtype";
                MySqlDataAdapter DB = new MySqlDataAdapter(sql_cmd.CommandText, conn);
                DataTable dt = new DataTable();
                DB.Fill(dt);
                dt.Columns["idTestType"].ColumnName = "№";
                dt.Columns["name"].ColumnName = "Име";
                dt.Columns["minValue"].ColumnName = "Минимално";
                dt.Columns["maxValue"].ColumnName = "Максимално";
                dt.Columns["unit"].ColumnName = "Единица";
                dt.Columns["price"].ColumnName = "Цена";
                testDG.DataContext = dt.DefaultView;
            }
        }

        private void Button_RemoveTest(object sender, RoutedEventArgs e)
        {
            DataRowView rowview = testDG.SelectedItem as DataRowView;
            if (rowview != null)
            {
                using (var conn = DBConfig.Connection)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("removeTestType", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new MySqlParameter("p_testTypeID", MySqlDbType.Int32));
                        cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters[0].Value = (int)rowview.Row[0];
                        if (cmd.ExecuteNonQuery() > 0)
                            MessageBox.Show("Успешно премахнат тест ");
                        else
                            MessageBox.Show("Тестът не може да бъде премахнат");

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Грешка " + ex);
                    }

                    loadClinics();
                }
            }
            else
                MessageBox.Show("Изберете тест");
        }
    }
}
