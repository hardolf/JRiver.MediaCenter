/******************************************************************************************
Description: Media Jukebox Input Plugin Sample
File Type: RAW
Copyright: Copyright (c) 2001-2008 J. River, Inc.

Description: Media Jukebox uses a CMJFileInfo class to get information about the files a
plugin supports.  It uses this information when playing or importing files.

Basically you need to derive a class from the CMJFileInfo base for your format.  Then,
you need to answer questions about the file when Media Jukebox asks.  Media Jukebox will
also tell you when you should save information about the file. (like tag info)
******************************************************************************************/
#include <windows.h>
#include <stdio.h>
#include <tchar.h>
#include "MJInputSource.h"
#include "GeneralReader.h"

#define BITS_PER_SAMPLE		16
#define SAMPLE_RATE			44100
#define CHANNELS			2

/******************************************************************************************
The file info class for our format (RAW), which is derived from CMJFileInfo
******************************************************************************************/
class CMJRAWFileInfo : public CMJFileInfo 
{
public:

	CMJRAWFileInfo();
	~CMJRAWFileInfo();

	BOOL GetAttribute(LPCTSTR Name, LPTSTR Value, int * maxSize);
	BOOL SetAttribute(LPCTSTR Name, LPCTSTR Value);
	BOOL GetFormatString(LPTSTR Value, int * maxSize);
	
	BOOL Open(LPCTSTR pFilename);
	BOOL Close();
	
	int DoCommand(MJ_FILE_INFO_COMMANDS nCommandID, int nParam1 = 0, int nParam2 = 0);
	
private:		
	
	INT64				m_nFileBytes;
	IReader			 *	m_pReader;
};

/******************************************************************************************
Constructor 

Initialize member variables and get a reader helper, so that you can get a reader later.
******************************************************************************************/
CMJRAWFileInfo::CMJRAWFileInfo()
{
	m_nFileBytes = 0;
	m_pReader = NULL;
}

/******************************************************************************************
Desturctor

Clean up...
******************************************************************************************/
CMJRAWFileInfo::~CMJRAWFileInfo()
{
	Close();
}

/******************************************************************************************
Open a file

Get a reader, open the file with the reader, and analyze the file
******************************************************************************************/
BOOL CMJRAWFileInfo::Open(LPCTSTR pFilename)
{	
	m_pReader = m_pOwner->CreateReader(pFilename);
	if (m_pReader == NULL)
		return false;

	if (!m_pReader->Open())
		return false;

	m_nFileBytes = m_pReader->GetLength();
	
	return true;
}

/******************************************************************************************
Open a file

Get a reader, open the file with the reader, and analyze the file
******************************************************************************************/
BOOL CMJRAWFileInfo::Close()
{
	if (m_pReader)
	{
		delete m_pReader;
		m_pReader = NULL;
	}

	return true;
}

/******************************************************************************************
Set attributes

This is for saving information like tag info.  See the list of attribute names in
MJInputSourceLib.h
******************************************************************************************/
BOOL CMJRAWFileInfo::SetAttribute(LPCTSTR Name, LPCTSTR Value)
{
	return false;
}

/******************************************************************************************
Get the file description

This is what is displayed on the "Format" page of the file's properties inside of 
Media Jukebox.  You can display any information you like.
******************************************************************************************/
BOOL CMJRAWFileInfo::GetFormatString(LPTSTR Value, int * maxSize)
{
	if (*maxSize < 256) { *maxSize = 256; return false; }
	_stprintf(Value, _T("Uncompressed Audio File (RAW)\r\n%.1f kHz, %d bit, %d ch"), float(SAMPLE_RATE) / 1000, BITS_PER_SAMPLE, CHANNELS);
	return true;
}

/******************************************************************************************
Get file information

This is how Media Jukebox queries the plugin for information.  See the list of attribute names in
MJInputSourceLib.h
******************************************************************************************/
BOOL CMJRAWFileInfo::GetAttribute(LPCTSTR Name, LPTSTR Value, int * maxSize)
{
	if (*maxSize < 256) { *maxSize = 256; return false; }
	
	if(_tcscmp(Name, _T("BITRATE")) == 0)
	{
		long nBlockAlign = (BITS_PER_SAMPLE / 8) * CHANNELS;
		_stprintf(Value, _T("%d"), (nBlockAlign * SAMPLE_RATE * 8) / 1000);
		
		return true;
	}	
	else if(_tcscmp(Name, _T("DURATION")) == 0)
	{
		long nBlockAlign = (BITS_PER_SAMPLE / 8) * CHANNELS;
		long nBytesPerSecond = (nBlockAlign * SAMPLE_RATE);
		if (nBytesPerSecond > 0)
			_stprintf(Value, _T("%d"), m_nFileBytes / nBytesPerSecond);
		else
			_stprintf(Value, _T("0"));
		
		return true;
	}
	else if(_tcscmp(Name, _T("CHANNELS")) == 0)
	{
		_stprintf(Value, _T("%d"), CHANNELS);
		return true;
	}
	else if(_tcscmp(Name, _T("FREQUENCY")) == 0)
	{
		_stprintf(Value, _T("%d"), SAMPLE_RATE);
		return true;
	}

	return false;
}

int CMJRAWFileInfo::DoCommand(MJ_FILE_INFO_COMMANDS nCommandID, int nParam1, int nParam2)
{
	if (nCommandID == MJ_FILE_INFO_COMMAND_REMOVE_TAG)
	{
		return -1;
	}

	return -1;
}


/******************************************************************************************
The main export (so that Media Jukebox can create a CMJRAWFileInfo class)
******************************************************************************************/
extern "C"
{
	__declspec (dllexport) int GetFileInfo(HINSTANCE hDllInstance, CMJFileInfo **classFileInfo) // return 0 on failure
	{
		*classFileInfo = (CMJFileInfo *) new CMJRAWFileInfo;
		
		return 1;
	}
}
