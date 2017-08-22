using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace OMMCDP
{
    class Database
    {
        SqlConnection conn;
        SqlConnection subConn;

        /***********************************************************************************************************************************/
        #region Constructor
        /***********************************************************************************************************************************/

        public Database()
        {
            conn = new SqlConnection();
            subConn = new SqlConnection();
        }

        /*---------------------------------------------------------------------------------------------------------*/

        public Database(string ConnectionString)
        {
            conn = new SqlConnection(ConnectionString);
            subConn = new SqlConnection(ConnectionString);
        }


        #endregion

        /***********************************************************************************************************************************/
        #region Property
        /***********************************************************************************************************************************/

        public string ConnectionString
        {
            set { conn.ConnectionString = value; }
        }

        #endregion

        /***********************************************************************************************************************************/
        #region Metheds
        /***********************************************************************************************************************************/

        public void Fill(string Query, ref DataTable Table)
        {
            System.Diagnostics.Debug.WriteLine(Query);

            SqlCommand comm = new SqlCommand(Query, conn);
            try
            {

                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                Table.Load(reader);
                conn.Close();
            }
            catch (Exception ex)
            {
                Common.WriteLog("GetData()" + ex.Message);
                Common.WriteLog(Query);
                throw;
            }
            finally
            {
                comm.Dispose();
                conn.Close();
            }
        }

        /*---------------------------------------------------------------------------------------------------------*/

        public void ExecuteNonQuery(string Query)
        {
            int i = 0;
            SqlCommand comm = new SqlCommand(Query, conn);

            conn.Open();
            i = comm.ExecuteNonQuery();
            conn.Close();

            if (i == 0)
            {
                Common.WriteLog("0개행이 실행 되었습니다.");
                Common.WriteLog(Query);
                comm.Dispose();
                conn.Close();
            }
        }

        public void ExecuteNonQuery(string[] Query)
        {
            using (conn)
            {
                conn.Open();

                using (SqlTransaction sqlTrans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = sqlTrans;
                        try
                        {
                            foreach (string query in Query)
                            {
                                cmd.CommandText = query;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch
                        {
                            sqlTrans.Rollback();
                            throw;
                        }
                    }
                    sqlTrans.Commit();
                }
            }

        }

        public int ExecuteTransaction(string[] strSQL, string fileName)
        {
            int ret = 0;

            try
            {
                conn.Open();
                using (SqlTransaction sqlTrans = conn.BeginTransaction())
                {

                    try
                    {
                        for (int i = 0; i < strSQL.Length; i++)
                        {
                            SqlCommand Comm = new SqlCommand(strSQL[i], conn, sqlTrans);
                            Comm.CommandType = CommandType.Text;

                            ret = Comm.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        sqlTrans.Rollback();
                        throw (ex);
                    }

                    sqlTrans.Commit();
                    conn.Close();
                }
            }
            catch
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                throw;
            }
            return ret;
        }

        public void GetData(string Query, ref DataTable Table)
        {
            SqlCommand comm = new SqlCommand(Query, conn);
            try
            {

                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                Table.Load(reader);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                comm.Dispose();
                conn.Close();
            }
        }

    }

}
        #endregion
