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
        //richiede permessi
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

            Button b = (Button)FindViewById(Resource.Id.Salva);
            b.Click += NuovaNotaOnClick;

            img = (Button)FindViewById(Resource.Id.foto);
            img.Click += CamptureButton_Click;

        }

        private void NuovaNotaOnClick(object sender, EventArgs eventArgs)
        {
            //Inizializzo l'oggetto nota
            Note n = new Note();
            //prendo il titolo e lo setto nell'oggetto
            EditText tit = (EditText)FindViewById(Resource.Id.titolo);
            String titolo = tit.Text.ToString();
            n.setTitolo(titolo);
            //prendo il contenuto della nota e lo setto nell'oggetto
            EditText cont = (EditText)FindViewById(Resource.Id.contenuto);
            String contenuto = cont.Text.ToString();
            n.setContenuto(contenuto);
            //prendo la data corrente e lo setto nell'oggetto
            DateTime d = DateTime.Now;
            Console.WriteLine("Data della nota: " + d);
            n.setData(d.ToString());
            //recupero dall'intent l'username utente
            String username = Intent.GetStringExtra("username");

            MySQL m = new MySQL();
            //inserimento nota
            m.inserimentoNota(username, n);

            //torna alla home passando sempre l'username nell'intent
            Intent openPage1 = new Intent(this, typeof(Home));
            openPage1.PutExtra("username", username);
            StartActivity(openPage1);
        }

        //evento che cattura il bottone per fare la foto
        public void CamptureButton_Click(object sender, System.EventArgs eventArgs)
        {
            TakePhoto();
            // img.Visibility = ViewStates.Visible;
        }

        //funzione che identifica cosa fare dopo aver fatto la foto
        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();
            //definiamo dove  e come salvare la fot nel telefono
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name = "img.jpg",
                Directory = "sample"
            });

            if (file == null) { return; }
            //convertiamo l'immagine in array di byte
            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap b = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            String path = file.Path.ToString();
            //recuperiamo il testo dalla foto
            String testo = FaceUnlockVocalNode.Resources.MyCognitive.getText(path);
            //settiamo il testo ottenuto nell'EditText contenuto
            EditText contenuto = (EditText)FindViewById(Resource.Id.contenuto);
            contenuto.Text += testo;
        }
    }
}