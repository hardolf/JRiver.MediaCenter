// ImpactFill.cpp : Implementation of DLL Exports.
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.


// Note: Proxy/Stub Information
//      To build a separate proxy/stub DLL, 
//      run nmake -f ImpactFillps.mk in the project directory.

#include "stdafx.h"
#include "resource.h"
#include <initguid.h>
#include "ImpactFill.h"

#include "ImpactFill_i.c"
#include "ImpactFillImpl.h"


CComModule _Module;

BEGIN_OBJECT_MAP(ObjectMap)
OBJECT_ENTRY(CLSID_ImpactFill, CImpactFillImpl)
END_OBJECT_MAP()

/////////////////////////////////////////////////////////////////////////////
// DLL Entry Point

extern "C"
BOOL WINAPI DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID /*lpReserved*/)
{
    if (dwReason == DLL_PROCESS_ATTACH)
    {
        _Module.Init(ObjectMap, hInstance, &LIBID_IMPACTFILLLib);
        DisableThreadLibraryCalls(hInstance);
    }
    else if (dwReason == DLL_PROCESS_DETACH)
        _Module.Term();
    return TRUE;    // ok
}

/////////////////////////////////////////////////////////////////////////////
// Used to determine whether the DLL can be unloaded by OLE

STDAPI DllCanUnloadNow(void)
{
    return (_Module.GetLockCount()==0) ? S_OK : S_FALSE;
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
	CRegKey reg;
	if(ERROR_SUCCESS == reg.Create(HKEY_LOCAL_MACHINE, REGISTRY_PATH_MJ_PLUGINS_IMPACTFILL))
	{		
		// general plugin information
		DWORD nIVersion		   = 2; // SUPPORTED VERSION OF VISUALIZATION PLUG-IN SDK
		TCHAR  strCompany[128] = {_T("J. River, Inc.")}; // COMPANY NAME
		TCHAR  strVersion[10]  = {_T("1.0.4")}; // VERSION OF THE PLUG-IN
		TCHAR  strURL[128]     = {_T("www.jrmediacenter.com")}; // ADDRESS OF THE WEB PAGE FOR MORE INFO ON THE PLUG-IN
		TCHAR  strCopyright[255] = {_T("Copyright (c) 2002-2008, J. River, Inc.")}; // COPYRIGHT INFORMATION
		LPWSTR strID; StringFromCLSID(CLSID_ImpactFill, &strID); // CLASS ID OF THE COM CONTROL
		
		reg.SetValue(_bstr_t(strID), _T("CLSID")); 		
		reg.SetValue(nIVersion, _T("IVersion"));
		reg.SetValue(strCompany, _T("Company"));
		reg.SetValue(strVersion, _T("Version"));
		reg.SetValue(strURL, _T("URL"));
		reg.SetValue(strCopyright, _T("Copyright"));
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

