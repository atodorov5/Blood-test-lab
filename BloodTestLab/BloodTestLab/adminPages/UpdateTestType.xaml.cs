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
    /// Interaction logic for UpdateTestType.xaml
    /// </summary>
    public partial class UpdateTestType : Page
    {
        public UpdateTestType()
        {
            InitializeComponent();
            loadClinicInDataGrid();
        }

        private void loadClinicInDataGrid()
        {
            string sql = "SELECT * from testtype";

            var connection = DBConfig.Connection;
            MySqlCommand cmdSel = new MySqlCommand(sql, connection);
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
            da.Fill(dt);
            dt.Columns["idTestType"].ColumnName = "№";
            dt.Columns["name"].ColumnName = "Име";
            dt.Columns["minValue"].ColumnName = "Мин стойност";
            dt.Columns["maxValue"].ColumnName = "Макс стойност";
            dt.Columns["unit"].ColumnName = "Единица";
            dt.Columns["price"].ColumnName = "Цена";
            testsDG.DataContext = dt;
        }

        private void Button_UpdateTestType(object sender, RoutedEventArgs e)
        {

            DataRowView row = (DataRowView)testsDG.SelectedItem;
            if (row != null)
            {
                using (var conn = DBConfig.Connection)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("updateTestType", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new MySqlParameter("p_testTypeID", MySqlDbType.Int32));
                        cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_name", MySqlDbType.VarChar));
                        cmd.Parameters[1].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_minValue", MySqlDbType.VarChar));
                        cmd.Parameters[2].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_maxValue", MySqlDbType.VarChar));
                        cmd.Parameters[3].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_unit", MySqlDbType.VarChar));
                        cmd.Parameters[4].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters.Add(new MySqlParameter("p_price", MySqlDbType.VarChar));
                        cmd.Parameters[5].Direction = System.Data.ParameterDirection.Input;
                        cmd.Parameters[0].Value = (int)row["№"];
                        if (!string.IsNullOrWhiteSpace(testNameTB.Text))
                            cmd.Parameters[1].Value = testNameTB.Text;
                        if (!string.IsNullOrWhiteSpace(minValueTB.Text))
                            cmd.Parameters[2].Value = minValueTB.Text;
                        if (!string.IsNullOrWhiteSpace(maxValueTB.Text))
                            cmd.Parameters[3].Value = maxValueTB.Text;
                        if (!string.IsNullOrWhiteSpace(testUnitTB.Text))
                            cmd.Parameters[4].Value = testUnitTB.Text;
                        if (!string.IsNullOrWhiteSpace(priceTB.Text))
                            cmd.Parameters[5].Value = priceTB.Text;

                        cmd.ExecuteNonQuery();
                        loadClinicInDataGrid();
                        testNameTB.Clear();
                        minValueTB.Clear();
                        maxValueTB.Clear();
                        priceTB.Clear();
                        testUnitTB.Clear();

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Грешка " + ex);
                    }

                }
            }
            else
                MessageBox.Show("Изберете клиника!");
        }


    }
}
