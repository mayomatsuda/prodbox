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

namespace MBuilder
{
    [Activity(Label = "instruments")]
    public class instruments : Activity
    {
        public static bool hasStarted = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.instruments);
            
            TextView name = (TextView)FindViewById(Resource.Id.selected);
            if (menu.theSong.getInstrumentsName() != null) name.Text = menu.theSong.getInstrumentsName();

            Button dl = FindViewById<Button>(Resource.Id.downloaded);
            dl.Click += delegate { StartActivity(typeof(downloadedInstruments)); };

            Button bpm = FindViewById<Button>(Resource.Id.bpm);
            bpm.Click += delegate { StartActivity(typeof(bpmInstruments)); };

            Button import = FindViewById<Button>(Resource.Id.importTrack);
            import.Click += delegate { StartActivity(typeof(importInstruments)); };

            Button play = FindViewById<Button>(Resource.Id.play);
            play.Click += delegate
            {
                menu.theSong.getInstruments().resample();
                hasStarted = true;
            };
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (menu.instanceCreated) { menu.theSong.stop(); }

            TextView name = (TextView)FindViewById(Resource.Id.selected);
            if (menu.theSong.getInstruments() != null) name.Text = menu.theSong.getInstrumentsName();
            Button key = (Button)FindViewById(Resource.Id.key);
            if (menu.theSong.getInstruments() != null) key.Text = "KEY: " + menu.theSong.getInstruments().getKey().ToUpper();
            if (menu.theSong.getInstruments().getKey() == "na") key.Text = "KEY: N/A";
        }
    }
}