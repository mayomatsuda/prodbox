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
using Java.Security;

namespace MBuilder
{
    [Activity(Label = "bpm")]
    class bpmVocals : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.bpm);

            if (vocals.hasStarted) menu.theSong.getVocals().stop();

            EditText editText = FindViewById<EditText>(Resource.Id.bpm);
            try { editText.Text = menu.theSong.getVocals().getBPM().ToString(); }
            catch { }

            Button reset = FindViewById<Button>(Resource.Id.reset);
            reset.Click += delegate { menu.theSong.getVocals().setBPM(menu.theSong.getVocals().getDefBPM()); };

            editText.KeyPress += (object sender, View.KeyEventArgs e) => {
            e.Handled = false;
            if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
            {
                menu.theSong.getVocals().setBPM(int.Parse(editText.Text));
                e.Handled = true;
            }};
        }
    }
}