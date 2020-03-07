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
            Utente u = new Utente(user, pass);
            MySQL m = new MySQL();
          
            Boolean flag= m.inserimentoUtente(u.getUsername(), u.getPassword());

            View view = (View)sender;
            if (flag)
            {
                Intent openPage1 = new Intent(this, typeof(Home));
                openPage1.PutExtra("username", u.getUsername());
                StartActivity(openPage1);

            }
            else {
                Snackbar.Make(view, "Errore esiste già un utente con questa email: " + user, Snackbar.LengthLong)
                 .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            }
           

        }

          
        

    }
}
      

