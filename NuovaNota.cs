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
using Plugin.Media;
using Android.Graphics;
using Android;

namespace FaceUnlockVocalNode
{
    [Activity(Label = "NuovaNota")]
    public class NuovaNota : Activity
    {

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
            SetContentView(Resource.Layout.nuovaNotaLayout);
            // Create your application here
 
            Button b = (Button)FindViewById(Resource.Id.Salva);
            b.Click += NuovaNotaOnClick;

            img = (Button)FindViewById(Resource.Id.foto);
            img.Click += CamptureButton_Click;
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
    
            EditText contenuto = (EditText)FindViewById(Resource.Id.contenuto);
            contenuto.Text +=testo;

        }


    }
}