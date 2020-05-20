# ProdBox
This is the source code for my first Android app. Simple tool for mixing audio tracks into songs. Ability to resample
tracks to any BPM, sync entire songs, and choose between a variety of tracks.

<img src="https://github.com/mayomatsuda/prodbox/blob/master/Screenshots/sc1.jpg" alt="drawing" width="200"/>   <img src="https://github.com/mayomatsuda/prodbox/blob/master/Screenshots/sc2.jpg" alt="drawing" width="200"/>   <img src="https://github.com/mayomatsuda/prodbox/blob/master/Screenshots/sc3.jpg" alt="drawing" width="200"/>

### Planned developments:
* Fully implement ability to import tracks from device
* UI overhaul
* Marketplace for uploading/downloading tracks

### Notes:
All tracks included for demo are royalty free from [LooperMan](https://www.looperman.com/). I'm happy to address any
licensing concerns.

If you'd like to add your own track, the file format is `<name with no dashes-key-bpm-type>`. Just add any .wav file
named as such to the project Assets. For example, `<drum beat-c#m-110-d>`. The types are *b* for bass, *d* for drums,
and *i* for instruments.

### Dependencies:
* Xamarin
* FilePicker 2.1.34 (from [NuGet](https://www.nuget.org/packages/Xamarin.Plugin.FilePicker/2.1.34))
