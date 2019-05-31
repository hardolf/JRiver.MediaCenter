
The LyricsFinder finds lyrics for the currently playing songs in the "Playing Now" list.

The LyricsFinder can be used as a standalone program and/or as a plug-in for the JRiver Media Center.

Features
--------
- Automatic search for lyrics in several public lyrics services.
- Automatic update of the lyrics tags of the current Media Center playlist tunes' files.
- Manual search for lyrics from the lyrics services for a single playlist tune at a time.
- Simple Lyrics editor.
- Selection of the public lyrics services to be used and the order in which they are called during search.
- Installation as a JRiver Media Center plugin and/or standalone lyrics finder with connection to a running local or remote Media Center.
- Uses the JRiver Media Center REST Web service (MCWS), thus allowing the LyricsFinder standalone program and the Media Center to be on different machines.


Prerequisites
-------------
JRiver Media Center 23 or newer, using its builtin REST Web service (MCWS).


Compatibility
-------------
The LyricsFinder is expected to be compatible with future versions of JRiver Media Center, as long as these feature the REST Web service (MCWS).


Current lyrics services
-----------------------
Apiseeds Lyrics API          : https://apiseeds.com (Account required, no other restrictions)
Chart Lyrics                 : http://www.chartlyrics.com (No restrictions)
Musixmatch                   : https://www.musixmatch.com (Account required, max. 2.000 free requests per day for 30% of the lyrics text, paid plans for full lyrics)
STANDS4 Network Lyrics.com   : https://www.lyrics.com (Account required, max. 100 free requests per day)


Relevant links
--------------
JRiver                       : https://www.jriver.com/
Forum                        : https://yabb.jriver.com/interact/
Wiki                         : https://wiki.jriver.com/


Installation
------------
1. Download the LyricsFinder : https://github.com/hardolf/JRiver.MediaCenter/releases
2. Unpack the ZIP file       : Setup.zip
3. Run the setup file        : Setup.exe


Installation notes
------------------
The Setup.exe automatically finds any installed JRiver Media Center versions and the user can select which one to install the plug-in to.
Only one version of JRiver Media Center can be "plugged-in" on the same machine, as the plug-in is registered via COM.
The standalone program connects to the JRiver Media Center version matching the URL in the standalone program's configuration.
