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
            //prendiamo il titolo dall'intent e lo settiamo nell'EditText
            String t = Intent.GetStringExtra("titolo");
            TextView titolo = (TextView)FindViewById(Resource.Id.titoloMod);
            titolo.Text = t;
            //prendiamo il contenuto dall'intent e lo settiamo nell'EditText
            String c = Intent.GetStringExtra("contenuto");
            TextView contenuto = (TextView)FindViewById(Resource.Id.contenutoMod);
            contenuto.Text = c;

            Button b = (Button)FindViewById(Resource.Id.modifica);
            //prendiamo l'id della nota dall'intent e creiamo un nuovo oggetto Nota, passandogli tutte le informazioni dall'intent
            int id = int.Parse(Intent.GetStringExtra("id"));
            n = new Note(id, Intent.GetStringExtra("titolo"), Intent.GetStringExtra("data"), Intent.GetStringExtra("contenuto"), Intent.GetStringExtra("username"));
            b.Click += modificaOnClick;

            img = (Button)FindViewById(Resource.Id.mfoto);
            img.Click += CamptureButton_Click;

        }

        //metodo che si attiva quando l'utente dell'app preme il tasto Back del telefono portandolo alla schermata Home
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

        //evento  lanciato nel momento che si preme il bottone per salvare le modifiche
        private void modificaOnClick(object sender, EventArgs eventArgs)
        {

            //prendiamo il titolo dall'Edit text e lo settiamo nell'oggetto Nota
            EditText tit = (EditText)FindViewById(Resource.Id.titoloMod);
            String titolo = tit.Text.ToString();
            n.setTitolo(titolo);
            //prendiamo il contenuto dall'Edit text e lo settiamo nell'oggetto Nota
            EditText cont = (EditText)FindViewById(Resource.Id.contenutoMod);
            String contenuto = cont.Text.ToString();
            n.setContenuto(contenuto);
            //prendiamo la data corrente e lo settiamo nell'oggetto Nota
            DateTime d = DateTime.Now;
            Console.WriteLine("Data della nota: " + d);
            n.setData(d.ToString());
            //richiamiamo il metodo per modificare la nota nel DB passandogli l'oggetto nota
            MySQL m = new MySQL();
            m.updateNota(n);
            //si torna alla schermata home
            String username = Intent.GetStringExtra("username");
            Intent openPage1 = new Intent(this, typeof(Home));
            openPage1.PutExtra("username", username);
            StartActivity(openPage1);



        }

        //si attiva quando si scatta la foto
        public void CamptureButton_Click(object sender, System.EventArgs eventArgs)
        {
            TakePhoto();
            // img.Visibility = ViewStates.Visible;
        }

        //definisce il comportamento dopo aver scattato la foto
        async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();
            //dove e in che modo salvare la foto sul telefono
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 40,
                Name = "img.jpg",
                Directory = "sample"
            });

            if (file == null) { return; }
            //la foto viene convertito in un array di byte
            byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap b = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            String path = file.Path.ToString();
            //viene richiamato il metodo per il riconoscimento del testo nelle immagini
            String testo = FaceUnlockVocalNode.Resources.MyCognitive.getText(path);
            //il testo riconosciuti viene aggiunto nel contenuto della nota nell'EditText
            EditText contenuto = (EditText)FindViewById(Resource.Id.contenutoMod);
            contenuto.Text += testo;

        }

    }
}