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
using Newtonsoft.Json;

namespace FaceUnlockVocalNode
{
    [Activity(Label = "ModificaNota")]
    public class ModificaNota : Activity
    {
        Note n;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.modificaNotaLayout);
            // Create your application here
           
            String t =Intent.GetStringExtra("titolo");
            TextView titolo = (TextView)FindViewById(Resource.Id.titoloMod);
                       titolo.Text= t;

            String c = Intent.GetStringExtra("contenuto");
            TextView contenuto = (TextView)FindViewById(Resource.Id.contenutoMod);
            contenuto.Text = c;
            Button b = (Button)FindViewById(Resource.Id.modifica);
            int id= int.Parse( Intent.GetStringExtra("id")); 
            n = new Note(id, Intent.GetStringExtra("titolo"), Intent.GetStringExtra("data"), Intent.GetStringExtra("contenuto"), Intent.GetStringExtra("username"));
            b.Click += modificaOnClick;
            // Create your application here
        }

        private void modificaOnClick(object sender, EventArgs eventArgs)
        {

            
            EditText tit= (EditText)FindViewById(Resource.Id.titoloMod);
            String titolo = tit.Text.ToString();
            n.setTitolo(titolo);

            EditText cont = (EditText)FindViewById(Resource.Id.contenutoMod);
            String contenuto = cont.Text.ToString();
            n.setContenuto(contenuto);
            DateTime d= DateTime.Now;
            Console.WriteLine("Data della nota: "+ d);
            n.setData(d.ToString());
            String username = Intent.GetStringExtra("username");

            MySQL m = new MySQL();

             m.updateNota(n);

            
                Intent openPage1 = new Intent(this, typeof(Home));
                openPage1.PutExtra("username", username);
                StartActivity(openPage1);



        }

    }
}