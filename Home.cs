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
        public TextView textV;
        String username;
       List<Note> n;

        static string[] frasi ={"Un animo onesto quando viene offeso si irrita più del normale.",
        "Chi disprezza è sempre più vile del disprezzato.",
        "Attento al disgusto; è un altro male incurabile; un morto vale più di un vivo disgustato di vivere.",
        "È bello morire per ciò in cui si crede; chi ha paura muore ogni giorno, chi non ha paura muore una volta sola.",
        "La felicità è un dono e il trucco è non aspettarla, ma gioire quando arriva",
        "Reprimere le proprie emozioni è come avere una bomba a orologeria nel corpo.",
        "La tristezza viene dalla solitudine del cuore.",
        "Se avessimo fatto tutte le cose di cui siamo capaci, ci saremmo sorpresi di noi stessi."};
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.homeLayout); // loads the HomeScreen.axml as this activity's view
            listView = FindViewById<ListView>(Resource.Id.List); // get reference to the ListView in the layout
            textV = FindViewById<TextView>(Resource.Id.benvenuto);
          
            username = Intent.GetStringExtra("username");
            string emozione = Intent.GetStringExtra("emozione");
            string text="";
            if (emozione != null)
            {
                int numFrase = Intent.GetIntExtra("numFrase", 0);
                 text = "Benvenuto " + username + ", l'emozione riscontrata nella sua foto è: " + emozione + ". Ecco la sua frase del giorno:\"" + frasi[numFrase] + "\"";
                
                textV.Text= text;
            }
            else {
                text = "Benvenuto " + username;
                 textV.Text = text;
            }
            
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