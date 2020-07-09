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
using Java.IO;

namespace MBuilder
{
    [Activity(Label = "bass")]
    public class bass : Activity
    {
        public static bool hasStarted = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.bass);

            TextView name = (TextView)FindViewById(Resource.Id.selected);
            if (menu.theSong.getBassName() != null) name.Text = menu.theSong.getBassName();

            Button dl = FindViewById<Button>(Resource.Id.downloaded);
            dl.Click += delegate { StartActivity(typeof(downloadedBass)); };

            Button bpm = FindViewById<Button>(Resource.Id.bpm);
            bpm.Click += delegate { StartActivity(typeof(bpmBass)); };

            Button import = FindViewById<Button>(Resource.Id.importTrack);
            import.Click += delegate { StartActivity(typeof(importBass)); };

            Button play = FindViewById<Button>(Resource.Id.play);
            play.Click += delegate
            {
                menu.theSong.getBass().resample();
                hasStarted = true;
            };
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (menu.instanceCreated) { menu.theSong.stop(); }

            TextView name = (TextView)FindViewById(Resource.Id.selected);
            if (menu.theSong.getBass() != null) name.Text = menu.theSong.getBassName();
            Button key = (Button)FindViewById(Resource.Id.key);
            if (menu.theSong.getBass() != null) key.Text = "KEY: " + menu.theSong.getBass().getKey().ToUpper();
            if (menu.theSong.getBass().getKey() == "na") key.Text = "KEY: N/A";
            if (menu.theSong.getBass().source() && menu.theSong.getBass().wantCalc)
            {
                menu.theSong.getBass().setBPM(menu.theSong.getBass().getCalcBPM());
                menu.theSong.getBass().setDefBPM(menu.theSong.getBass().getCalcBPM());

            }
        }
    }
}