/******************************************************************************************
Description: Media Jukebox Input Plugin Sample
File Type: RAW
Copyright: Copyright (c) 2001-2008 J. River, Inc.

Description: All MJ plugins are built around the base class CMJInputSource.  To make a plugin
for a new file type, simply create a new class derived from CMJInputSource, such as 
CMJRAWInputSource in this example.  Although using the base class CMJInputSource isn't necessary,
it manages buffering and a few other things that should make your life easier.
******************************************************************************************/
#include <Windows.h>

#include <tchar.h>
#include "MJRAWInputSource.h"
#include "GeneralReader.h"

#define BITS_PER_SAMPLE		16
#define SAMPLE_RATE			44100
#define CHANNELS			2

/******************************************************************************************
Exported function to create an instance of this class
******************************************************************************************/
extern "C"
{
	__declspec (dllexport) CMJInputSource *GetInputSource()
	{
		return new CMJRAWInputSource;
	}
}

/******************************************************************************************
Construction

Initialize any variables here.  One instance of the class will only be used for one file.

Note: override m_nTotalBufferBytes here if you need a working buffer that's a differnet
size than the default (64k)
******************************************************************************************/
CMJRAWInputSource::CMJRAWInputSource()
{

}

/******************************************************************************************
Destruction

Clean everything up.
******************************************************************************************/
CMJRAWInputSource::~CMJRAWInputSource()
{

}

/******************************************************************************************
Open

This gets called when the plugin first opens a file.  If you call the base class
MJInputSource::Open(), it'll take care of opening the file, getting an MJReader, and
creating the buffer.

Then, set the format information and do any other initialization.

A return value of 0 means success.  Any other return will be interpreted as an error code.
******************************************************************************************/
int CMJRAWInputSource::Open(LPCTSTR pFilename)
{
	// use the base class to open the file and create the buffer
	long nRetVal = CMJInputSource::Open(pFilename);
	if (nRetVal != 0) { return nRetVal; }

	// set the length (be careful of overflows)
	INT64 nTotalBytes = m_pReader->GetLength();
	long nBlockAlign = (BITS_PER_SAMPLE / 8) * CHANNELS;
	INT64 nTotalBlocks = nTotalBytes / nBlockAlign;
	m_nLengthMS = nTotalBlocks / (SAMPLE_RATE / 1000);

	// set the audio format
	m_nCurrentSampleRate = SAMPLE_RATE;
	m_nCurrentChannels = CHANNELS;
	m_nCurrentBitsPerSample = BITS_PER_SAMPLE;
	
	// set the bitrate
	m_nCurrentBitrate = (nBlockAlign * SAMPLE_RATE * 8) / 1000;
	
	// return a successful value
	return 0;
}

/******************************************************************************************
Add data to the internal buffer

This gets called whenever Media Jukebox needs more data.  You can add data to the buffer
in any size blocks that you like.  When adding data, follow these steps:

1) get the amount of room in the buffer with GetBufferBytesAvailable()
2) add up to that amount of data to the location specified by GetBufferFillLocation()
3) update the amount of data in the buffer with UpdateBufferSize()

Notes: Of course, you shouldn't add more than the available buffer bytes.  If you need
a larger working buffer, simply modify m_nTotalBufferBytes in the constructor, and
the base class will use that size when creating the buffer in the Open() function.

Update m_nCurrentBitrate in this function if you want a VBR display.
******************************************************************************************/
int CMJRAWInputSource::AddDataToBuffer()
{
	// calculate the amount of room available in the buffer
	long nBufferBytesAvailable = GetBufferBytesAvailable();

	// attempt to fill the buffer with data
	long nBytesRead = m_pReader->Read((char *) GetBufferFillLocation(), nBufferBytesAvailable);
	
	// update the buffer location to reflect the data we just added
	UpdateBufferSize(nBytesRead);

	// set the 'done' flag if we didn't get any more data
	if (nBytesRead == 0) { m_bDone = true; }

	// return a successful value
	return 0;
}

/******************************************************************************************
Get information from the plugin

The fields that can be queried are enumerated in InfoIDEnum.  Be sure to pass unhandled
commands to the base class.
******************************************************************************************/
int CMJRAWInputSource::GetInformation(MJ_INPUT_SOURCE_INFO_FIELDS nInfoID, LPTSTR pResult)
{
	if (nInfoID == MJ_INPUT_SOURCE_INFO_PLUGIN_DESCRIPTION)
	{
		_stprintf(pResult, _T("Media Jukebox RAW Plugin v1.0"));
		return 0;
	}
	
	if (nInfoID == MJ_INPUT_SOURCE_INFO_EXTENSIONS)
	{
		_stprintf(pResult, _T("RAW; Waveform Audio File (*.RAW)"));
		return 0;
	}

	return CMJInputSource::GetInformation(nInfoID, pResult);
}	

/******************************************************************************************
Set information

The fields that can be set are enumerated in InfoIDEnum.  Be sure to pass unhandled
commands to the base class.
******************************************************************************************/
int CMJRAWInputSource::SetInformation(MJ_INPUT_SOURCE_INFO_FIELDS nInfoID, LPCTSTR pValue)
{
	if (nInfoID == MJ_INPUT_SOURCE_INFO_POSITION_MS)
	{
		// get the position
		long nPositionMS = _ttoi(pValue);

		// calculate how many 'blocks' into the file we should seek
		long nBlockOffset = (nPositionMS / 1000) * SAMPLE_RATE;
		long nBlockAlign = (BITS_PER_SAMPLE / 8) * CHANNELS;

		// actually seek
		m_pReader->SetPosition(nBlockOffset * nBlockAlign);

		// reset the internal buffers (*** make sure to do this when seeking ***)
		ResetInternalBuffer();

		// return a successful value
		return 0;
	}

	// forward unhandled messages to the base class
	return CMJInputSource::SetInformation(nInfoID, pValue);
}	

/******************************************************************************************
Perform a command

The plugin commands are enumerated in CommandIDEnum.
******************************************************************************************/
int CMJRAWInputSource::DoCommand(MJ_INPUT_SOURCE_COMMANDS nCommandID, int nParam1, int nParam2)
{
	// let's make a mock configuration dialog
	if (nCommandID == MJ_INPUT_SOURCE_COMMAND_HAS_CONFIGURATION_DIALOG)
	{
		return TRUE;
	}
	else if (nCommandID == MJ_INPUT_SOURCE_COMMAND_SHOW_CONFIGURATION_DIALOG)
	{
		::MessageBox((HWND) nParam1, _T("RAW Plugin Configuration."), _T("Configuration"), MB_OK);
		return 0;
	}

	// forward other commands on
	return CMJInputSource::DoCommand(nCommandID, nParam1, nParam2);
}
