using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Security;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;

namespace MBuilder
{
    [Activity(Label = "importFile")]
    class importDrums : Activity
    {
        private string name;
        private string customName = "";
        private int bpm;
        private string key = "na";
        private string type = "b";
        private track[] tracks;
        private string[] fileStrings;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.importFile);

            tracks = openFiles();

            Spinner browse = FindViewById<Spinner>(Resource.Id.browse);
            browse.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(fileSpinner_ItemSelected);
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, fileStrings);
            browse.Adapter = adapter;

            EditText bpmEnter = FindViewById<EditText>(Resource.Id.bpm);
            bpmEnter.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    bpm = int.Parse(bpmEnter.Text);
                    e.Handled = true;
                }};

            Spinner keySpinner = FindViewById<Spinner>(Resource.Id.key);
            keySpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(keySpinner_ItemSelected);
            var kadapter = ArrayAdapter.CreateFromResource(this, Resource.Array.key_array, Android.Resource.Layout.SimpleSpinnerItem);
            kadapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            keySpinner.Adapter = kadapter;

            EditText nameEnter = FindViewById<EditText>(Resource.Id.nameEnter);
            nameEnter.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    customName = nameEnter.Text;
                    e.Handled = true;
                }
            };

            Button done = FindViewById<Button>(Resource.Id.done);
            done.Click += delegate
            {
                doneMethod();
            };
        }

        private track[] openFiles()
        {
            track[] trackList = new track[100];
            int count = 0;

            var pathFile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            var absolutePath = pathFile.AbsolutePath;

            foreach (string file in Directory.GetFiles(absolutePath))
            {
                if (file.Contains(".wav"))
                {
                    //try
                    {
                        track newTrack = new track(name, file);
                        trackList[count] = newTrack;
                        count++;
                    }
                    //catch { }
                }
            }

            fileStrings = new string[count];
            for (int i = 0; i < count; i++)
            {
                string[] currentFile = trackList[i].getSecond().Split("/");
                fileStrings[i] = currentFile[currentFile.Length - 1];
            }
            return trackList;
        }

        private void doneMethod()
        {
            try
            {
                foreach (track t in tracks)
                {
                    if (t != null)
                    {
                        if (t.getSecond().Contains(name))
                        {
                            if (customName == "") customName = name;
                            track theTrack = new track(customName, t.getSecond());
                            theTrack.setBPM(bpm);
                            theTrack.setDefBPM(bpm);
                            theTrack.setKey(key);
                            theTrack.setType(type);
                            menu.theSong.setDrums(theTrack);
                            break;
                        }
                    }
                }
            }
            catch
            {
                var toast = Toast.MakeText(Application.Context, "Could not import file", ToastLength.Short);
                toast.Show();
            }
            finally
            {
                Finish();
            }
        }

        private void keySpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            key = string.Format ("{0}", spinner.GetItemAtPosition(e.Position));
            key = key.ToLower();
            if (key == "N/A") key = "na";
        }

        private void fileSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            name = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (drums.hasStarted) menu.theSong.getDrums().stop();
        }
    }
}