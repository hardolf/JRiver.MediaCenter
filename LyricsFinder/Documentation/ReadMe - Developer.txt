The LyricsFinder finds lyrics for the currently playing songs in the "Playing Now" list.
The found lyrics are saved in the songs' tags.
The LyricsFinder can be used as a standalone program and/or as a plug-in for the JRiver Media Center.

This ReadMe file is just an introduction.
For more developer details, please see:

- The LyricsFinder wiki      : https://github.com/hardolf/JRiver.MediaCenter/wiki/LyricsFinder.
- Solution source code       : https://github.com/hardolf/JRiver.MediaCenter (LyricsFinder.sln and LyricsFinder subfolder)


Features
--------
Automatic search for lyrics in several public lyrics services.
Automatic update of the lyrics tags of the current Media Center playlist tunes' files.
Manual search for lyrics from the lyrics services for a single playlist tune at a time.
Simple Lyrics editor.
Selection of the public lyrics services to be used and the order in which they are called during search.
Installation as a JRiver Media Center plugin and/or standalone lyrics finder with connection to a running local or remote Media Center.
Uses the JRiver Media Center REST Web service (MCWS), thus allowing the LyricsFinder standalone program and the Media Center to be on different machines.


Prerequisites
-------------
JRiver Media Center 23 or newer, using its builtin REST Web service (MCWS).


Compatibility
-------------
The LyricsFinder is expected to be compatible with future versions of JRiver Media Center, as long as these feature the REST Web service (MCWS).
The plugin should also be compatible with future versions of JRiver Media Center because there is code in the Setup.iss file that finds any installed instances of the MediaCenter on the target machine.


Development environment
-----------------------
Microsoft Visual Studio 2017, 2019, ..., minimum Community edition, C#
Inno Setup Compiler 6.0.2 (only needed for changes of the setup program, e.g. if a new Media Center version is installed)


Current lyrics services
-----------------------
Apiseeds Lyrics API          : https://apiseeds.com (Account required, no other restrictions)
Chart Lyrics                 : http://www.chartlyrics.com (No restrictions)
Musixmatch                   : https://www.musixmatch.com (Account required, max. 2.000 free requests per day for 30% of the lyrics text, paid plans for full lyrics)
STANDS4 Network Lyrics.com   : https://www.lyrics.com (Account required, max. 100 free requests per day)

More lyric services may be found in the future, look here for inspiration:
https://www.programmableweb.com/category/lyrics/api


Solution projects
-----------------
Documentation                : Documentation project holding the ReadMe notes etc.
Installation                 : Installation project responsible for building the release packages.
LyricServices\*              : Lyric service projects, one for each service, no "hard" references but loaded by LyricsFinderCore at runtime.
LyricsFinderPlugin           : Plugin for JRiver Media Center.
LyricsFinderCore             : Core project used by both standalone and plugin.
LyricsFinderExe              : Standalone program, does not need the plugin.
MessageInspection            : Utility module for SOAP message inspections.
Utility                      : General utility module.


Installation
------------
Unpack and run the installation program:

- End user                   : ...\MediaCenter\LyricsFinder\Installation\Release\Setup.zip
- Developer                  : ...\MediaCenter\LyricsFinder\Installation\Output\Setup.exe


Installation notes
------------------
The Setup.exe automatically finds any installed JRiver Media Center versions and the user can select which one to install the plug-in to.
Only one version of JRiver Media Center can be "plugged-in" on the same machine, as the plug-in is registered via COM.
The standalone program connects to the JRiver Media Center version matching the URL in the standalone program's configuration.


Relevant links
--------------
JRiver                       : https://www.jriver.com/
Forum                        : https://yabb.jriver.com/interact/
Wiki                         : https://wiki.jriver.com/
DevZone                      : https://wiki.jriver.com/index.php/DevZone
Plug-ins and accessories     : https://accessories.jriver.com/mediacenter/accessories.php
Inno Setup Compiler          : http://www.jrsoftware.org/
