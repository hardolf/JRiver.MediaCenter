// SleepTimerCtrl.cpp : Implementation of CSleepTimerCtrl
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

#include "stdafx.h"
#include "SleepTimer.h"
#include "SleepTimerCtrl.h"

/////////////////////////////////////////////////////////////////////////////
// CSleepTimerCtrl
#define IDT_UPDATE				1
#define EWX_FORCEIFHUNG			0x00000010

CSleepTimerCtrl::CSleepTimerCtrl()
{
	m_bWindowOnly = TRUE;
	CalcExtent(m_sizeExtent);

	m_nSleepSeconds = 3600;
	m_bSleepActive = FALSE;
}


LRESULT CSleepTimerCtrl::OnClickedSleep(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
	ToggleSleep();
	return 0;
}

void CSleepTimerCtrl::ToggleSleep()
{
	KillTimer(IDT_UPDATE);

	if (m_bSleepActive)
	{
		m_bSleepActive = FALSE;

		m_ctrlCountdown.DisplayTime(0);
		::EnableWindow(GetDlgItem(IDC_FADE_VOLUME), TRUE);
		::EnableWindow(GetDlgItem(IDC_DURATION), TRUE);
		::EnableWindow(GetDlgItem(IDC_ACTION), TRUE);
		::SetWindowText(GetDlgItem(IDC_SLEEP), "Sleep");
	}
	else
	{
		m_bSleepActive = TRUE;
		
		::EnableWindow(GetDlgItem(IDC_FADE_VOLUME), FALSE);
		::EnableWindow(GetDlgItem(IDC_DURATION), FALSE);
		::EnableWindow(GetDlgItem(IDC_ACTION), FALSE);
		::SetWindowText(GetDlgItem(IDC_SLEEP), "Abort");

		m_dwStartTickCount = GetTickCount();
		char cDuration[256]; cDuration[0] = 0;
		::GetWindowText(GetDlgItem(IDC_DURATION), cDuration, 256);
		m_nSleepSeconds = (atoi(cDuration) == 0) ? 3600 : atoi(cDuration) * 60;
		
		m_nActionType = ::SendMessage(GetDlgItem(IDC_ACTION), CB_GETCURSEL, 0, 0);

		m_fStartVolume = 1.0;
		m_bFadeVolume = ::SendMessage(GetDlgItem(IDC_FADE_VOLUME), BM_GETCHECK, 0, 0);
		if (m_bFadeVolume)
		{
			IMJMixerAutomationPtr pMixer = m_pMJ->GetMJMixer();
			m_fStartVolume = float(pMixer->Volume) / 100;
		}

		SetTimer(IDT_UPDATE, 50);
	}
}

LRESULT CSleepTimerCtrl::OnTimer(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	KillTimer(IDT_UPDATE);

	int nElapsedSeconds = (GetTickCount() - m_dwStartTickCount) / 1000;
	int nRemainingSeconds = m_nSleepSeconds - nElapsedSeconds;

	float fPercentLeft = float(nRemainingSeconds) / float(m_nSleepSeconds);
	if (m_bFadeVolume)
	{
		IMJMixerAutomationPtr pMixer = m_pMJ->GetMJMixer();
		pMixer->Volume = int((m_fStartVolume * fPercentLeft) * float(100));
	}

	m_ctrlCountdown.DisplayTime(nRemainingSeconds);

	if (nRemainingSeconds <= 0)
	{
		// stop playback
		IMJPlaybackAutomationPtr pPlayback = m_pMJ->GetPlayback();
		pPlayback->Stop();

		// restore the volume
		if (m_bFadeVolume)
		{
			IMJMixerAutomationPtr pMixer = m_pMJ->GetMJMixer();
			pMixer->Volume = int(m_fStartVolume * float(100));
		}

		// we're done...
		if (m_nActionType == 0)
		{
			// nothing else to do
		}
		else if (m_nActionType == 1)
		{
			// close Media Jukebox
			HWND hwndMJ = FindWindow("MJFrame", NULL);
			::PostMessage(hwndMJ, WM_CLOSE, 0, 0);
		}
		else if (m_nActionType == 2)
		{
			// shutdown windows
			HANDLE hToken; TOKEN_PRIVILEGES tkp;
            OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken);
            LookupPrivilegeValue(NULL, SE_SHUTDOWN_NAME, &tkp.Privileges[0].Luid);
            tkp.PrivilegeCount = 1;
            tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
            AdjustTokenPrivileges(hToken, FALSE, &tkp, 0, NULL, 0);
            ExitWindowsEx(EWX_POWEROFF | EWX_FORCEIFHUNG, 0);
		}

		ToggleSleep();

		return TRUE;
	}

	SetTimer(IDT_UPDATE, 1000);

	return TRUE;
}

LRESULT CSleepTimerCtrl::OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	long nWidth = LOWORD(lParam);
	long nHeight = HIWORD(lParam);

	if (::IsWindow(m_hWnd) && ::IsWindow(m_ctrlCountdown.m_hWnd))
	{
		CRect rectBottom; ::GetWindowRect(GetDlgItem(IDC_SLEEP), &rectBottom); ScreenToClient(&rectBottom);
		m_ctrlCountdown.SetWindowPos(NULL, 7, rectBottom.bottom + 7, nWidth - 14, nHeight - rectBottom.bottom - 14, SWP_NOZORDER);
	}

	return 0;
}

STDMETHODIMP CSleepTimerCtrl::Init(LPDISPATCH pDisp)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	m_pMJ.Attach((IMJAutomation *) pDisp, TRUE);
	
	m_pMJ->EnableSkinning(FALSE);
	m_ctrlCountdown.Create("", WS_VISIBLE | WS_CHILD | SS_CENTER, CRect(0, 0, 600, 100), CWnd::FromHandle(m_hWnd));
	m_pMJ->EnableSkinning(TRUE);

	m_ctrlCountdown.DisplayTime(0);

	::SendMessage(GetDlgItem(IDC_ACTION), CB_ADDSTRING, 0, (LPARAM) "Stop Playback");
	::SendMessage(GetDlgItem(IDC_ACTION), CB_ADDSTRING, 0, (LPARAM) "Close Media Jukebox");
	::SendMessage(GetDlgItem(IDC_ACTION), CB_ADDSTRING, 0, (LPARAM) "Shutdown Windows");
	::SendMessage(GetDlgItem(IDC_ACTION), CB_SETCURSEL, 0, 0);
	::SetWindowText(GetDlgItem(IDC_DURATION), "60");

	return S_OK;
}
