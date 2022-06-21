using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slagalica_91_2018
{
    class DbConnection
    {
        private static SqlConnection instance = null;
        private static readonly object padlock = new object();
        //private static string konekcioni = @"Data Source=(localdb)\luka;Initial Catalog=slagalica;Integrated Security=True";

        public static SqlConnection Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SqlConnection(Properties.Settings.Default.slagalicaConnectionString);
                    }
                    return instance;
                }
            }
        }
    }
}
