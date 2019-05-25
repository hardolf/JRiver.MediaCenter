
The LyricsFinder solution contains a standalone program and a plug-in for the JRiver Media Center

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
JRiver Media Center 24 or newer, using its builtin REST Web service (MCWS).


Compatibility
-------------
The LyricsFinder is expected to be compatible with future versions of JRiver Media Center, as long as these feature the REST Web service (MCWS).
The plugin should also be compatible with future versions of JRiver Media Center. The only requirement is editing and compiling the Plugin Installation Script.iss file.


Development environment
-----------------------
Microsoft Visual Studio 2017, 2019, ..., minimum Community edition, C#
Inno Setup Compiler 6.0.2 (only needed for changes of the setup program, e.g. if a new Media Center version is installed)


Current lyrics services
-----------------------
Chart Lyrics                 : http://www.chartlyrics.com (No restrictions)
FANDOM LyricWiki             : http://lyrics.wikia.com/wiki/LyricWiki (Pending)
Musixmatch                   : https://www.musixmatch.com (Account required, max. 2.000 free requests per day for 30% of the lyrics text, paid plans for full lyrics)
STANDS4 Network Lyrics.com   : https://www.lyrics.com (Account required, max. 100 free requests per day)


Relevant links
--------------
JRiver                       : https://www.jriver.com/
Forum                        : https://yabb.jriver.com/interact/
Wiki                         : https://wiki.jriver.com/
DevZone                      : https://wiki.jriver.com/index.php/DevZone
Plug-ins and accessories     : https://accessories.jriver.com/mediacenter/accessories.php
Inno Setup Compiler          : http://www.jrsoftware.org/


Projects
--------
LyricServices\*              : Lyric service projects, one for each service.
LyricsFinderPlugin           : Plugin for JRiver Media Center.
LyricsFinderCore             : Core project used by both standalone and plugin.
LyricsFinderExe              : Standalone program, does not need the plugin.
MessageInspection            : Utility module for SOAP message inspections.
Utility                      : General utility module.


Installation
------------
Run the relevant installation program:

LyricsFinderPlugin           : ...\MediaCenter\LyricsFinder\Installation\Output\SetupPlugin.exe
LyricsFinderExe              : ...\MediaCenter\LyricsFinder\Installation\Output\SetupStandalone.exe

The plugin installation into other versions than JRiver Media Center 24 requires another edit and compile of the SetupPlugin program, using the Inno Setup Compiler.
