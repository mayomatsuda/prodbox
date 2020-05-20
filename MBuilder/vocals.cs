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
    [Activity(Label = "vocals")]
    public class vocals : Activity
    {
        public static bool hasStarted = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.vocals);
            
            TextView name = (TextView)FindViewById(Resource.Id.selected);
            if (menu.theSong.getVocalsName() != null) name.Text = menu.theSong.getVocalsName();

            Button dl = FindViewById<Button>(Resource.Id.downloaded);
            dl.Click += delegate { StartActivity(typeof(downloadedVocals)); };

            Button bpm = FindViewById<Button>(Resource.Id.bpm);
            bpm.Click += delegate { StartActivity(typeof(bpmVocals)); };

            Button import = FindViewById<Button>(Resource.Id.importTrack);
            import.Click += delegate { StartActivity(typeof(importVocals)); };

            Button play = FindViewById<Button>(Resource.Id.play);
            play.Click += delegate
            {
                menu.theSong.getVocals().resample();
                hasStarted = true;
            };
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (menu.instanceCreated) { menu.theSong.stop(); }

            TextView name = (TextView)FindViewById(Resource.Id.selected);
            if (menu.theSong.getVocals() != null) name.Text = menu.theSong.getVocalsName();
            Button key = (Button)FindViewById(Resource.Id.key);
            if (menu.theSong.getVocals() != null) key.Text = "KEY: " + menu.theSong.getVocals().getKey().ToUpper();
            if (menu.theSong.getVocals().getKey() == "na") key.Text = "KEY: N/A";
        }
    }
}