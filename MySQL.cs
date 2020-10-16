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
            builder.DataSource = "server-faccia.database.windows.net";
            builder.UserID = "annunziata";
            builder.Password = "mario-94";
            builder.InitialCatalog = "app";

            return builder;
        }

        private int maxNota()
        {
            SqlConnectionStringBuilder builder = connessione();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT MAX(id_nota) From nota;");

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
        public Boolean loginUtente(String username, String password)
        {

            SqlConnectionStringBuilder builder = connessione();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT username From utente where username= '" + username + "' AND passw='" + password + "';");

                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("{0}", reader.GetString(0));
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

            }

        }
        public Boolean controlloUtente(String username) {

            SqlConnectionStringBuilder builder = connessione();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * From utente where username= '" + username + "';");

                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("{0}", reader.GetString(0));
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

            }

        }

        public Boolean inserimentoUtente(String text, String pasw)
        {

            if (!controlloUtente(text))
            {


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
                                    (
                                    username,  
                                    passw 
                                    )  
                                VALUES  
                                    (
                                    @username,  
                                    @passw  
                                    ); ";


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

        public void inserimentoNota(String username, Note n)
        {

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
                            INSERT INTO nota  
                                    (
                                    id_nota,
                                    titolo,  
                                    data_nota,
                                    contenuto,
                                    username
                                    )  
                                VALUES  
                                    (@id_nota,  
                                    @titolo,  
                                    @data_nota,
                                    @contenuto,
                                    @username 
                                    ); ";

                    parameter = new SqlParameter("@id_nota", DT.SqlDbType.NVarChar, 50);
                    parameter.Value = maxNota() + 1;
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@titolo", DT.SqlDbType.NVarChar, 50);
                    parameter.Value = n.getTitolo();
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@data_nota", DT.SqlDbType.NVarChar, 50);
                    parameter.Value = n.getData();
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@contenuto", DT.SqlDbType.NVarChar, 16);
                    parameter.Value = n.getContenuto();
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@username", DT.SqlDbType.NVarChar, 16);
                    parameter.Value = username;
                    command.Parameters.Add(parameter);

                    command.ExecuteScalar();


                }
            }

        }
        
        public void updateNota(Note n) {

            SqlConnectionStringBuilder builder = connessione();


            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                SqlParameter parameter;

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = DT.CommandType.Text;
                    command.CommandText = @"  update nota set titolo = @titolo, data_nota = @data, contenuto = @contenuto  where id_nota = @id_nota ; ";

                    parameter = new SqlParameter("@id_nota", DT.SqlDbType.NVarChar, 50);
                    parameter.Value = n.getId_nota();
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@titolo", DT.SqlDbType.NVarChar, 50);
                    parameter.Value = n.getTitolo();
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@data", DT.SqlDbType.NVarChar, 50);
                    parameter.Value = n.getData();
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@contenuto", DT.SqlDbType.NVarChar, 16);
                    parameter.Value = n.getContenuto();
                    command.Parameters.Add(parameter);

                    command.ExecuteScalar();


                }
            }

        }
        public void deleteNota(int id_nota) {

            SqlConnectionStringBuilder builder = connessione();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("delete From nota where id_nota= " + id_nota + ";");

                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Console.WriteLine("{0} {1} {2} {3}", reader.GetValue(0)); reader.GetString(1); reader.GetString(2); reader.GetString(3));

                            Console.WriteLine("Cancellato");


                        }
                      
                    }
                }

            }


        }

        public List<Note> getNote(String username)
        {
            List<Note> n = new List<Note>();
            SqlConnectionStringBuilder builder = connessione();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT id_nota, titolo, data_nota, contenuto From nota where username= '" + username + "' ORDER BY(data_nota);");

                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           // Console.WriteLine("{0} {1} {2} {3}", reader.GetValue(0)); reader.GetString(1); reader.GetString(2); reader.GetString(3));
                            Note nota = new Note( );
                            nota.setId_nota(Convert.ToInt32(reader.GetValue(0)));
                            nota.setTitolo(reader.GetString(1));
                            nota.setData(Convert.ToString(reader.GetValue(2)).Substring(0,10));
                            nota.setContenuto(reader.GetString(3));
                            nota.setUsername(username);
                            n.Add(nota);
                        }
                        return n;
                    }
                }

            }

        }

    }
}