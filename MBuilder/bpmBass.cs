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
    [Activity(Label = "bpm")]
    class bpmBass : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.bpm);

            if (bass.hasStarted) menu.theSong.getBass().stop();

            EditText editText = FindViewById<EditText>(Resource.Id.bpm);
            try { editText.Text = menu.theSong.getBass().getBPM().ToString(); }
            catch { }

            Button reset = FindViewById<Button>(Resource.Id.reset);
            reset.Click += delegate { menu.theSong.getBass().setBPM(menu.theSong.getBass().getDefBPM()); };

            editText.KeyPress += (object sender, View.KeyEventArgs e) => {
            e.Handled = false;
            if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
            {
                menu.theSong.getBass().setBPM(int.Parse(editText.Text));
                e.Handled = true;
            }};
        }
    }
}