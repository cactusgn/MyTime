using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timeRecorder
{
    class SqlHelper
    {//获取数据库连接字符串
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }

        #region 封装一个执行SQL返回受影响的行数
        public static int ExecuteNoQuery(string sqlText, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = sqlText;
                    cmd.Parameters.AddRange(parameters.ToArray());
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region 封装一个执行SQL返回查询结果中第一行第一列的值
        public static object ExecuteScalar(string sqlText, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = sqlText;
                    cmd.Parameters.AddRange(parameters.ToArray());
                    return cmd.ExecuteScalar();
                }
            }
        }
        #endregion

        #region 封装一个执行SQL返回一个DataTable
        public static DataTable ExecuteDataTable(string sqlText, params SqlParameter[] parameters)
        {
           
            using (SqlDataAdapter adapter = new SqlDataAdapter(sqlText, GetConnectionString()))
            {
                DataTable dt = new DataTable();
                adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
                adapter.Fill(dt);
                return dt;
            }
            
            
        }
        #endregion

        #region 封装一个执行SQL返回一个SqlDataReader
        public static SqlDataReader ExecutedReader(string sqlText, params SqlParameter[] parameters)
        {
            //SqlDataReader要求独占SqlConnection对象，并且SqlConnection必须是Open状态
            SqlConnection con = new SqlConnection(GetConnectionString());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = sqlText;
            cmd.Parameters.AddRange(parameters.ToArray());
            //SqlDataReader执行完成后顺便关闭数据库连接
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        #endregion
    }
}
