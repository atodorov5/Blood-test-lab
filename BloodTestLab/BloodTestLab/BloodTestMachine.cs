using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BloodTestLab
{
    class BloodTestMachine
    {
        public static double analyse(int testType)
        {
            using (var conn = new MySqlConnection("server=localhost;user id=root;password=root;database=bloodlab;"))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("analyse", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("p_testTypeID", MySqlDbType.Int32));
                    cmd.Parameters[0].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.Add(new MySqlParameter("o_minValue", MySqlDbType.Double));
                    cmd.Parameters[1].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(new MySqlParameter("o_maxValue", MySqlDbType.Double));
                    cmd.Parameters[2].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters[0].Value = testType;
                    cmd.ExecuteNonQuery();

                    Random random = new Random();
                    double k = (double)cmd.Parameters["o_minValue"].Value / 2;
                    double max = (double)cmd.Parameters["o_maxValue"].Value;
                    double min = (double)cmd.Parameters["o_minValue"].Value;
                    return Math.Round( random.NextDouble() * ((max+k) - (min-k)) + (min-k),2);
                    
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
