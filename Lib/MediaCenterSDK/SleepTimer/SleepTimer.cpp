// SleepTimer.cpp : Implementation of DLL Exports.
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.


// Note: Proxy/Stub Information
//      To build a separate proxy/stub DLL, 
//      run nmake -f SleepTimerps.mk in the project directory.

#include "stdafx.h"
#include "resource.h"
#include <initguid.h>
#include "SleepTimer.h"

#include "SleepTimer_i.c"
#include "SleepTimerCtrl.h"


CComModule _Module;

BEGIN_OBJECT_MAP(ObjectMap)
OBJECT_ENTRY(CLSID_SleepTimerCtrl, CSleepTimerCtrl)
END_OBJECT_MAP()

class CSleepTimerApp : public CWinApp
{
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSleepTimerApp)
	public:
    virtual BOOL InitInstance();
    virtual int ExitInstance();
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CSleepTimerApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

BEGIN_MESSAGE_MAP(CSleepTimerApp, CWinApp)
	//{{AFX_MSG_MAP(CSleepTimerApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

CSleepTimerApp theApp;

BOOL CSleepTimerApp::InitInstance()
{
    _Module.Init(ObjectMap, m_hInstance, &LIBID_SLEEPTIMERLib);
    return CWinApp::InitInstance();
}

int CSleepTimerApp::ExitInstance()
{
    _Module.Term();
    return CWinApp::ExitInstance();
}

/////////////////////////////////////////////////////////////////////////////
// Used to determine whether the DLL can be unloaded by OLE

STDAPI DllCanUnloadNow(void)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());
    return (AfxDllCanUnloadNow()==S_OK && _Module.GetLockCount()==0) ? S_OK : S_FALSE;
}

/////////////////////////////////////////////////////////////////////////////
// Returns a class factory to create an object of the requested type

STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
    return _Module.GetClassObject(rclsid, riid, ppv);
}

/////////////////////////////////////////////////////////////////////////////
// DllRegisterServer - Adds entries to the system registry

STDAPI DllRegisterServer(void)
{
 	// register with Media Jukebox
	char cRegistryPath[1024];
	strcpy (cRegistryPath, REGISTRY_PATH_MJ_PLUGINS_INTERFACE);
	strcat(cRegistryPath, "Sleep Timer");
	
	CRegKey reg;
	if (ERROR_SUCCESS == reg.Create(HKEY_LOCAL_MACHINE, cRegistryPath))
	{		
		// general plugin information
		DWORD nIVersion       = 1;
		DWORD nPluginMode	  = 1;
		char  strCompany[128] = {"J. River, Inc."};
		char  strVersion[10]  = {"1.0.6"};
		char  strURL[128]     = {"http://www.jrmediacenter.com"};
		char  strCopyright[255] = {"Copyright (c) 2002-2008, J. River, Inc."};
		LPWSTR strID; StringFromCLSID(CLSID_SleepTimerCtrl, &strID);
		
		reg.SetValue(_bstr_t(strID), "CLSID");
		reg.SetValue(nIVersion, "IVersion");
		reg.SetValue(strCompany, "Company");
		reg.SetValue(strVersion, "Version");
		reg.SetValue(strURL, "URL");
		reg.SetValue(strCopyright, "Copyright");
		reg.SetValue(nPluginMode, "PluginMode");
	}	
		
	// registers object, typelib and all interfaces in typelib
    return _Module.RegisterServer(TRUE);
}

/////////////////////////////////////////////////////////////////////////////
// DllUnregisterServer - Removes entries from the system registry

STDAPI DllUnregisterServer(void)
{
    return _Module.UnregisterServer(TRUE);
}

