/******************************************************************************************
CMJInputSource - base class for Media Core playback plugins.
Copyright (c) 2001-2008 J. River, Inc.

It's strongly recommend that you DO NOT ALTER this file.  This class is a base building
class for new plugins.  Add all new format specific changes to the new class.
******************************************************************************************/

#pragma once

/************************************************************************************
Input plugins MUST be built in Unicode
************************************************************************************/
#if (!defined(UNICODE) || !defined(_UNICODE))
#error "Input plugins must be built in Unicode mode!"
#endif

/************************************************************************************
Make sure constants are defined
************************************************************************************/
#ifndef MB_OK
#define MB_OK                       0x00000000L
#define MB_OKCANCEL                 0x00000001L
#define MB_ABORTRETRYIGNORE         0x00000002L
#define MB_YESNOCANCEL              0x00000003L
#define MB_YESNO                    0x00000004L
#define MB_RETRYCANCEL              0x00000005L
#define MB_CANCELTRYCONTINUE        0x00000006L
#endif

#ifndef ALIGN64
#define ALIGN64 __declspec(align(64))
#endif

/************************************************************************************
Forward declares
************************************************************************************/
class IReader;

/************************************************************************************
Information fields
************************************************************************************/
enum MJ_INPUT_SOURCE_INFO_FIELDS
{
	MJ_INPUT_SOURCE_INFO_CURRENT_BITRATE,
	MJ_INPUT_SOURCE_INFO_LENGTH_MS,
	MJ_INPUT_SOURCE_INFO_PLUGIN_DESCRIPTION,
	MJ_INPUT_SOURCE_INFO_POSITION_MS,
	MJ_INPUT_SOURCE_INFO_EXTENSIONS,
	MJ_INPUT_SOURCE_INFO_ARTIST,
	MJ_INPUT_SOURCE_INFO_ALBUM,
	MJ_INPUT_SOURCE_INFO_TITLE,
	MJ_INPUT_SOURCE_INFO_SAMPLE_RATE,
	MJ_INPUT_SOURCE_INFO_CHANNELS,
	MJ_INPUT_SOURCE_INFO_BITS_PER_SAMPLE,
	MJ_INPUT_SOURCE_LAST_ERROR_MESSAGE,
	MJ_INPUT_SOURCE_INFO_LENGTH_BLOCKS,
	MJ_INPUT_SOURCE_INFO_PREVIOUS_CHAPTER_MS,
	MJ_INPUT_SOURCE_INFO_NEXT_CHAPTER_MS,
	MJ_INPUT_SOURCE_INFO_SUPPORT_BOOKMARK,
	MJ_INPUT_SOURCE_INFO_SUPPORT_CHAPTERS,
	MJ_INPUT_SOURCE_INFO_IMJAUTOMATION,
	MJ_INPUT_SOURCE_INFO_IWMREADER,
	MJ_INPUT_SOURCE_INFO_HEADER_KILL_BLOCKS,
	MJ_INPUT_SOURCE_INFO_FOOTER_KILL_BLOCKS,
	MJ_INPUT_SOURCE_INFO_CD_TRACK_COUNT,
	MJ_INPUT_SOURCE_INFO_CD_TRACK_POS,
	MJ_INPUT_SOURCE_INFO_NEXT_MP3_FRAME_INFO,
	MJ_INPUT_SOURCE_INFO_DISC_CAPACITY_SECONDS,
	MJ_INPUT_SOURCE_INFO_CACHE_ALL_METADATA,
	MJ_INPUT_SOURCE_INFO_GET_CACHED_METADATA,
	MJ_INPUT_SOURCE_INFO_GET_CACHED_METADATA_CHARACTERS,
	MJ_INPUT_SOURCE_INFO_IVERSION,
	MJ_INPUT_SOURCE_INFO_ATTEMPT_LICENSING_ONLY,
	MJ_INPUT_SOURCE_INFO_LICENSE_INFO,
	MJ_INPUT_SOURCE_INFO_ENCODER,
	MJ_INPUT_SOURCE_INFO_IPOD_GAPLESS_MAGIC_NUMBER,
};

// when these are queried, use the define MJ_INPUT_SOURCE_GET_INFORMATION_CHARACTERS
// for the safe amount of data the receiving buffer can hold (since a size wasn't
// specified in the original GetInformation(...) function)
#define MJ_INPUT_SOURCE_GET_INFORMATION_CHARACTERS			65536

/************************************************************************************
Commands
************************************************************************************/
enum MJ_INPUT_SOURCE_COMMANDS
{
	MJ_INPUT_SOURCE_COMMAND_SHOW_ABOUT_DIALOG,
	MJ_INPUT_SOURCE_COMMAND_SHOW_CONFIGURATION_DIALOG,
	MJ_INPUT_SOURCE_COMMAND_THREAD_SAFE_CANCEL,
	MJ_INPUT_SOURCE_COMMAND_HAS_ABOUT_DIALOG,
	MJ_INPUT_SOURCE_COMMAND_HAS_CONFIGURATION_DIALOG,
	MJ_INPUT_SOURCE_COMMAND_SET_RIGHTS,
};

