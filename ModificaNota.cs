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
using Plugin.Media;
using Android.Graphics;
using Android;

namespace FaceUnlockVocalNode
{
    [Activity(Label = "ModificaNota")]
    public class ModificaNota : Activity
    {
        Note n;
        Button img;

        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };
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

            img = (Button)FindViewById(Resource.Id.mfoto);
            img.Click += CamptureButton_Click;
            // Create your application here
        }
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            switch (keyCode)
            {
                // in smartphone
                case Keycode.Back:
                    Intent openPage1 = new Intent(this, typeof(Home));
                    String username = Intent.GetStringExtra("username");
                    openPage1.PutExtra("username", username);
                    StartActivity(openPage1);
                    break;

            
            }
            return base.OnKeyDown(keyCode, e);
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


        public void CamptureButton_Click(object sender, System.EventArgs eventArgs)
        {
            TakePhoto();
            // img.Visibility = ViewStates.Visible;
        }


        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name = "img.jpg",
                Directory = "sample"
            });

            if (file == null) { return; }
            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap b = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            String path = file.Path.ToString();
            // img.SetImageBitmap(b);

            String testo = FaceUnlockVocalNode.Resources.MyCognitive.getText(path);
            
            EditText contenuto = (EditText)FindViewById(Resource.Id.contenutoMod);
            contenuto.Text += testo;

        }

    }
}