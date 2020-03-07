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

namespace FaceUnlockVocalNode
{
    [Activity(Label = "NuovaNota")]
    public class NuovaNota : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.nuovaNotaLayout);
            // Create your application here

            Button b = (Button)FindViewById(Resource.Id.Salva);
            b.Click += NuovaNotaOnClick;
            // Create your application here
        }

        private void NuovaNotaOnClick(object sender, EventArgs eventArgs)
        {

            Note n = new Note();
            EditText tit= (EditText)FindViewById(Resource.Id.titolo);
            String titolo = tit.Text.ToString();
            n.setTitolo(titolo);

            EditText cont = (EditText)FindViewById(Resource.Id.contenuto);
            String contenuto = cont.Text.ToString();
            n.setContenuto(contenuto);
            DateTime d= DateTime.Now;
            Console.WriteLine("Data della nota: "+ d);
            n.setData(d.ToString());
            String username = Intent.GetStringExtra("username");

            MySQL m = new MySQL();

             m.inserimentoNota(username, n);

            
                Intent openPage1 = new Intent(this, typeof(Home));
                openPage1.PutExtra("username", username);
                StartActivity(openPage1);



        }

    }
}