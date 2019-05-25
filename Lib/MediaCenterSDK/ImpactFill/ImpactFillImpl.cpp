// ImpactFillImpl.cpp : Implementation of CImpactFillImpl
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.


#include "stdafx.h"
#include <stdio.h>
#include "ImpactFill.h"
#include "ImpactFillImpl.h"
#include "VisPlugin.h"

#define SAFE_DELETE(POINTER) if(POINTER) { delete POINTER; POINTER = NULL; }
#define GET_X_LPARAM(lp)                        ((int)(short)LOWORD(lp))
#define GET_Y_LPARAM(lp)                        ((int)(short)HIWORD(lp))
#define MENU_ENABLED(EXPRESSION) ((EXPRESSION) ? MF_ENABLED : MF_GRAYED)
#define MENU_CHECKED(EXPRESSION) ((EXPRESSION) ? MF_CHECKED : MF_UNCHECKED)

CImpactFillImpl::CImpactFillImpl()
{
	m_bWindowOnly = TRUE;
	m_pVisRedrawHelper = NULL;
	m_pVisData = NULL;
	m_nPreset = 0;
	m_rgbFillColor = GetFillColor();
	m_bShowFramerate = TRUE;
}

CImpactFillImpl::~CImpactFillImpl()
{
	
}

LRESULT CImpactFillImpl::OnEraseBkgnd(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	return TRUE;
}

unsigned int CImpactFillImpl::GetFillColor()
{
	unsigned int nFillColor = 0;

	if (m_nPreset == 0)
		nFillColor = RGB(rand() % 256, rand() % 256, rand() % 256); // rainbow style
	else if (m_nPreset == 1)
		nFillColor = RGB(rand() % 256, 0, 0); // red-style
	else if (m_nPreset == 2)
		nFillColor = RGB(0, rand() % 256, 0); // green-style
	else if (m_nPreset == 3)
		nFillColor = RGB(0, 0, rand() % 256); // blue-style

	return nFillColor;
}

HRESULT CImpactFillImpl::OnDrawAdvanced(ATL_DRAWINFO& di)
{
	// get the data (call this once each draw to get new data and have it analyzed)
	m_pVisData->GetData();

	// change the fill color on impact
	if (m_pVisData->GetVisInfo(VIS_DATA_IMPACT) > 100)
		m_rgbFillColor = GetFillColor();

	// fill the background
	HBRUSH hBrush = CreateSolidBrush(m_rgbFillColor);
	RECT rectWindow; GetClientRect(&rectWindow);
	FillRect(di.hdcDraw, &rectWindow, hBrush);
	DeleteObject(hBrush);

	// output some text in the middle
	SetTextAlign(di.hdcDraw, TA_CENTER | TA_BASELINE);
	SetBkMode(di.hdcDraw, TRANSPARENT);
	
	TCHAR cBuffer[1024];
	if (m_bShowFramerate)
		_stprintf(cBuffer, _T("Media Center Display Plugin Sample (fps: %.1f)"), float(m_pVisData->GetVisInfo(VIS_DATA_FPS)) / float(1000));
	else
		_stprintf(cBuffer, _T("Media Center Display Plugin Sample"));

	RECT & rc = *((RECT *) di.prcBounds);
	TextOut(di.hdcDraw, (rc.left + rc.right) / 2, (rc.top + rc.bottom) / 2, cBuffer, lstrlen(cBuffer));

	return S_OK;
}

