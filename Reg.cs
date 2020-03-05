using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using DT = System.Data;            // System.Data.dll  


namespace FaceUnlockVocalNode
{
    [Activity(Label = "Reg")]


    public class Reg : Activity
    {
        EditText username;
        EditText password;
        string user;
        string pass;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.regLayout);
            // Create your application here

            Button b = (Button)FindViewById(Resource.Id.registrati);
            b.Click += RegOnClick;
        }

        private void RegOnClick(object sender, EventArgs eventArgs)
        {

            username = (EditText)FindViewById(Resource.Id.textUser);
            user = username.Text.ToString();

            password = (EditText)FindViewById(Resource.Id.password);
            pass = password.Text.ToString();

            inserimento(user, pass);
            View view = (View)sender;
            Snackbar.Make(view, "Inserimento effettuato " + user + " " + pass, Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            /*Intent openPage1 = new Intent(this, typeof(Reg));
            StartActivity(openPage1);*/
        }

        public void inserimento(String text, String pasw)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "tcp:serverappfaceunlock.database.windows.net";
            builder.UserID = "mario";
            builder.Password = "faceunlocK94";
            builder.InitialCatalog = "DBFaceUnlockVocalNode";
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))

            {
                connection.Open();
                Console.WriteLine("Connected successfully.");

                Reg.InsertRows(connection, text, pasw);


            }
        }

        static public void InsertRows(SqlConnection connection, String text, String pasw)
        {
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
                parameter.Value = 1;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@username", DT.SqlDbType.NVarChar, 50);
                parameter.Value = text;
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@passw", DT.SqlDbType.NVarChar, 16);
                parameter.Value = pasw;
                command.Parameters.Add(parameter);
                command.ExecuteScalar();

            }
        }

    }
}