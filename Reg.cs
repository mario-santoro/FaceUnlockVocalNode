using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Plugin.Media;
using Android.Graphics;
using MySql.Data.MySqlClient;
using DT = System.Data;            // System.Data.dll  


namespace FaceUnlockVocalNode
{
    [Activity(Label = "Reg")]


    public class Reg : Activity
    {
        ImageView img;

        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };

        EditText username;
        EditText password;
        string user;
        string pass;
        string path;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.regLayout);
            // Create your application here
            img = (ImageView)FindViewById(Resource.Id.frameImage);
            img.Click += CamptureButton_Click;
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
            if (flag)//se l'username inserite non esiste già
            {
                var id= FaceUnlockVocalNode.Resources.MyCognitive.addPerson(user, ""); //creo un nuovo PersonGroup Person con l'username utente              
                m.inserimentoPersonID(user, id);       //////////PROBLEMA:inserisco nel DB l'id PersonId dell'utente///////        
                FaceUnlockVocalNode.Resources.MyCognitive.addFace(id, path);//aggiungo una faccia al groupPerson Person
                FaceUnlockVocalNode.Resources.MyCognitive.trainPersonGroup("2"); //faccio il train del PersonGroup

                Intent openPage1 = new Intent(this, typeof(Home));
                openPage1.PutExtra("username", u.getUsername());
                StartActivity(openPage1);

            }
            else {
                Snackbar.Make(view, "Errore esiste già un utente con questo username: " + user, Snackbar.LengthLong)
                 .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            }
           

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {

            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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
            path = file.Path.ToString();
            img.SetImageBitmap(b);
        }




    }
}
      

