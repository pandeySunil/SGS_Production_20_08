 using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGS_DEPLOYMENTPROJECT
{
     public class SQLConnectionSetUp
    {
        public SqlConnection GetConn() {
            try
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
                conn.ConnectionString = @"Data Source=sql.freeasphost.net\MSSQL2016;Initial Catalog=pandeysunil_SampleDB;Persist Security Info=True;User ID=pandeysunil_SampleDB;Password=DBSamplePW";
                return conn;
            }
            catch (Exception Ex) {
                throw Ex;
            }
        }
    }
}
