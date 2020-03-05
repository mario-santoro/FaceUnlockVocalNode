using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FaceUnlockVocalNode
{
    class MySqlCreate
    {
        
        
       public static async void inserimento(String username, String password)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "serverappfaceunlock.database.windows.net",
                Database = "DBFaceUnlockVocalNode",
                UserID = "Mario",
                Password = "faceunloK94",
                SslMode = MySqlSslMode.Required,
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("Opening connection");
                await conn.OpenAsync();

                using (var command = conn.CreateCommand())
                {
                   
                
                    command.CommandText = @"INSERT INTO utente (id, username, password) VALUES (@id, @username, @password);";
                    command.Parameters.AddWithValue("@id", 1);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@passw", password);

                    int rowCount = await command.ExecuteNonQueryAsync();
                    Console.WriteLine(String.Format("Number of rows inserted={0}", rowCount));
                }

                // connection will be closed by the 'using' block
                Console.WriteLine("Closing connection");
            }

            Console.WriteLine("Press RETURN to exit");
            Console.ReadLine();
        }
    }
}