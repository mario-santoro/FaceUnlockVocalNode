using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Android.Content;
using System;
using Android;
using Plugin.Media;
using Android.Graphics;
using Android.Views;
using Android.Support.Design.Widget;


namespace FaceUnlockVocalNode
{
    [Activity(Label = "Login")]
    public class Login : Activity
    {
        Button ButtonLogin;
        ImageView img;

        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };

        EditText username;
        string user;
        string path;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.logFaceLayout);

            RequestPermissions(permissionGroup, 0);

            img = (ImageView)FindViewById(Resource.Id.frameImage);
           
            ButtonLogin = (Button)FindViewById(Resource.Id.ButtonLogin);
            ButtonLogin.Click += LogOnClick;
            img.Click += CamptureButton_Click;
            Button b1 = (Button)FindViewById(Resource.Id.Credenziali);
            b1.Click += LogconCred;
            
        }
        private void LogOnClick(object sender, EventArgs eventArgs)
        {
           
           
           
            username = (EditText)FindViewById(Resource.Id.textUser);
            user = username.Text.ToString();

          
         
            MySQL m = new MySQL();

            Boolean flag= m.controlloUtente(user);

             View view = (View)sender;
            if (flag)
            {             
                var id= FaceUnlockVocalNode.Resources.MyCognitive.Detect(path);
                string idP = FaceUnlockVocalNode.Resources.MyCognitive.identify(id);
                if (idP!="") {
                    if (m.getPersonID(user, idP))
                    {
                        Intent openPage1 = new Intent(this, typeof(Home));
                        openPage1.PutExtra("username", user);
                        StartActivity(openPage1);

                    }
                    else {
                        Snackbar.Make(view, "Errore mismatch tra foto e username: ", Snackbar.LengthLong)
                   .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();

                    }
                   

                }
                else
                {
                    Snackbar.Make(view, "Errore riconoscimento: ", Snackbar.LengthLong)
                     .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
            }
            else
            {
                Snackbar.Make(view, "Errore non esiste un utente con questo username: " + user, Snackbar.LengthLong)
                 .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            }


        }
        private void LogconCred(object sender, EventArgs eventArgs)
        {

            Intent openPage1 = new Intent(this, typeof(LoginCredenziali));
            StartActivity(openPage1);
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
            path= file.Path.ToString();
            img.SetImageBitmap(b);
        }

    }
}