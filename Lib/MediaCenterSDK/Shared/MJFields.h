#pragma	once

#ifndef _IT
#define _IT _T
#endif

// database fields (general)
#define MF_FILENAME						_IT("Filename")
#define MF_ARTIST						_IT("Artist")
#define MF_ALBUM						_IT("Album")
#define MF_NAME							_IT("Name")
#define MF_GENRE						_IT("Genre")
#define MF_COMMENT						_IT("Comment")
#define MF_BAND							_IT("Band")
#define MF_FILETYPE						_IT("File Type")
#define MF_DATE							_IT("Date")
#define MF_BITRATE						_IT("Bitrate")
#define MF_IMAGEFILE					_IT("Image File")
#define MF_LASTPLAYED					_IT("Last Played")
#define MF_RATING						_IT("Rating")
#define MF_FILESIZE						_IT("File Size")
#define MF_DURATION						_IT("Duration")
#define MF_NUMBERPLAYS					_IT("Number Plays")
#define MF_TRACKNUMBER					_IT("Track #")
#define MF_DISCNUMBER					_IT("Disc #")
#define MF_DATECREATED					_IT("Date Created")
#define MF_MEDIATYPE					_IT("Media Type")
#define MF_DATEMODIFIED					_IT("Date Modified")
#define MF_DATEIMPORTED					_IT("Date Imported")
#define MF_LYRICS						_IT("Lyrics")
#define MF_NOTES						_IT("Notes")
#define MF_COMPOSER						_IT("Composer")
#define MF_CONDUCTOR					_IT("Conductor")
#define MF_KEYWORDS						_IT("Keywords")
#define MF_ALBUMARTIST					_IT("Album Artist")
#define MF_PEOPLE						_IT("People")
#define MF_PLACES						_IT("Places")
#define MF_EVENTS						_IT("Events")
#define MF_PLAYBACKRANGE				_IT("Playback Range")
#define MF_ACCESSRATING					_IT("Access Rating")
#define MF_TEXT							_IT("Text")
#define MF_COMPRESSION					_IT("Compression")
#define MF_BOOKMARK						_IT("Bookmark")
#define MF_COPYRIGHT					_IT("Copyright")
#define MF_READONLY						_T("ReadOnly")
#define MF_PROTECTION_TYPE              _T("Protection Type")
#define MF_CONTENT_DESCRIPTION			_IT("Description")
#define MF_THUMBNAIL_OFFSET_SMALL		_IT("Thumbnail Offset Small")
#define MF_THUMBNAIL_OFFSET_MEDIUM		_IT("Thumbnail Offset Medium")
#define MF_THUMBNAIL_OFFSET_LARGE		_IT("Thumbnail Offset Large")
#define MF_THUMBNAIL_INFO_LOCAL			_IT("Thumbnail Info (Local)")
#define MF_THUMBNAIL_INFO				_IT("Thumbnail Info")
#define MF_CAPTION						_IT("Caption")
#define MF_MEDIASUBTYPE					_IT("Media Sub Type")
#define MF_SKIPCOUNT					_IT("Skip Count")
#define MF_LASTSKIPPED					_IT("Last Skipped")
#define MF_USE_BOOKMARKING              _IT("Use Bookmarking")
#define MF_STACK_TOP					_IT("Stack Top")
#define MF_STACK_FILES					_IT("Stack Files")

// database fields (audio specific)
#define MF_REPLAYGAIN					_IT("Replay Gain")
#define MF_PEAKLEVEL					_IT("Peak Level")
#define MF_INTENSITY					_IT("Intensity")
#define MF_BPM							_T("BPM")
#define MF_SAMPLERATE					_IT("Sample Rate")
#define MF_CHANNELS						_IT("Channels")
#define MF_BITDEPTH						_IT("Bit Depth")

// database fields (image specific)
#define MF_WIDTH						_IT("Width")
#define MF_HEIGHT						_IT("Height")
#define MF_BAD_PIXELS					_IT("Bad Pixels")
#define MF_CAMERA						_IT("Camera")
#define MF_ROTATION						_IT("Rotation")
#define MF_APERTURE						_IT("Aperture")
#define MF_ISO							_IT("ISO")
#define MF_SHUTTER_SPEED				_IT("Shutter Speed")
#define MF_FOCAL_LENGTH					_IT("Focal Length")
#define MF_FLASH						_IT("Flash")

// database fields (image specific - GPS)
#define MF_LATITUDE						_IT("Latitude")
#define MF_LONGITUDE					_IT("Longitude")
#define MF_ALTITUDE						_IT("Altitude")
#define MF_DIRECTION					_IT("Direction")

// database fields (video series specific)
#define MF_SERIES_NAME					_IT("Series")
#define MF_SEASON_NUMBER				_IT("Season")
#define MF_EPISODE_NUMBER				_IT("Episode")

// non-DB fields
#define MF_TOOLNAME						_T("Tool Name")
#define MF_TOOLVERSION					_T("Tool Version")

// obsolete / currently unused fields
#define MF_DISKLOOKUPKEY				_T("Disk Lookup Key")
#define MF_YEAR							_T("Year")

// special fields
#define MF_PLAYLISTS					_T("Playlists")

// album-analyzer fields
#define MF_COMPLETE_ALBUM				_IT("Complete Album")
#define MF_MIX_ALBUM					_IT("Mix Album")
#define MF_ALBUMGAIN					_IT("Album Gain")

