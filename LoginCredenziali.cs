using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;


namespace FaceUnlockVocalNode
{
    [Activity(Label = "Activity1")]
    public class LoginCredenziali : Activity
    {

        EditText username;
        EditText password;
        string user;
        string pass;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
           
            SetContentView(Resource.Layout.loginLayout);
            // Create your application here
            Button b = (Button)FindViewById(Resource.Id.log);
            b.Click += LogOnClick;
        }



        private void LogOnClick(object sender, EventArgs eventArgs)
        {

            username = (EditText)FindViewById(Resource.Id.userLog);
            user = username.Text.ToString();

            password = (EditText)FindViewById(Resource.Id.passLog);
            pass = password.Text.ToString();
            Utente u = new Utente(user, pass);
            MySQL m = new MySQL();

            Boolean flag = m.loginUtente(u.getUsername(), u.getPassword());

            View view = (View)sender;
            if (flag)
            {
                /* Snackbar.Make(view, "Login effettuato", Snackbar.LengthLong)
                     .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();*/
                
                
              
                Intent openPage1 = new Intent(this, typeof(Home));
                openPage1.PutExtra("username",u.getUsername());
                StartActivity(openPage1);


            }
            else
            {
                Snackbar.Make(view, "Errore non esiste un utente con questa email: " + user, Snackbar.LengthLong)
                 .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            }
        
        }

    }
}