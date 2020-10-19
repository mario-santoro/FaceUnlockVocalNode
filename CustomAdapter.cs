
  
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
            
            
            view.Click += (sender, args) =>
            {
                Intent openPage1 = new Intent(context, typeof(ModificaNota));
                // Toast.MakeText(Application.Context, "Stampa: " + item.getId_nota() , ToastLength.Long).Show();
                openPage1.PutExtra("username", item.getUsername());

                openPage1.PutExtra("id", "" + item.getId_nota());
                openPage1.PutExtra("titolo", item.getTitolo());
                openPage1.PutExtra("contenuto", item.getContenuto());
                context.StartActivity(openPage1);
            };

            return view;
        }
        
       
    }
}
