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
    [Activity(Label = "drums")]
    public class drums : Activity
    {
        public static bool hasStarted = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.drums);
            
            TextView name = (TextView)FindViewById(Resource.Id.selected);
            if (menu.theSong.getDrumsName() != null) name.Text = menu.theSong.getDrumsName();

            Button dl = FindViewById<Button>(Resource.Id.downloaded);
            dl.Click += delegate { StartActivity(typeof(downloadedDrums)); };

            Button bpm = FindViewById<Button>(Resource.Id.bpm);
            bpm.Click += delegate { StartActivity(typeof(bpmDrums)); };

            Button import = FindViewById<Button>(Resource.Id.importTrack);
            import.Click += delegate { StartActivity(typeof(importDrums)); };

            Button play = FindViewById<Button>(Resource.Id.play);
            play.Click += delegate
            {
                menu.theSong.getDrums().resample();
                hasStarted = true;
            };
        }
        
        protected override void OnStart()
        {
            base.OnStart();
            if (menu.instanceCreated) { menu.theSong.stop(); }

            TextView name = (TextView)FindViewById(Resource.Id.selected);
            if (menu.theSong.getDrums() != null) name.Text = menu.theSong.getDrumsName();
            Button key = (Button)FindViewById(Resource.Id.key);
            if (menu.theSong.getDrums() != null) key.Text = "KEY: " + menu.theSong.getDrums().getKey().ToUpper();
            if (menu.theSong.getDrums().getKey() == "na") key.Text = "KEY: N/A";
        }
    }
}