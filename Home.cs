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
using static Android.Bluetooth.BluetoothClass;
using static Android.Widget.AdapterView;

namespace FaceUnlockVocalNode
{
    [Activity(Label = "Home")]
    public class Home : Activity
    {
        public ListView listView;
        String username;
       List<Note> n;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.homeLayout); // loads the HomeScreen.axml as this activity's view
            listView = FindViewById<ListView>(Resource.Id.List); // get reference to the ListView in the layout
           
             username = Intent.GetStringExtra("username");

            MySQL s = new MySQL();
            
           n = s.getNote(username);
            // populate the listview with data
            listView.Adapter = new CustomAdapter(this, n);

           /* if (!OnBackButtonPressed()) {
                Toast.MakeText(Application.Context, "Stampa: titolo ", ToastLength.Long).Show();
              
            }*/
            // Create your application here

            Button b = (Button)FindViewById(Resource.Id.addNota);
            b.Click += noteOnClick;

            /*
            Button canc = (Button)FindViewById(Resource.Id.elimina);
            canc.Click += eliminaOnClick;*/
        }

        
        private void noteOnClick(object sender, EventArgs eventArgs)
        {
            
          
                Intent openPage1 = new Intent(this, typeof(NuovaNota));
                openPage1.PutExtra("username", username);
                StartActivity(openPage1);


        }


    }

}