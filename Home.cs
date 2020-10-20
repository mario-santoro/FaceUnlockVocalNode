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
        //array con tuttelefrasi di benvenuto, che verranno visualizzate a seconda dell'umore riscontrato nella foto
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
            SetContentView(Resource.Layout.homeLayout);

            textV = FindViewById<TextView>(Resource.Id.benvenuto);
            //recuperiamo username e l'emozione percepita nella foto di login
            username = Intent.GetStringExtra("username");
            string emozione = Intent.GetStringExtra("emozione");
            string text = "";
            if (emozione != null)//se la stringa che dovrebbe contenere l'emozione non è null 
            {
                int numFrase = Intent.GetIntExtra("numFrase", 0); //recuperiamo l'intero che identifica l'emozione (indice) per l'array di frasi di benvenuto
                text = "Benvenuto " + username + ", l'emozione riscontrata nella sua foto è: " + emozione + ". Ecco la sua frase del giorno:\"" + frasi[numFrase] + "\"";
                //e  settiamo il tutto nella text view di benvenuto
                textV.Text = text;
            }
            else//se la stringa che dovrebbe contenere l'emozione è null (cioè se si fa accesso con credenziali o abbiamo fatto la registrazione), viene visualizzato un normale benvenuto per l'utente
            {
                text = "Benvenuto " + username;
                textV.Text = text;
            }
            //recuperiamo tutte le note dell'utente dal database
            MySQL s = new MySQL();
            n = s.getNote(username);

            listView = FindViewById<ListView>(Resource.Id.List);
            // popoliamo la listView con i dati (le note)
            listView.Adapter = new CustomAdapter(this, n);

            Button b = (Button)FindViewById(Resource.Id.addNota);
            b.Click += noteOnClick;

            Button logOut = (Button)FindViewById(Resource.Id.logout);
            logOut.Click += Logout;

        }

        //nel caso l'utente preme il tasto back del telefono, lo continua  aportare alla home per uscire deve fare logout
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

        //al click del tasto + si passa a una pagina per aggiungere una nuova nota
        private void noteOnClick(object sender, EventArgs eventArgs)
        {


            Intent openPage1 = new Intent(this, typeof(NuovaNota));
            openPage1.PutExtra("username", username);
            StartActivity(openPage1);


        }
        //metodo peril logout che porta l'utente alla prima pagina dell'app
        private void Logout(object sender, EventArgs eventArgs)
        {
            Intent openPage1 = new Intent(this, typeof(MainActivity));
            StartActivity(openPage1);
        }


    }

}