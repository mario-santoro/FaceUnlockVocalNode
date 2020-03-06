using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using DT = System.Data;

namespace FaceUnlockVocalNode
{
    class MySQL
    {
        private static SqlConnectionStringBuilder connessione()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "source";
            builder.UserID = "id";
            builder.Password = "passw";
            builder.InitialCatalog = "catalog";

            return builder;
        }

        private int maxId()
        {
            SqlConnectionStringBuilder builder = connessione();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT MAX(id) From utente;");

                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("{0}", reader.GetValue(0));
                            return Convert.ToInt32(reader.GetValue(0));
                        }
                        else
                        {
                            return 0;
                        }

                    }
                }

            }

        }

        private Boolean controlloUtente(String email) {

            SqlConnectionStringBuilder builder = connessione();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * From utente where username= '" + email + "';");

                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("{0}", reader.GetString(0));
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

            }
      
        }

         public Boolean inserimentoUtente(String text, String pasw)
        {
            
           if (controlloUtente( text))
            {
              int max= maxId();

                SqlConnectionStringBuilder builder = connessione();


                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

                {
                    connection.Open();
                    SqlParameter parameter;

                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = DT.CommandType.Text;
                        command.CommandText = @"  
                            INSERT INTO utente  
                                    (id,
                                    username,  
                                    passw 
                                    )  
                                VALUES  
                                    (@id,  
                                    @username,  
                                    @passw  
                                    ); ";
                        parameter = new SqlParameter("@id", DT.SqlDbType.Int);
                        parameter.Value = max+1;
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@username", DT.SqlDbType.NVarChar, 50);
                        parameter.Value = text;
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@passw", DT.SqlDbType.NVarChar, 16);
                        parameter.Value = pasw;
                        command.Parameters.Add(parameter);
                        command.ExecuteScalar();

                        return true;
                    }
                }
            }
            else {
                return false;
            }
        }
    }
}
