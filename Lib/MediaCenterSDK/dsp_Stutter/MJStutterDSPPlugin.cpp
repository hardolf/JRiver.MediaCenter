// MJStutterDSPPlugin.cpp: implementation of the CMJStutterDSPPlugin class.
//
//////////////////////////////////////////////////////////////////////
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

#include "stdafx.h"
#include "dsp_Stutter.h"
#include "MJStutterDSPPlugin.h"
#include "StutterDlg.h"

#define STUTTER_COUNT			2

CMJStutterDSPPlugin::CMJStutterDSPPlugin(IMJDSPNotification * pNotification, int nZone)
{
	// initialize
	m_paryStutter = NULL;
	m_nStutterPosition = 0;
	m_nStutterBlocks = 0;
	m_nStutterDuration = 100;
	m_pNotification = pNotification;
	m_pwndDisplay = NULL;
}

CMJStutterDSPPlugin::~CMJStutterDSPPlugin()
{
	// cleanup
	if (m_paryStutter)
	{
		delete [] m_paryStutter;
		m_paryStutter = NULL;
	}

	if (m_pwndDisplay)
	{
		if (::IsWindow(m_pwndDisplay->m_hWnd))
			m_pwndDisplay->DestroyWindow();

		delete m_pwndDisplay;
		m_pwndDisplay = NULL;
	}
}

int CMJStutterDSPPlugin::Flush()
{
	// lock access (so notifications from our dialog won't cause problems)
	CSingleLock(&m_CriticalSection, TRUE);

	// reset so that our internal buffer is empty
	m_nStutterPosition = 0;

	return 0;
}

int CMJStutterDSPPlugin::SetAudioFormat(const WAVEFORMATEX * pwfeInput)
{
	// lock access (so notifications from our dialog won't cause problems)
	CSingleLock(&m_CriticalSection, TRUE);

	// figure out how many "blocks" correlate to the desired milliseconds
	m_nStutterBlocks = (pwfeInput->nSamplesPerSec * m_nStutterDuration) / 1000;
	
	// erase the existing buffer and make a new one
	if (m_paryStutter)
	{
		delete [] m_paryStutter;
		m_paryStutter = NULL;
	}
	m_paryStutter = new float [m_nStutterBlocks * pwfeInput->nChannels];

	// reset
	Flush();

	// store the format information
	if (&m_wfeInput != pwfeInput)
		memcpy(&m_wfeInput, pwfeInput, sizeof(WAVEFORMATEX));

	// successful (we work with any format)
	return 0;
}

int CMJStutterDSPPlugin::Process(float * pBuffer, int nBlocks, IMJDSPOutput * pOutput)
{
	// lock access (so notifications from our dialog won't cause problems)
	CSingleLock(&m_CriticalSection, TRUE);

	// stuttering
	int nBlocksLeft = nBlocks;
	int nBufferPosition = 0;
	
	while (nBlocksLeft > 0)
	{
		// add the data to our internal buffer
		int nBlocksNeededToFillOutput = m_nStutterBlocks - m_nStutterPosition;
		int nBlocksThisPass = min(nBlocksLeft, nBlocksNeededToFillOutput);
		memcpy(&m_paryStutter[m_nStutterPosition * m_wfeInput.nChannels], &pBuffer[nBufferPosition * m_wfeInput.nChannels], nBlocksThisPass * m_wfeInput.nChannels * sizeof(float));
		m_nStutterPosition += nBlocksThisPass;
		nBlocksLeft -= nBlocksThisPass;
		nBufferPosition += nBlocksThisPass;

		// if the internal buffer is full, output it (with stuttering)
		if (m_nStutterPosition == m_nStutterBlocks)
		{
			for (int z = 0; z < STUTTER_COUNT; z++)
				pOutput->Output(m_paryStutter, m_nStutterBlocks);
			m_nStutterPosition = 0;
		}
	}

	return 0;
}

int	CMJStutterDSPPlugin::GetInformation(MJ_DSP_INFO_FIELDS nInfoID, LPTSTR pResult)
{
	// lock access (so notifications from our dialog won't cause problems)
	CSingleLock(&m_CriticalSection, TRUE);

	if (nInfoID == MJ_DSP_INFO_FIELD_PLUGIN_DESCRIPTION)
	{
		_tcscpy(pResult, _T("Stutter"));
		return 0;
	}

	return -1;
}

int	CMJStutterDSPPlugin::SetInformation(MJ_DSP_INFO_FIELDS nInfoID, LPCTSTR pValue)
{
	// lock access (so notifications from our dialog won't cause problems)
	CSingleLock(&m_CriticalSection, TRUE);

	return -1;
}

int	CMJStutterDSPPlugin::DoCommand(MJ_DSP_COMMANDS nCommandID, int nParam1, int nParam2)
{
	// lock access (so notifications from our dialog won't cause problems)
	CSingleLock(&m_CriticalSection, TRUE);

	int nRetVal = -1;

	switch (nCommandID)
	{
		case MJ_DSP_COMMAND_GET_DISPLAY_WINDOW:
		{	
			if (m_pwndDisplay == NULL)
				m_pwndDisplay = new CStutterDlg(this, CWnd::FromHandle((HWND) nParam1));

			if (::IsWindow(m_pwndDisplay->m_hWnd) == FALSE)
				m_pwndDisplay->Create(IDD_STUTTER, CWnd::FromHandle((HWND) nParam1));

			SIZE * pIdealSize = (SIZE *) nParam2;
			
			CRect rectDisplay;
			m_pwndDisplay->GetWindowRect(&rectDisplay);
			
			pIdealSize->cx = rectDisplay.Width();
			pIdealSize->cy = rectDisplay.Height();

			nRetVal = (int) m_pwndDisplay->m_hWnd;
			break;
		}
		case MJ_DSP_COMMAND_RELEASE_DISPLAY_WINDOW:
		{
			::DestroyWindow((HWND) nParam1);
			nRetVal = 0;
			break;
		}
	}
	
	return nRetVal;
}

int	CMJStutterDSPPlugin::SetStutterDuration(int nMilliseconds)
{
	// lock access (so notifications from our dialog won't cause problems)
	CSingleLock(&m_CriticalSection, TRUE);

	// update our duration
	m_nStutterDuration = nMilliseconds;

	// reset the format so we recreate our buffer
	SetAudioFormat(&m_wfeInput);

	// let MJ know that the settings changed
	m_pNotification->SettingsChanged();

	return 0;
}

int CMJStutterDSPPlugin::GetStutterDuration()
{
	// lock access (so notifications from our dialog won't cause problems)
	CSingleLock(&m_CriticalSection, TRUE);

	return m_nStutterDuration;
}
