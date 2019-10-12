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

namespace CellGo1
{
    [Activity(Label = "inventory_activity")]
    public class inventory_activity : ListActivity
    {
        static readonly item[] items = new item[] {
            new item("sword", "a shiny sword" ),
            new item("bow", "a powerful bow" )
          };

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ListAdapter = new ArrayAdapter<item>(this, Resource.Layout.list_item, items);

            ListView.TextFilterEnabled = true;

            ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                Toast.MakeText(Application, ((TextView)args.View).Text, ToastLength.Short).Show();
            };
        }
    }
}