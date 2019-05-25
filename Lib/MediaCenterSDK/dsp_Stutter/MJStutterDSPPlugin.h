// MJStutterDSPPlugin.h: interface for the CMJStutterDSPPlugin class.
//
//////////////////////////////////////////////////////////////////////

// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

#pragma once

#include "MJDSPPlugin.h"
class CStutterDlg;

class CMJStutterDSPPlugin : public IMJDSPPlugin  
{
public:

	CMJStutterDSPPlugin(IMJDSPNotification * pNotification, int nZone);
	virtual ~CMJStutterDSPPlugin();

	int Flush();
	int SetAudioFormat(const WAVEFORMATEX * pwfeInput);
	int Process(float * pBuffer, int nBlocks, IMJDSPOutput * pOutput);

	int	GetInformation(MJ_DSP_INFO_FIELDS nInfoID, LPTSTR pResult);
	int	SetInformation(MJ_DSP_INFO_FIELDS nInfoID, LPCTSTR pValue);
	int	DoCommand(MJ_DSP_COMMANDS nCommandID, int nParam1 = 0, int nParam2 = 0);

	IMJDSPNotification * m_pNotification;

	int SetStutterDuration(int nMilliseconds);
	int GetStutterDuration();

protected:
	
	CCriticalSection m_CriticalSection;
	WAVEFORMATEX m_wfeInput;
	float * m_paryStutter;
	int m_nStutterPosition;
	int m_nStutterBlocks;
	int m_nStutterDuration;
	CStutterDlg * m_pwndDisplay;
};