/************************************************************************************
Output modes
************************************************************************************/
enum MJ_OUTPUT_MODES
{
	MJ_OUTPUT_MODE_UNDEFINED,		// unknown
	MJ_OUTPUT_MODE_NONE,			// no output intended -- only being used for analysis
	MJ_OUTPUT_MODE_PLAY,			// playback
	MJ_OUTPUT_MODE_BURN,			// burning
	MJ_OUTPUT_MODE_HANDHELD,		// handheld
	MJ_OUTPUT_MODE_SETTOP_BOX,		// settop box
	MJ_OUTPUT_MODE_FILE,			// file (conversion, etc.)
};

/************************************************************************************
Defines
************************************************************************************/
#define MAX_PATH_SAFE							1024
#define MJ_INPUT_SOURCE_ERROR_SUPRESS_MESSAGE	_T("<Suppress Error Message>")	
#define MJ_INPUT_SOURCE_CURRENT_IVERSION		2

/************************************************************************************
IMJInputOwner - interface provided by the framework to allow an input source
to communicate back with the main program, call helper functions, etc.
************************************************************************************/
class IMJInputOwner
{
public:

	virtual IReader * CreateReader(LPCTSTR pResourceName) = 0;
	virtual BOOL GetInstallPath(LPTSTR pPath) = 0;
	virtual BOOL GetTemporaryFilename(LPTSTR pOutput, LPCTSTR pName, LPCTSTR pExtensionWithoutDot, 
		BOOL bMakeThreadSafe = TRUE) = 0;
};

/************************************************************************************
CMJInputSource - base class for Media Core playback plugins
************************************************************************************/
class CMJInputSource
{
public:

	// construction / destruction
	CMJInputSource();
	virtual ~CMJInputSource();

	// public functions
    virtual int	Open(LPCTSTR pFilename);
	virtual int	GetInformation(MJ_INPUT_SOURCE_INFO_FIELDS nInfoID, LPTSTR pResult);
	virtual int	SetInformation(MJ_INPUT_SOURCE_INFO_FIELDS nInfoID, LPCTSTR pValue);
	virtual int	DoCommand(MJ_INPUT_SOURCE_COMMANDS nCommandID, int nParam1 = 0, int nParam2 = 0);
    virtual int	GetSamples(BYTE * pSampleBuffer, int nBytes);
	
protected:

	// protected functions
	virtual int	AddDataToBuffer();
	virtual int	ResetInternalBuffer();

	// simple buffer helper functions
	int GetBufferBytesAvailable() { return m_nTotalSampleBufferBytes - m_nSampleBufferBytes; }
	BYTE * GetBufferFillLocation() { return (BYTE *) &m_pSampleBuffer[m_nSampleBufferBytes]; }
	void UpdateBufferSize(int nBytesAdded) { m_nSampleBufferBytes += max(nBytesAdded, 0); }
	void DeleteSampleBuffer();
	
	// owner
	IMJInputOwner * m_pOwner;

	// reader (handles I/O)
	IReader * m_pReader;
	
	// buffer
	BYTE * m_pSampleBuffer;
	int m_nSampleBufferBytes;
	int	m_nTotalSampleBufferBytes;	

	// length and format
	int	m_nCurrentBitrate;
	int	m_nCurrentSampleRate;
	int	m_nCurrentBitsPerSample;
	int	m_nCurrentChannels;

	// because of a MSVC 7.1 compiler bug, we need to manually ensure that __int64 variables occur
	// on 8-byte boundaries -- without doing this, the base class can have &m_nLengthBlocks offset
	// that is on a 4 byte boundary while any derived classes in a different CPP file will use 
	// an 8 byte boundary meaning effectively that (&__super::m_nLengthBlocks != &m_nLengthBlocks) 
	// which leads to data corruption and an inability to set member variables
	ALIGN64 __int64 m_nLengthBlocks;
	ALIGN64 __int64 m_nLengthMS;
	
	// other flags
	BOOL m_bDone;

private:

	// private data
	int	m_nSampleBufferHead;
};

/************************************************************************************
Commands
************************************************************************************/
enum MJ_FILE_INFO_COMMANDS
{
	MJ_FILE_INFO_COMMAND_REMOVE_TAG,
	MJ_FILE_INFO_COMMAND_CAN_UPDATE_WHILE_PLAYING,
	MJ_FILE_INFO_COMMAND_GET_REMOVE_TAG_PARAM,
};

/*************************************************************************************
CMJFileInfo - base class for handling tagging
*************************************************************************************/
class CMJFileInfo
{
public:

	// construction / destruction
	CMJFileInfo();
	virtual ~CMJFileInfo();
	
	// public functions
	virtual BOOL GetAttribute(LPCTSTR Name, LPTSTR Value, int * pnCharacters);
	virtual BOOL SetAttribute(LPCTSTR Name, LPCTSTR Value);
	virtual BOOL GetFormatString(LPTSTR Value, int * pnCharacters);
	virtual BOOL Open(LPCTSTR pFilename);
	virtual BOOL Close();
	virtual int DoCommand(MJ_FILE_INFO_COMMANDS nCommandID, int nParam1 = 0, int nParam2 = 0);

protected:

	// owner
	IMJInputOwner * m_pOwner;
};

/*************************************************************************************
Creation function declarations
*************************************************************************************/
typedef BOOL (*proc_SetOwner)(IMJInputOwner * pOwner);
typedef CMJInputSource * (*proc_GetInputSource)();
typedef int (*proc_GetFileInfo)(HINSTANCE hDllInstance, CMJFileInfo ** ppFileInfo);
