// MyEncoder.cpp : Implementation of DLL Exports.

// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

// Note: Proxy/Stub Information
//      To build a separate proxy/stub DLL, 
//      run nmake -f MyEncoderps.mk in the project directory.

#include "stdafx.h"
#include "resource.h"
#include <initguid.h>
#include "MyEncoder.h"

#include "MyEncoder_i.c"
#include "MyImpl.h"


CComModule _Module;

BEGIN_OBJECT_MAP(ObjectMap)
OBJECT_ENTRY(CLSID_MyImpl, CMyImpl)
END_OBJECT_MAP()

class CMyEncoderApp : public CWinApp
{
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMyEncoderApp)
	public:
    virtual BOOL InitInstance();
    virtual int ExitInstance();
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CMyEncoderApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

BEGIN_MESSAGE_MAP(CMyEncoderApp, CWinApp)
	//{{AFX_MSG_MAP(CMyEncoderApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

CMyEncoderApp theApp;

BOOL CMyEncoderApp::InitInstance()
{
    _Module.Init(ObjectMap, m_hInstance, &LIBID_MYENCODERLib);
    return CWinApp::InitInstance();
}

int CMyEncoderApp::ExitInstance()
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
	CString strRegistryPath = REGISTRY_PATH_MJ_PLUGINS_ENCODERS;
	strRegistryPath += _T("Wave Reversal");
	
	CRegKey reg;
	if(ERROR_SUCCESS == reg.Create(HKEY_LOCAL_MACHINE, (LPCTSTR)strRegistryPath))
	{		
		// general plugin information
		DWORD nIVersion       = 3;  // must be 3!!!
		CString  strCompany = _T("My Company");
		CString  strVersion  = _T("1.0.0");
		CString  strURL;
		CString  strCopyright = _T("Copyright (c) 2008, My Company Inc.");

		LPWSTR strID; 
		StringFromCLSID(CLSID_MyImpl, &strID);
		reg.SetValue(_bstr_t(strID), _T("CLSID"));
		
		StringFromCLSID(IID_IMyImpl, &strID);
		reg.SetValue(_bstr_t(strID), _T("IID"));

		StringFromCLSID(IID__IMyBufferInputImpl, &strID);
		reg.SetValue(_bstr_t(strID), _T("BufferInputIID"));
		
		StringFromCLSID(IID__IMyImplEvents, &strID);
		reg.SetValue(_bstr_t(strID), _T("EIID"));
		
		reg.SetValue(nIVersion, _T("IVersion"));
		reg.SetValue(strCompany, _T("Company"));
		reg.SetValue(strVersion, _T("Version"));
		reg.SetValue(strURL, _T("URL"));
		reg.SetValue(strCopyright, _T("Copyright"));

		CString strExt = MY_EXTENSION;
		DWORD nIsConfigurable	 = ISCONFIGURABLE_NO;
		
		reg.SetValue(strExt, _T("Ext"));
		reg.SetValue(nIsConfigurable, _T("Configurable"));

		// encoder specific options
		reg.SetValue(_T("Backwards"), _T("SelectedQuality"));
		reg.SetValue(_T("Backwards"), _T("Qualities"));
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