// calculated fields
#define MF_VOLUME_NAME					_IT("Volume Name")
#define MF_ALBUM_ARTIST_AUTO			_IT("Album Artist (auto)")
#define MF_ALBUM_TYPE					_IT("Album Type")
#define MF_REMOVABLE					_IT("Removable")
#define MF_DATE_DAY						_IT("Date (day)")
#define MF_DATE_MONTH					_IT("Date (month)")
#define MF_DATE_YEAR					_IT("Date (year)")
#define MF_DATE_FILENAME_FRIENDLY		_IT("Date (filename friendly)")
#define MF_FILENAME_NAME				_IT("Filename (name)")
#define MF_FILENAME_PATH				_IT("Filename (path)")

// template-based calculated fields
#define MF_ARTIST_ALBUM_YEAR			_IT("Artist - Album (Year)")
#define MF_YEAR_ALBUM					_IT("Year - Album")
#define MF_DIMENSIONS					_IT("Dimensions")

// fields that can get created on upgrade (legacy MJ support)
#define MF_CUSTOM1						_T("Custom 1")
#define MF_CUSTOM2						_T("Custom 2")
#define MF_CUSTOM3						_T("Custom 3")

// MusicMatch fields
#define MF_MM_TEMPO						_IT("Tempo")
#define MF_MM_MOOD						_IT("Mood")
#define MF_MM_SITUATION					_IT("Situation")
#define MF_MM_BIOS						_IT("Bios")
#define MF_MM_PREFERENCE				_IT("Preference")

// Podcast fields
#define MF_FEED_URL						_T("Feed URL")
#define MF_EPISODE_URL					_T("Episode URL")

// Web Media fields
#define MF_WEB_MEDIA_URL				_T("Web Media URL")

// service fields 
#define MF_SERVICE_NAME					_T("Service: Name")
#define MF_SERVICE_ID					_T("Service: ID")
#define MF_SERVICE_ITEM_INFO			_T("Service: Item Info")
#define MF_SERVICE_STREAM_METADATA		_T("Service: Stream Metadata")

// WM DRM fields
#define MF_DRM_CONTENT_ID				_IT("Content ID")
#define MF_DRM_CONTENT_DISTRIBUTOR		_IT("Content Distributor")
#define MF_DRM_CONTENT_TYPE				_IT("Content Type")
#define MF_DRM_PROTECTED				_IT("Protected")
#define MF_DRM_NUMBER_BURNS				_IT("Number Burns")
#define MF_DRM_NUMBER_UPLOADS			_IT("Number Uploads")
#define MF_DRM_EXPIRATION_DATE          _IT("Expiration Date")

// CD database fields
#define MF_CDDB_COVERARTURL				_IT("Coverart URL")

// constants
#define MJ_IMAGE_FILE_INTERNAL			_T("INTERNAL")

// media types
#define MEDIATYPE_AUDIO					_IT("Audio")
#define MEDIATYPE_IMAGE					_IT("Image")
#define MEDIATYPE_VIDEO					_IT("Video")
#define MEDIATYPE_TV					_IT("TV")
#define MEDIATYPE_DATA					_IT("Data")
#define MEDIATYPE_PLAYLIST				_IT("Playlist")
#define MEDIATYPE_UNKNOWN				_IT("Unknown")

// media types
#define MEDIASUBTYPE_PODCAST			_IT("Podcast")
#define MEDIASUBTYPE_AUDIOBOOK			_IT("Audiobook")
#define MEDIASUBTYPE_RADIO				_IT("Radio")
#define MEDIASUBTYPE_MOVIE				_IT("Movie")
#define MEDIASUBTYPE_MUSICVIDEO			_IT("Music Video")
#define MEDIASUBTYPE_TVSHOW				_IT("TV Show")
#define MEDIASUBTYPE_UNKNOWN			_IT("Unknown")

// protection types (blank if unprotected)
#define PROTECTIONTYPE_PURCHASED		_IT("Purchased")
#define PROTECTIONTYPE_SUBSCRIPTION		_IT("Subscription")

// database location flags for mediafile info objects
#define DB_LOCATION_INVALID				0
#define DB_LOCATION_MAIN				(1 << 0)
#define DB_LOCATION_PLAYING_NOW			(1 << 1)
#define DB_LOCATION_CD					(1 << 2)
#define DB_LOCATION_EXPLORER			(1 << 3)
#define DB_LOCATION_UNASSIGNED			(1 << 4)
#define DB_LOCATION_FILE_TRANSFER		(1 << 5)
#define DB_LOCATION_HANDHELD			(1 << 6)
#define DB_LOCATION_GROUPING			(1 << 7)
#define DB_LOCATION_REMOVED				(1 << 8)
#define DB_LOCATION_DOWNLOADING			(1 << 9)
#define DB_LOCATION_PODCAST_FEED		(1 << 10)
#define DB_LOCATION_PODCAST_KEEP		(1 << 11)
#define DB_LOCATION_PLAYLIST			(1 << 12)

#define DB_LOCATION_ALL					(0xFFFFFFFF)
#define DB_LOCATION_ALBUM_CHECK			(DB_LOCATION_MAIN | DB_LOCATION_CD)
#define DB_LOCATION_TAG					(DB_LOCATION_MAIN | DB_LOCATION_PLAYING_NOW | DB_LOCATION_EXPLORER | DB_LOCATION_UNASSIGNED)
#define DB_LOCATION_CLEAN				(DB_LOCATION_PLAYING_NOW | DB_LOCATION_EXPLORER | DB_LOCATION_FILE_TRANSFER | DB_LOCATION_HANDHELD | DB_LOCATION_UNASSIGNED | DB_LOCATION_GROUPING | DB_LOCATION_PODCAST_FEED | DB_LOCATION_DOWNLOADING)
