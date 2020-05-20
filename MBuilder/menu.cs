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
    [Activity(Label = "menu")]
    public class menu : Activity
    {
        public static song theSong = new song(null, null, 0, 0, null, null, 0, 0, null, null, 0, 0, null, null, 0, 0);
        public static bool instanceCreated = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.menu);

            Button playButton = FindViewById<Button>(Resource.Id.listen);

            Button drumsButton = FindViewById<Button>(Resource.Id.drums);
            drumsButton.Click += delegate { StartActivity(typeof(drums)); playButton.Text = "LISTEN"; };

            Button bassButton = FindViewById<Button>(Resource.Id.bass);
            bassButton.Click += delegate { StartActivity(typeof(bass)); playButton.Text = "LISTEN"; };

            Button vocalsButton = FindViewById<Button>(Resource.Id.vocals);
            vocalsButton.Click += delegate { StartActivity(typeof(vocals)); playButton.Text = "LISTEN"; };

            Button instrumentsButton = FindViewById<Button>(Resource.Id.instruments);
            instrumentsButton.Click += delegate { StartActivity(typeof(instruments)); playButton.Text = "LISTEN"; };

            playButton.Click += delegate
            {
                instanceCreated = true;

                if (!(theSong.playing()) && ((theSong.getBass().exists()) || (theSong.getDrums().exists()) || (theSong.getVocals().exists()) || (theSong.getInstruments().exists())))
                {
                    playButton.Text = "STOP";
                    theSong.play();
                }
                else
                {
                    playButton.Text = "LISTEN";
                    theSong.stop();
                }
            };

            Button options = FindViewById<Button>(Resource.Id.options);
            options.Click += delegate { StartActivity(typeof(bpmSong)); playButton.Text = "LISTEN"; };
        }

        public void updateText()
        {
            TextView bassText = (TextView)FindViewById(Resource.Id.bassText);
            TextView drumText = (TextView)FindViewById(Resource.Id.drumsText);
            TextView vocalsText = (TextView)FindViewById(Resource.Id.vocalsText);
            TextView instrumentsText = (TextView)FindViewById(Resource.Id.instrumentsText);

            if (theSong.getBassName() != null) bassText.Text = theSong.getBassName();
            if (theSong.getDrumsName() != null) drumText.Text = theSong.getDrumsName();
            if (theSong.getVocalsName() != null) vocalsText.Text = theSong.getVocalsName();
            if (theSong.getInstrumentsName() != null) instrumentsText.Text = theSong.getInstrumentsName();
        }

        protected override void OnStart()
        {
            base.OnStart();
            
            updateText();
            if (bass.hasStarted) theSong.getBass().stop();
            if (drums.hasStarted) theSong.getDrums().stop();
            if (instruments.hasStarted) theSong.getInstruments().stop();
            if (vocals.hasStarted) theSong.getVocals().stop();
        }
    }
}