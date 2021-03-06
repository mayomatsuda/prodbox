﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Security;

namespace MBuilder
{
    [Activity(Label = "downloaded")]
    class downloadedInstruments : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.downloaded);
        }

        protected override void OnStart()
        {
            base.OnStart();


            track[] instrumentsTracks = new track[25];
            track[] tracks = openFiles();

            int count = 0;
            foreach (track i in tracks)
            {
                if (!(i == null))
                {
                    if (i.getType() == "i" || i.getType() == "v")
                    {
                        instrumentsTracks[count] = i;
                        count++;
                    }
                }
            }

            LinearLayout layout = (LinearLayout)FindViewById(Resource.Id.linearLayout1);

            Button none = new Button(this)
            {
                Text = "NONE"
            };
            layout.AddView(none);
            none.Click += delegate
            {
                menu.theSong.setInstruments(new track(null, null, 0, 0));
                Finish();
            };

            foreach (track i in instrumentsTracks)
            {
                if (!(i == null))
                {
                    Button btn = new Button(this)
                    {
                        Text = i.getName() + " [" + i.getBPM() + " BPM]"
                    };
                    layout.AddView(btn);
                    btn.Click += delegate
                    {
                        menu.theSong.setInstruments(i);
                        Finish();
                    };
                }
            }
            if (instruments.hasStarted) menu.theSong.getInstruments().stop();
        }

        private track[] openFiles()
        {
            track[] trackList = new track[100];
            int count = 0;

            var listAssets = Assets.List("");
            foreach (var file in listAssets)
            {
                try
                {
                    Android.Content.Res.AssetFileDescriptor newFd = Assets.OpenFd(file);
                    track newTrack = new track(newFd.FileDescriptor, file, newFd.StartOffset, newFd.DeclaredLength);
                    trackList[count] = newTrack;
                    count++;
                }
                catch {}
            }

            return trackList;
        }
    }
}