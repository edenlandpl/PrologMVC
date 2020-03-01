using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolog.SQLConnection
{
    class DBClass
    {
        public static string GetConnectionString()
        {
            string strConString = "Data Source=DELL-ADI\\SQLEXPRESS;Initial Catalog=Prolog;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";
            return strConString;
            //ADRIAN-ZH DELL-ADI
        }

        public static string sql;
        public static SqlConnection con = new SqlConnection();
        public static SqlCommand cmd = new SqlCommand("", con);
        public static SqlDataReader rd;
        public static DataTable dt;
        public static SqlDataAdapter da;

        public static void openConnection()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = GetConnectionString();
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nie połączono - " + Environment.NewLine + " Opis - " + ex.Message.ToString(), "C# Connect to SQL Server");
            }
        }

        public static void closeConnection()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
