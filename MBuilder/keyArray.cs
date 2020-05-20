using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
    public class keyArray
    {
        private string[] array = new string[12];
        private int keyIndex;
        private bool isMinor = false;

        public keyArray(string key)
        {
            array[0] = "c";
            array[1] = "c#";
            array[2] = "d";
            array[3] = "d#";
            array[4] = "e";
            array[5] = "f";
            array[6] = "f#";
            array[7] = "g";
            array[8] = "g#";
            array[9] = "a";
            array[10] = "a#";
            array[11] = "b";

            if (key.Length > 1)
            {
                if (key.Last() == 'm')
                {
                    isMinor = true;
                    key = key.Replace("m", "");
                }
            }

            int counter = 0;
            for (int i = 0; counter < 12; i = (i + 1) % array.Length)
            {
                if (array[i] == key)
                {
                    keyIndex = i; break;
                }
                counter++;
            }
        }

        public string newKey(float r)
        {
            int c = (int) Math.Round((r - 1) * 12);
            int rot = 12 + c;
            if (rot >= 12) rot = rot - 12;

            int counter = 0;
            string n = "";
            for (int i = keyIndex; counter <= rot; i = (i + 1) % array.Length)
            {
                n = array[i];
                counter++;
            }
            if (isMinor) n = n + "m";
            return n;
        }
    }
}