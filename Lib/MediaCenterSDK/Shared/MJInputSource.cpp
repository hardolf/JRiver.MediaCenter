/******************************************************************************************
CMJInputSource - base class for Media Center playback plugins.
Copyright (c) 2001-2008 J. River, Inc.

It's strongly recommend that you DO NOT ALTER this file.  This class is a base building
class for new plugins.  Add all new format specific changes to the new class.

Last updated: February 8, 2006
******************************************************************************************/
#include "stdafx.h"
#include <windows.h>
#include <tchar.h>
#include <atlbase.h>
#include "MJInputSource.h"
#include "GeneralReader.h"

/************************************************************************************
Store the owner (this will be called by the framework and remain valid as
long as the DLL is loaded)
************************************************************************************/
IMJInputOwner * g_pOwner = NULL;
extern "C"
{
	__declspec (dllexport) BOOL SetOwner(IMJInputOwner * pOwner)
	{
		g_pOwner = pOwner;
		return TRUE;
	}
}

/******************************************************************************************
CMJInputSource
******************************************************************************************/
CMJInputSource::CMJInputSource()
{
	// store the owner
	m_pOwner = g_pOwner;

	// initialize format
	m_nLengthMS = -1i64;
	m_nLengthBlocks = -1i64;
	m_nCurrentBitrate = -1;
	m_nCurrentSampleRate = -1;
	m_nCurrentBitsPerSample = -1;
	m_nCurrentChannels = -1;

	// initialize buffer
	m_pSampleBuffer = NULL;
	m_nSampleBufferBytes = -1;
	m_nSampleBufferHead = 0;
	m_nTotalSampleBufferBytes = 65536;
	
	// initialize everything else
	m_pReader = NULL;
	m_bDone = FALSE;
}

CMJInputSource::~CMJInputSource()
{
	// clean up
	DeleteSampleBuffer();
	
	if (m_pReader)
	{
		delete m_pReader;
		m_pReader = NULL;
	}
}

void CMJInputSource::DeleteSampleBuffer()
{
	if (m_pSampleBuffer)
	{
		delete [] m_pSampleBuffer;
		m_pSampleBuffer = NULL;
	}
}

int CMJInputSource::Open(LPCTSTR pFilename)
{
	// reset variables
	m_nSampleBufferBytes = 0;

	// get a reader
	m_pReader = m_pOwner->CreateReader(pFilename);

	// open
	int nRetVal = -1;
	if (m_pReader != NULL)
	{
		// no need for write access
		m_pReader->SetOption(READER_OPTION_READ_ONLY, _T("1"));

		// perform actual open
		if (m_pReader->Open() != FALSE)
		{ 
			// create the buffer
			m_pSampleBuffer = new BYTE [m_nTotalSampleBufferBytes];
			if (m_pSampleBuffer != NULL) 
			{ 
				nRetVal = 0;
			}
		}
	}
	
	// return
	return nRetVal;
}

int CMJInputSource::GetSamples(BYTE * pSampleBuffer, int nBytes)
{
	int nBytesRetrieved = 0;
	int nBytesNeeded = nBytes;
	DWORD dwTick = GetTickCount();
	BOOL bLoop = TRUE;

	while (bLoop && (nBytesNeeded > 0))
	{
		// get as much as possible from the sample buffer
		int nBytesAvailable = m_nSampleBufferBytes - m_nSampleBufferHead;
		int nBytesToAdd = min(nBytesAvailable, nBytesNeeded);
		if (nBytesToAdd > 0)
		{
			// copy data out of the buffer and update it
			memcpy(&pSampleBuffer[nBytesRetrieved], &m_pSampleBuffer[m_nSampleBufferHead], nBytesToAdd);
			nBytesRetrieved += nBytesToAdd;
			nBytesNeeded -= nBytesToAdd;
			m_nSampleBufferHead += nBytesToAdd;
		}

		// if the buffer is empty, update
		if ((m_nSampleBufferBytes - m_nSampleBufferHead) <= 0)
		{
			// reset variables
			m_nSampleBufferBytes = 0;
			m_nSampleBufferHead = 0;

			// get more data if we're not done
			if (m_bDone == FALSE)
			{
				int nOriginalSampleBufferBytes = m_nSampleBufferBytes;
				int nAddRetVal = AddDataToBuffer();
				int nBytesAdded = m_nSampleBufferBytes - nOriginalSampleBufferBytes;

				// if there was an error, break the while loop
				if (nAddRetVal != 0)
					bLoop = FALSE;

				// if we didn't add any data, break (we'll return -1 and try again)
				if (nBytesAdded <= 0)
					bLoop = FALSE;

				// if there is no data, and we've been working for over 100 ms, break
				if ((m_nSampleBufferBytes == 0) && ((GetTickCount() - dwTick) > 100))
					bLoop = FALSE;
			}
			else
			{
				// if we're done, simply stop the loop
				bLoop = FALSE;
			}
		}
	}

	// handle the special case where we broke out of the loop, but should try again
	if ((nBytesRetrieved == 0) && (m_bDone == FALSE))
		nBytesRetrieved = -1;

	return nBytesRetrieved;
}

