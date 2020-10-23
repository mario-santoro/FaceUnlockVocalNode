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
        //metodo per ottenere la connessione al DB
        private static SqlConnectionStringBuilder connessione()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "server-faccia.database.windows.net";
            builder.UserID = "XXXXXXXXXX";
            builder.Password = "XXXXXXXXXX";
            builder.InitialCatalog = "app";

            return builder;
        }

        //metodo per il login utente con credenziali
        public Boolean loginUtente(String username, String password)
        {
            //ottiene la connessione
            SqlConnectionStringBuilder builder = connessione();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                //query
                sb.Append("SELECT username From utente where username= '" + username + "' AND passw='" + password + "';");
                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())//se esiste l'utente restituisce true altrimenti false
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
        //controllo che il PersonID riconosciuto con il login a riconoscimento facciale sia effettivamente dell'utente dichiarato con quell'username
        public Boolean getPersonID(String username, String id)
        {
            //ottiene la connessione dal DB
            SqlConnectionStringBuilder builder = connessione();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT personID From utente where username= '" + username + "';");

                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())//se esiste 
                        {

                            if (reader.GetString(0) == id)//e se il PersonID di quell'utente è uguale al PersonID riconosciuto restituisce true, altrimenti false
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                        {
                            return false;
                        }

                    }
                }

            }

        }

        //controllo che esista l'utente username
        public Boolean controlloUtente(String username)
        {

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

        //inserimento del PersonId nel database, aggiunto in un secondo momento, poichè viene creato l'id dal servizio cognitivo solo se la registrazione è avvenuta con successo
        public Boolean inserimentoPersonID(String username, String personID)
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
                    command.CommandText = @"UPDATE utente SET personID = @personID WHERE username=@username ";


                    parameter = new SqlParameter("@personID", DT.SqlDbType.NVarChar, 40);
                    parameter.Value = personID;
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@username", DT.SqlDbType.NVarChar, 50);
                    parameter.Value = username;
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    return true;

                }
            }

        }

        //inserimento utente, cioè registrazione
        public Boolean inserimentoUtente(String text, String pasw)
        {
            //si controlla che l'username selezionato non esista già, altrimenti restituisce false
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
                        parameter = new SqlParameter("@username", DT.SqlDbType.NVarChar, 20);
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
            else
            {
                return false;
            }
        }
        //recupera il numero di Note, necessaria per inserimentoNota (creazione dell'ID)
        private int maxNota()
        {
            SqlConnectionStringBuilder builder = connessione();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT Count(id_nota) From nota;");

                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {                            
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
        //metodo per  l'inserimento nota
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
                    //l'id nota viene setatto prendendo dal DB la nota con id più alta e incrementando di 1 il valore
                    parameter = new SqlParameter("@id_nota", DT.SqlDbType.Int, 8);
                    parameter.Value = maxNota() + 1;
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@titolo", DT.SqlDbType.NVarChar, 20);
                    parameter.Value = n.getTitolo();
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@data_nota", DT.SqlDbType.NVarChar, 50);
                    parameter.Value = n.getData();
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@contenuto", DT.SqlDbType.NVarChar, 200);
                    parameter.Value = n.getContenuto();
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@username", DT.SqlDbType.NVarChar, 20);
                    parameter.Value = username;
                    command.Parameters.Add(parameter);

                    command.ExecuteScalar();


                }
            }

        }
        //metodo per modificare le note
        public void updateNota(Note n)
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
        //metodo per cancellare le note
        public void deleteNota(int id_nota)
        {

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
                            Console.WriteLine("Cancellato");
                        }

                    }
                }

            }


        }
        //recuperare dal DB tutte le note di un utente memorizzandole in un ArrayList di Note
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
                            //Per ogni record nota trovato viene setatto un oggetto Nota, inserito poi nell'arrayList
                            Note nota = new Note();
                            nota.setId_nota(Convert.ToInt32(reader.GetValue(0)));
                            nota.setTitolo(reader.GetString(1));
                            nota.setData(Convert.ToString(reader.GetValue(2)).Substring(0, 10));
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
