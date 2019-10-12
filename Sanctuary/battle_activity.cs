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
    [Activity(Label = "battle_activity")]
    public class battle_activity : Activity
    {
        enum battlestate
        {
            Waiting,
            Live,
            Over
        }
        
        battlestate gameState;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.battle_main);

            gameState = battlestate.Waiting;

            RelativeLayout TopHalf = FindViewById<RelativeLayout>(Resource.Id.TopLayout);
            TextView txt = new TextView(this);
            //Android.Graphics.Typeface typeface = this.Resources.GetFont(Resource.Font.betterpixels);
            //txt.Typeface = typeface;
            txt.Text = "Hello world check out this awesome font";
            TopHalf.AddView(txt);
        }
    }
}