// dsp_Stutter.cpp : Defines the initialization routines for the DLL.
//
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

#include "stdafx.h"
#include "dsp_Stutter.h"

#include "MJStutterDSPPlugin.h"

/////////////////////////////////////////////////////////////////////////////
// CDsp_StutterApp

BEGIN_MESSAGE_MAP(CDsp_StutterApp, CWinApp)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDsp_StutterApp construction

CDsp_StutterApp::CDsp_StutterApp()
{
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CDsp_StutterApp object

CDsp_StutterApp theApp;

STDAPI DllRegisterServer()
{
	// get the full path to the app
	TCHAR szPath[_MAX_PATH + 1];
	GetModuleFileName(AfxGetInstanceHandle(), szPath, MAX_PATH);

	CRegKey reg;
	if (ERROR_SUCCESS == reg.Create(HKEY_LOCAL_MACHINE, REGISTRY_PATH_PLUGIN))
	{		
		// general plugin information
		reg.SetValue(1, _T("IVersion"));
		reg.SetValue(_T("J. River, Inc."), _T("Company"));
		reg.SetValue(szPath, _T("Path"));
		reg.SetValue(_T("2.0.2"), _T("Version"));
		reg.SetValue(_T("www.jrmediacenter.com"), _T("URL"));
		reg.SetValue(_T("Copyright (c) 2002-2008, J. River, Inc."), _T("Copyright"));
		reg.SetValue(_T("DSP SDK Sample: Stutter"), _T("Description"));
	}

	return TRUE;
}

extern "C" __declspec(dllexport) IMJDSPPlugin * GetDSPPlugin(int nIndex, IMJDSPNotification * pNotification, int nZone)
{
	if (nIndex == 0)
		return new CMJStutterDSPPlugin(pNotification, nZone);
	else
		return NULL;
}