int CMJInputSource::ResetInternalBuffer()
{
	m_nSampleBufferHead = 0;
	m_nSampleBufferBytes = 0;

	return 0;
}

int CMJInputSource::GetInformation(MJ_INPUT_SOURCE_INFO_FIELDS nInfoID, LPTSTR pResult)
{
	BOOL bHandled = TRUE;

	switch (nInfoID)
	{
		case MJ_INPUT_SOURCE_INFO_CURRENT_BITRATE:
			_stprintf(pResult, _T("%d"), m_nCurrentBitrate);
			break;
		case MJ_INPUT_SOURCE_INFO_LENGTH_MS:
			_stprintf(pResult, _T("%I64d"), m_nLengthMS);
			break;
		case MJ_INPUT_SOURCE_INFO_LENGTH_BLOCKS:
			_stprintf(pResult, _T("%I64d"), m_nLengthBlocks);
			break;
		case MJ_INPUT_SOURCE_INFO_PLUGIN_DESCRIPTION:
			_stprintf(pResult, _T("Unknown Input Plugin"));
			break;
		case MJ_INPUT_SOURCE_INFO_SAMPLE_RATE:
			_stprintf(pResult, _T("%d"), m_nCurrentSampleRate);
			break;
		case MJ_INPUT_SOURCE_INFO_CHANNELS:
			_stprintf(pResult, _T("%d"), m_nCurrentChannels);
			break;
		case MJ_INPUT_SOURCE_INFO_BITS_PER_SAMPLE:
			_stprintf(pResult, _T("%d"), m_nCurrentBitsPerSample);
			break;
		case MJ_INPUT_SOURCE_INFO_IVERSION:
			_stprintf(pResult, _T("%d"), MJ_INPUT_SOURCE_CURRENT_IVERSION);
			break;
		default:
			bHandled = FALSE;
	}

	return bHandled ? 0 : -1;
}	

int CMJInputSource::SetInformation(MJ_INPUT_SOURCE_INFO_FIELDS nInfoID, LPCTSTR pValue)
{
	return -1;
}	

int CMJInputSource::DoCommand(MJ_INPUT_SOURCE_COMMANDS nCommandID, int nParam1, int nParam2)
{
	if (nCommandID == MJ_INPUT_SOURCE_COMMAND_THREAD_SAFE_CANCEL)
	{
		if (m_pReader)
		{
			m_pReader->Close();
			return 0;
		}
	}

	return -1;
}

int CMJInputSource::AddDataToBuffer()
{
	return -1;
}

/*************************************************************************************
CMJFileInfo - base class for handling tagging
*************************************************************************************/
CMJFileInfo::CMJFileInfo()
{
	// store the owner
	m_pOwner = g_pOwner;
}

CMJFileInfo::~CMJFileInfo()
{
}


BOOL CMJFileInfo::GetAttribute(LPCTSTR Name, LPTSTR Value, int * pnCharacters)
{
	UNREFERENCED_PARAMETER(Name); UNREFERENCED_PARAMETER(Value); UNREFERENCED_PARAMETER(pnCharacters);
	return FALSE;
}

BOOL CMJFileInfo::SetAttribute(LPCTSTR Name, LPCTSTR Value)
{
	UNREFERENCED_PARAMETER(Name); UNREFERENCED_PARAMETER(Value);
	return FALSE;
}

BOOL CMJFileInfo::GetFormatString(LPTSTR Value, int * pnCharacters)
{
	UNREFERENCED_PARAMETER(Value); UNREFERENCED_PARAMETER(pnCharacters);
	return FALSE;
}

BOOL CMJFileInfo::Open(LPCTSTR pFilename)
{
	UNREFERENCED_PARAMETER(pFilename);
	return FALSE;
}

BOOL CMJFileInfo::Close() 
{ 
	return FALSE; 
}

int CMJFileInfo::DoCommand(MJ_FILE_INFO_COMMANDS nCommandID, int nParam1, int nParam2)
{
	UNREFERENCED_PARAMETER(nCommandID); UNREFERENCED_PARAMETER(nParam1); UNREFERENCED_PARAMETER(nParam2);
	return -1;
}