LRESULT CImpactFillImpl::OnControlBarCommand(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	// this function adds menu commands to the display menu (right-click / Player > Display Options)
	// it also builds the list of items used for preset / style selection in the menu, on screen display, etc.

	// this toggles what type of preset / style system the plugin will use
	BOOL bHasNamedPresets = TRUE;

	if ((wParam == CB_FILL_MENU_MODES) && (lParam != NULL))
	{
		// fill the menu with our modes
		IMenuEx * pMenu = (IMenuEx *) lParam;

		if (bHasNamedPresets)
		{
			// add three choices for color presets
			// we set the MF_ADDTOSWITCHBAR flag so that the bar below the visualizations
			// will allow selection of the preset
			// the text of the checked MF_ADDTOSWITCHBAR item will be 
			// displayed as the current item
			pMenu->Add(DISPLAY_MENU_ID_MIN + 100 + 0, _T("Rainbow Style"), MF_ADDTOSWITCHBAR | MENU_CHECKED(m_nPreset == 0));
			pMenu->Add(DISPLAY_MENU_ID_MIN + 100 + 1, _T("Red Style"), MF_ADDTOSWITCHBAR | MENU_CHECKED(m_nPreset == 1));
			pMenu->Add(DISPLAY_MENU_ID_MIN + 100 + 2, _T("Green Style"), MF_ADDTOSWITCHBAR | MENU_CHECKED(m_nPreset == 2));
			pMenu->Add(DISPLAY_MENU_ID_MIN + 100 + 3, _T("Blue Style"), MF_ADDTOSWITCHBAR | MENU_CHECKED(m_nPreset == 3));
		}
		else
		{
			// there are no named presets, but we still allow previous / next
			pMenu->Add(DISPLAY_MENU_ID_MIN + 100 + 0, _T("Previous"), MF_ADDTOSWITCHBAR);
			pMenu->Add(DISPLAY_MENU_ID_MIN + 100 + 1, _T("Next"), MF_ADDTOSWITCHBAR);
		}
	}
	else if ((wParam == CB_FILL_MENU) && (lParam != NULL))
	{
		// fill the menu
		IMenuEx * pMenu = (IMenuEx *) lParam;
		
		// add a separator (not really needed here, but as an example)
		pMenu->AddSeparator();

		// add a framerate menu
		int nFramerate = 30;
		IMenuEx * pFramerateMenu = pMenu->AddSubMenu(_T("Show Framerate"));
		pFramerateMenu->Add(DISPLAY_MENU_ID_MIN + 200 + 0, _T("Show Framerate"), MENU_CHECKED(m_bShowFramerate));
		pFramerateMenu->Add(DISPLAY_MENU_ID_MIN + 200 + 1, _T("Don't Show Framerate"), MENU_CHECKED(!m_bShowFramerate));

		return TRUE;
	}
	else if (wParam == CB_PROCESS_MENU_COMMAND)
	{
		// process the menu return
		// note that we can also treat the return as a MENU_INFO_EX
		// but this requires that we are using MFC
		MENU_INFO * pInfo = (MENU_INFO *) lParam;

		BOOL bHandled = TRUE;
		if ((pInfo->nID >= (DISPLAY_MENU_ID_MIN + 100)) && (pInfo->nID < (DISPLAY_MENU_ID_MIN + 200)))
		{
			// toggle the preset
			if (bHasNamedPresets)
			{
				// explicitly set the preset
				m_nPreset = pInfo->nID - (DISPLAY_MENU_ID_MIN + 100);
			}
			else
			{
				if (pInfo->nID == (DISPLAY_MENU_ID_MIN + 100 + 0))
				{
					// previous preset
					m_nPreset -= 1;
					if (m_nPreset < 0)
						m_nPreset = 3;
				}
				else if (pInfo->nID == (DISPLAY_MENU_ID_MIN + 100 + 1))
				{
					// next preset
					m_nPreset = (m_nPreset + 1) % 4;
				}
			}

			// update the fill color
			m_rgbFillColor = GetFillColor();
		}
		else if (pInfo->nID == (DISPLAY_MENU_ID_MIN + 200 + 0))
		{
			m_bShowFramerate = TRUE;
		}
		else if (pInfo->nID == (DISPLAY_MENU_ID_MIN + 200 + 1))
		{
			m_bShowFramerate = FALSE;
		}
		else
		{
			bHandled = FALSE;
		}

		if (bHandled)
			return TRUE;
	}

	return FALSE;
}

STDMETHODIMP CImpactFillImpl::Init(IDispatch* pMJ)
{
	m_pMJ.Attach((IMJAutomation *) pMJ, TRUE);

	TCHAR CurrentZone[64];
	wsprintf(CurrentZone, _T("%d"), m_pMJ->GetZones()->GetActiveZone());
	m_pVisData = (IVisData *) m_pMJ->CreateInterface(_T("IVisData"), CurrentZone);
	m_pVisRedrawHelper = (IVisRedrawHelper *) m_pMJ->CreateInterface(_T("IVisRedrawHelper"), _T(""));
	m_pVisRedrawHelper->Start(m_hWnd);

	return S_OK;
}

LRESULT CImpactFillImpl::OnDestroy(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	SAFE_DELETE(m_pVisRedrawHelper);
	SAFE_DELETE(m_pVisData);
	return FALSE;
}

STDMETHODIMP CImpactFillImpl::Terminate()
{
	SAFE_DELETE(m_pVisRedrawHelper);
	SAFE_DELETE(m_pVisData);
	return S_OK;
}
