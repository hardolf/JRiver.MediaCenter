// stdafx.h : include file for standard system include files,
//      or project specific include files that are used frequently,
//      but are changed infrequently
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

#if !defined(AFX_STDAFX_H__8F445627_5303_4C01_939D_4DA3AB0D6E27__INCLUDED_)
#define AFX_STDAFX_H__8F445627_5303_4C01_939D_4DA3AB0D6E27__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#define STRICT
#ifndef _WIN32_WINNT
#define _WIN32_WINNT 0x0400
#endif
#define _ATL_APARTMENT_THREADED

#include <afxwin.h>
#include <afxdisp.h>

#include <atlbase.h>
//You may derive a class from CComModule and use it if you want to override
//something, but do not change the name of _Module
extern CComModule _Module;
#include <atlcom.h>
#include <atlhost.h>
#include <atlctl.h>

#import "..\Shared\MCPlayerLib.tlb"  no_namespace, named_guids

#define REGISTRY_PATH_MJ_PLUGINS_INTERFACE	"Software\\J. River\\Media Center 12\\Plugins\\Interface\\"

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__8F445627_5303_4C01_939D_4DA3AB0D6E27__INCLUDED)
