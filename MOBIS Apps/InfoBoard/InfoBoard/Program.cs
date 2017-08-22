using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace InfoBoard
{
    static class Program
    {
        public static string strConnectionString = "server=172.17.44.34;database=OMMCDP;uid=ommcdp;pwd=MNA_M!2008;Connection Timeout=5";
        public static SqlConnection connection = null;

        public static string strConnectionString2nd = "server=10.120.175.75;database=OMMCDP;uid=ommcdp;pwd=MNA_M!2008@;Connection Timeout=5";
        public static SqlConnection connection2nd = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        private static void ConnectionDB()
        {
            if (connection == null)
            {
                connection = new SqlConnection();
                connection.ConnectionString = strConnectionString;
            }

            if (connection.State == ConnectionState.Closed)
            {
                connection.ConnectionString = strConnectionString;
                connection.Open();
            }
        }

        private static void DisconnectionDB()
        {
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();

            if (connection != null)
                connection.Dispose();

            connection = null;
        }

        public static DataTable QueryExecute(String strQuery, SqlParameter[] param)
        {
            DataTable table = new DataTable();
            SqlDataAdapter adapter = null;

            try
            {
                ConnectionDB();
                adapter = new SqlDataAdapter(strQuery, connection);

                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        adapter.SelectCommand.Parameters.Add(param[i]);
                    }
                }

                adapter.Fill(table);
            }
            catch
            {
                return null;
            }
            finally
            {
                DisconnectionDB();
            }

            return table;
        }

        private static void ConnectionDB2nd()
        {
            if (connection2nd == null)
            {
                connection2nd = new SqlConnection();
                connection2nd.ConnectionString = strConnectionString2nd;
            }

            if (connection2nd.State == ConnectionState.Closed)
            {
                connection2nd.ConnectionString = strConnectionString2nd;
                connection2nd.Open();
            }
        }

        private static void DisconnectionDB2nd()
        {
            if (connection2nd != null && connection2nd.State != ConnectionState.Closed)
                connection2nd.Close();

            if (connection2nd != null)
                connection2nd.Dispose();

            connection2nd = null;
        }

        public static DataTable QueryExecute2nd(String strQuery, SqlParameter[] param)
        {
            DataTable table = new DataTable();
            SqlDataAdapter adapter = null;

            try
            {
                ConnectionDB2nd();
                adapter = new SqlDataAdapter(strQuery, connection2nd);

                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        adapter.SelectCommand.Parameters.Add(param[i]);
                    }
                }

                adapter.Fill(table);
            }
            catch
            {
                return null;
            }
            finally
            {
                DisconnectionDB2nd();
            }

            return table;
        }
   
    }
}
