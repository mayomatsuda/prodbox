using Android.Media;
using Android.OS;
using Java.IO;

public class song
{
	private track theBass;
	private track theDrums;
	private track theInstruments;
	private track theVocals;
	private int songBPM;
	private bool bpmSet = false;
	private bool isPlaying = false;

	public song(Java.IO.FileDescriptor bassFile, string bassFileString, long b1, long b2, Java.IO.FileDescriptor drumsFile, string drumsFileString, long d1, long d2, Java.IO.FileDescriptor instrumentsFile, string instrumentsFileString, long i1, long i2, Java.IO.FileDescriptor vocalsFile, string vocalsFileString, long v1, long v2)
	{
		theBass = new track(bassFile, bassFileString, b1, b2);
		theDrums = new track(drumsFile, drumsFileString, d1, d2);
		theInstruments = new track(instrumentsFile, instrumentsFileString, i1, i2);
		theVocals = new track(vocalsFile, vocalsFileString, v1, v2);
	}

	public void play()
	{
		isPlaying = true;
		theBass.resample();
		theDrums.resample();
		theInstruments.resample();
		theVocals.resample();
	}

	public void stop()
	{
		isPlaying = false;
		theBass.stop();
		theDrums.stop();
		theInstruments.stop();
		theVocals.stop();
	}

	public void pause()
	{
		theBass.pause();
		theDrums.pause();
		theInstruments.pause();
		theVocals.pause();
	}

	public track getBass()
	{
		return theBass;
	}

	public string getBassName()
	{
		return theBass.getName();
	}

	public void setBass(track newBass)
	{
		theBass = newBass;
		if (!(bpmSet))
		{
			songBPM = newBass.getBPM();
			bpmSet = true;
		}
	}

	public track getDrums()
	{
		return theDrums;
	}

	public string getDrumsName()
	{
		return theDrums.getName();
	}

	public void setDrums(track newDrums)
	{
		theDrums = newDrums;
		if (!(bpmSet))
		{
			songBPM = newDrums.getBPM();
			bpmSet = true;
		}
	}

	public track getInstruments()
	{
		return theInstruments;
	}

	public string getInstrumentsName()
	{
		return theInstruments.getName();
	}

	public void setInstruments(track newInstruments)
	{
		theInstruments = newInstruments;
		if (!(bpmSet))
		{
			songBPM = newInstruments.getBPM();
			bpmSet = true;
		}
	}

	public track getVocals()
	{
		return theVocals;
	}

	public string getVocalsName()
	{
		return theVocals.getName();
	}

	public void setVocals(track newVocals)
	{
		theVocals = newVocals;
		if (!(bpmSet))
		{
			songBPM = newVocals.getBPM();
			bpmSet = true;
		}
	}

	public int getBPM()
	{
		return songBPM;
	}

	public void setBPM(int newBPM)
	{
		songBPM = newBPM;
		bpmSet = true;

		theBass.setBPM(songBPM);
		theDrums.setBPM(songBPM);
		theVocals.setBPM(songBPM);
		theInstruments.setBPM(songBPM);
	}

	public bool playing()
	{
		return isPlaying;
	}
}
