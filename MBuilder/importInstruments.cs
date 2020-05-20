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
    class importInstruments : Activity
    {
        private string path;
        private string name;
        private FileData file;
        private int bpm;
        private string key;
        private string type = "b";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.importFile);

            Button browse = FindViewById<Button>(Resource.Id.browse);
            browse.Click += async delegate
            {
                var fileImp = await CrossFilePicker.Current.PickFile();
                if (fileImp != null)
                {
                    path = fileImp.FilePath;
                    name = fileImp.FileName;
                    file = fileImp;
                    browse.Text = name;
                }
            };

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
            var kadapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.key_array, Android.Resource.Layout.SimpleSpinnerItem);
            kadapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            keySpinner.Adapter = kadapter;

            Button done = FindViewById<Button>(Resource.Id.done);
            done.Click += delegate
            {
                doneMethod();
            };
        }

        private void doneMethod()
        {
            try
            {
                string filePath = null;
                Android.Net.Uri uri = Android.Net.Uri.Parse(path);
                Android.Net.Uri tUri = uri;
                if (DocumentsContract.IsDocumentUri(this, uri))
                {
                    string docId = DocumentsContract.GetDocumentId(uri);
                    if (uri.Authority.Equals("com.android.providers.media.documents"))
                    {
                        string id = docId.Split(":")[1];
                        string selection = MediaStore.Video.Media.InterfaceConsts.Id + "=" + id;
                        filePath = getfilePath(MediaStore.Video.Media.ExternalContentUri, selection);
                    }
                    else if (uri.Authority.Equals("com.android.providers.downloads.documents"))
                    {
                        Android.Net.Uri contentUri = ContentUris.WithAppendedId(Android.Net.Uri.Parse("content://downloads/public_downloads"), long.Parse(docId));
                        filePath = getfilePath(contentUri, null);
                    }
                }
                else if (uri.Scheme.Equals("content", StringComparison.OrdinalIgnoreCase))
                {
                    filePath = getfilePath(uri, null);
                }
                else if (uri.Scheme.Equals("file", StringComparison.OrdinalIgnoreCase))
                {
                    filePath = uri.Path;
                }

                track theTrack = new track(name, filePath, file, Application.Context);
                theTrack.setName(name);
                try
                {
                    theTrack.setDefBPM(bpm);
                    theTrack.setBPM(bpm);
                }
                catch
                {
                    theTrack.setDefBPM(1);
                    theTrack.setBPM(1);
                }
                theTrack.setKey(key);
                theTrack.setType(type);
                
                menu.theSong.setInstruments(theTrack);
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
            if (key == "N/A") key = "na";
            key = key.ToLower();
        }

        private string getfilePath(Android.Net.Uri uri, string selection)
        {
            string path = null;
            var cursor = ContentResolver.Query(uri, null, selection, null, null);
            if (cursor != null)
            {
                if (cursor.MoveToFirst())
                {
                    path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Video.Media.InterfaceConsts.Data));
                }
                cursor.Close();
            }
            return path;
        }
    }
}