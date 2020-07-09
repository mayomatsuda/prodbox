using Android.Content;
using Android.Hardware;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.Provider;
using Java.IO;
using Java.Lang;
using MBuilder;
using NAudio.Wave;
using System;
using System.IO;

public class track
{
	private Java.IO.FileDescriptor songFile;
	private string songFileString;
	private long long1;
	private long long2;
	MediaPlayer player;

	private bool isPrepped;
	private bool hasStarted;

	private string name;
	private string key;
	private int bpm;
	private int defBPM;
	private string type;

	private SoundPool.Builder builder = new SoundPool.Builder();
	private SoundPool sp;
	private int id;
	private float ratio = 1;
	private TimeSpan length;
	private int calcBPM;

	private bool isFromStringPath = false;
	public bool wantCalc;

	private int playid;

	public track(Java.IO.FileDescriptor filename, string filenameString, long l1, long l2)
	{
		songFile = filename;
		songFileString = filenameString;
		long1 = l1;
		long2 = l2;
		
		isPrepped = false;
		hasStarted = false;

		if (!(songFile == null))
		{
			string[] datalist = filenameString.Split("-");
			name = datalist[0];
			key = datalist[1];
			bpm = int.Parse(datalist[2]);
			defBPM = bpm;
			type = datalist[3].Remove(1);
			try
			{
				sp = builder.Build();
				id = sp.Load(songFile, long1, long2, 1);
			}
			catch (System.Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }
		}
	}

	public track(string nName, string path)
	{
		isFromStringPath = true;
		songFileString = path;

		isPrepped = false;
		hasStarted = false;

		name = nName;
		key = null;
		bpm = 0;
		defBPM = 0;
		type = null;

		try
		{
			WaveFileReader wf = new WaveFileReader(songFileString);
			length = wf.TotalTime;
			calcBPM = calculateBPM();
		}
		catch (System.Exception e) { System.Diagnostics.Debug.WriteLine(e.Message); }

		sp = builder.Build();
		id = sp.Load(songFileString, 1);
	}

	private int calculateBPM()
	{
		double b = (16) / (length.TotalSeconds / 60);
		int nb = Convert.ToInt32(b);
		return nb;
	}

	public void load()
	{
		if (!(songFileString == null))
		{
			player = new MediaPlayer();
			player.Looping = true;
			if (isFromStringPath)
			{
				player.SetDataSource(songFileString);
				player.Prepare();
			}
			else
			{
				player.SetDataSource(songFile, long1, long2);
				player.Prepare();
			}
		}
	}

	public void play()
    {

		if (!(songFileString == null))
		{
			player.Start();
			hasStarted = true;
		}
	}

	public void stop()
    {
		try
		{
			/*if (ratio == 1f) player.Stop();
			else*/ sp.Stop(playid);
		}
		catch { }
	}

	public void resample()
	{
		// Original code to access MediaPlayer rather than SoundPool. Works for samples over 1 MB, however does not loop seamlessly.
		/*if (ratio == 1f)
		{
			try { stop(); }
			catch { }
			load();
			play();
		}

		else*/
		{
			try
			{
				ratio = ((float)bpm / (float)defBPM);

				int waitLimit = 1000;
				int waitCounter = 0;
				int throttle = 10;

				playid = sp.Play(id, 1, 1, 1, -1, ratio);
				while (playid == 0 && waitCounter < waitLimit)
				{ waitCounter++; SystemClock.Sleep(throttle); }
				hasStarted = true;
			}
			catch { }
		}
	}

	public void pause()
	{
		if (!(songFileString == null)) { player.Pause(); }
	}

	public string getName()
	{
		if (!(songFileString == null)) { return name; }
		else { return "None"; }
	}
	public void setName(string newName)
	{
		name = newName;
	}
	public string getKey()
	{
		if (!(songFileString == null))
		{
			keyArray k = new keyArray(key);
			ratio = ((float)bpm / (float)defBPM);
			if (ratio != 1f) key = k.newKey(ratio);

			return key;
		}
		else { return "None"; }
	}
	
	public void setKey(string newKey)
	{
		key = newKey;
	}
	public int getBPM()
	{
		if (!(songFileString == null)) { return bpm; }
		else { return 0; }
	}
	public string getType()
	{
		if (!(songFileString == null)) { return type; }
		else { return "None"; }
	}
	public void setType(string newType)
	{
		type = newType;
	}
	public Java.IO.FileDescriptor getFirst()
	{
		try { return songFile; }
		catch { return null; }
	}
	public string getSecond()
	{
		return songFileString;
	}
	public long getThird()
	{
		try { return long1; }
		catch { return 0; }
	}
	public long getFourth()
	{
		try { return long2; }
		catch { return 0; }
	}

	public void setBPM(int newBPM)
	{
		bpm = newBPM;
		ratio = ((float)bpm / (float)defBPM);
	}

	public int getDefBPM()
	{
		return defBPM;
	}
	public void setDefBPM(int newBPM)
	{
		defBPM = newBPM;
	}

	public float getRatio()
	{
		ratio = ((float)bpm / (float)defBPM);
		return ratio;
	}

	public bool source()
	{
		return isFromStringPath;
	}

	public bool exists()
	{
		return (songFileString != null);
	}

	public TimeSpan getLength()
	{
		return length;
	}

	public int getCalcBPM()
	{
		return calcBPM;
	}
}
