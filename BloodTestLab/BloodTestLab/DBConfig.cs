using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BloodTestLab
{
    public class DBConfig
    {
        static string conf = "server=localhost;user id=root;password=root;database=bloodlab;";

        static MySqlConnection db;

        public static MySqlConnection Connection
        {
            get
            {
                if (db == null)
                {
                    LazyInitializer.EnsureInitialized(ref db, CreateConnection);
                }
                return db;
            }
        }

        static MySqlConnection CreateConnection()
        {
            var db = new MySqlConnection(conf);

            //db.Open();
            return db;
        }

        static void CloseConnection()
        {
            if (db != null)
            {
                db.Close();
                db.Dispose();
                db = null;
            }
        }

    }
}
