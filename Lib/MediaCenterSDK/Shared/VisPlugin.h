#pragma once

#include <atlbase.h>
#include <tchar.h>

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// some CString constructors will be explicit
#define _CONVERSION_DONT_USE_THREAD_LOCALE	// ATL character set conversion won't use thread-specific settings (needed for non-US systems)

#define MAX_PATH_SAFE								1024

/*******************************************************************************************
VisData - the actual data structure of mapped data from Media Jukebox
*******************************************************************************************/
struct MAPPED_VIS_DATA
{
	char cWaveData[2][1024];
	unsigned int nPositionMS;
	DWORD dwLastUpdateTickCount;
}; 

/*******************************************************************************************
VisData - the expanded vis data structure
*******************************************************************************************/
struct VIS_DATA
{
	char cWaveData[2][1024];
	unsigned int nPositionMS;
	unsigned char ucSpectData[2][512];
}; 

/*******************************************************************************************
VIS_INFO_FIELDS
*******************************************************************************************/
enum VIS_INFO_FIELDS
{
	VIS_DATA_IMPACT,					// impact or punch of the music (100 is quite a bit, but can be higher)
	VIS_DATA_BEAT,						// whether it's currently a beat (0 or 1)
	VIS_DATA_FPS,						// the current frames per second * 1000 (32.1 fps = 32100)
	VIS_DATA_TRACK_CHANGED,				// whether the current track just changed (0 or 1)
};

/*******************************************************************************************
TRACK_INFO_FIELDS
*******************************************************************************************/
enum TRACK_INFO_FIELDS
{
	FIELD_ARTIST,
	FIELD_ALBUM,
	FIELD_NAME,
	FIELD_YEAR,
	FIELD_GENRE,
	FIELD_IMAGEFILE,
	FIELD_NOTES,
	FIELD_LYRICS,
	FIELD_CUSTOM1,
	FIELD_CUSTOM2,
	FIELD_CUSTOM3,
	FIELD_NUMBERTRACKINFOFIELDS
};

struct PLAYBACK_INFO
{
	wchar_t cFilename[MAX_PATH];
	float fReplayGainRadioMultiplier;
	int nPlayingNowPosition;
	int nPlayingNowTotalTracks;
	int nTrackTotalTime;
	int nTrackElapsedTime;
	int nBitrate;
	int nChannels;
	int nSampleRate;
	int nFileChangeCounter;
	unsigned int nKey;
};

/*******************************************************************************************
IVisData - implementation
*******************************************************************************************/
class IVisData
{
public:
	
	IVisData(){};
	virtual ~IVisData(){};

	virtual int GetData() = 0;
	virtual int Destroy() = 0;

	virtual VIS_DATA * GetVisData() = 0;
	virtual int GetVisInfo(VIS_INFO_FIELDS nID) = 0;
	virtual PLAYBACK_INFO * GetPlaybackInfo() = 0;
	virtual LPCTSTR GetTrackInfo(TRACK_INFO_FIELDS nField) = 0;
};

class IVisRedrawHelper  
{
public:

	virtual ~IVisRedrawHelper();

	virtual void Start(HWND hwndWindow, int nThreadPriority = THREAD_PRIORITY_LOWEST) = 0;
	virtual void Stop() = 0;
	
	virtual void SetDisplayWindow(HWND hwndDisplay) = 0;
	virtual void SetForceRedraw(BOOL bForce) = 0;

	virtual int ConvertTimeToFPS(int nSleep) = 0;
	virtual int ConvertFPSToTime(int nFPS) = 0;
};
