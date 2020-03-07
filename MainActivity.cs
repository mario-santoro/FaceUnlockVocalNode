using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Content;

namespace FaceUnlockVocalNode
{

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        // Add your training & prediction key from the settings page of the portal
        string trainingKey = System.Environment.GetEnvironmentVariable("");
        string predictionKey = System.Environment.GetEnvironmentVariable("");
        string ENDPOINT = System.Environment.GetEnvironmentVariable("");
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Button b1 = (Button)FindViewById(Resource.Id.button);
            b1.Click += LogOnClick;

            Button b2 = (Button)FindViewById(Resource.Id.buttonReg);
            b2.Click += RegOnClick;


        }
        private void LogOnClick(object sender, EventArgs eventArgs)
        {
            /*View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();*/
            Intent openPage1 = new Intent(this, typeof(Login));
            StartActivity(openPage1);
        }


        private void RegOnClick(object sender, EventArgs eventArgs)
        {

            Intent openPage1 = new Intent(this, typeof(Reg));
            StartActivity(openPage1);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}
