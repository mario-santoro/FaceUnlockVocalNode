
  
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

using Nancy.Json;

namespace FaceUnlockVocalNode
{
    public class CustomAdapter : BaseAdapter<Note>
    {
        List<Note> items;
        private Activity context;
        // private int po;
       Note item2;
        public CustomAdapter(Activity context, List<Note> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Note this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            item2 = item;
            //     Console.WriteLine("id: " + item.getId_nota());
            //po = position;
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.home, null);
            view.FindViewById<TextView>(Resource.Id.titoloNota).Text = item.getTitolo();
            view.FindViewById<TextView>(Resource.Id.dataNota).Text = "ultima modifica: "+item.getData();
            view.FindViewById<Button>(Resource.Id.elimina).Text = "Cancella";
            
            view.FindViewById<Button>(Resource.Id.elimina).Click += (sender, args) =>
            {
                MySQL s = new MySQL();
               // Console.WriteLine("elimino " + item.getId_nota());
                s.deleteNota(item.getId_nota());
                //Toast.MakeText(Application.Context, "Stampa: " + item.getId_nota() + "titolo " + item.getTitolo(), ToastLength.Long).Show();
                Intent openPage1 = new Intent(context, typeof(Home));
                openPage1.PutExtra("username", item.getUsername());
                context.StartActivity(openPage1);
            };
            view.FindViewById<TextView>(Resource.Id.titoloNota).Click += (sender, args) =>
            {
                Intent openPage1 = new Intent(context, typeof(ModificaNota));
                // Toast.MakeText(Application.Context, "Stampa: " + item.getId_nota() + "titolo " + item.getTitolo(), ToastLength.Long).Show();
                openPage1.PutExtra("username", item.getUsername());
                openPage1.PutExtra("id", item.getId_nota());
                openPage1.PutExtra("titolo", item.getTitolo());
                openPage1.PutExtra("contenuto", item.getContenuto());
                context.StartActivity(openPage1);
            };

            view.FindViewById<TextView>(Resource.Id.dataNota).Click += (sender, args) =>
            {
                Intent openPage1 = new Intent(context, typeof(ModificaNota));
               // Toast.MakeText(Application.Context, "Stampa: " + item.getId_nota() , ToastLength.Long).Show();
                openPage1.PutExtra("username", item.getUsername());

                openPage1.PutExtra("id", ""+item.getId_nota());
                openPage1.PutExtra("titolo", item.getTitolo());
                openPage1.PutExtra("contenuto", item.getContenuto());
                context.StartActivity(openPage1);
            };
            /*

            TextView d = (TextView)view.FindViewById<TextView>(Resource.Id.titoloNota);
            d.Click += (sender, args) =>
        {
           // var t = items[po];

            Intent openPage1 = new Intent(context, typeof(ModificaNota));
             Toast.MakeText(Application.Context, "Stampa: " + item.getId_nota()+ "titolo "+ item.getTitolo(), ToastLength.Long).Show();
            openPage1.PutExtra("username", item.getUsername());
            openPage1.PutExtra("id", item.getId_nota());
            openPage1.PutExtra("titolo", item.getTitolo());
            openPage1.PutExtra("contenuto", item.getContenuto());
            //context.StartActivity(openPage1);
        };
            TextView t = (TextView)view.FindViewById<TextView>(Resource.Id.dataNota);
            t.Click += (sender, args) =>
            {
                // var t = items[po];

                Intent openPage1 = new Intent(context, typeof(ModificaNota));
                Toast.MakeText(Application.Context, "Stampa: " + item.getId_nota() + "titolo " + item.getTitolo(), ToastLength.Long).Show();
                openPage1.PutExtra("username", item.getUsername());
                openPage1.PutExtra("id", item.getId_nota());
                openPage1.PutExtra("titolo", item.getTitolo());
                openPage1.PutExtra("contenuto", item.getContenuto());
               // context.StartActivity(openPage1);
            }; 
            */

            return view;
        }
        
        private void modificaNota(object sender, EventArgs e)
        {

            Toast.MakeText(Application.Context, "Stampa: " + item2.getId_nota()+ "titolo "+ item2.getTitolo(), ToastLength.Long).Show();
            Intent openPage1 = new Intent(context, typeof(ModificaNota));

            openPage1.PutExtra("username", item2.getUsername());
            openPage1.PutExtra("id", item2.getId_nota());
            openPage1.PutExtra("titolo", item2.getTitolo());
            openPage1.PutExtra("contenuto", item2.getContenuto());

            //context.StartActivity(openPage1);


        }

        /*
        private void cancellaNota(object sender, EventArgs e)
        {

            MySQL s = new MySQL();
             var t = items[po];
            s.deleteNota(t.getId_nota());
            Intent openPage1 = new Intent(context, typeof(Home));
            openPage1.PutExtra("username", t.getUsername());
            context.StartActivity(openPage1);
          

        }
      */

    }
}
