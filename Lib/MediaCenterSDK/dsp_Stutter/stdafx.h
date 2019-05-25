// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently

#pragma once

// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

#define VC_EXTRALEAN		// Exclude rarely-used stuff from Windows headers

#pragma warning(disable: 4996)

#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions
#include <atlbase.h>


#include <afxdtctl.h>		// MFC support for Internet Explorer 4 Common Controls
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>			// MFC support for Windows Common Controls
#endif // _AFX_NO_AFXCMN_SUPPORT

#include <afxmt.h>

#define REGISTRY_PATH_PLUGIN	      _T("Software\\J. River\\Media Jukebox\\Plugins\\DSP\\Stutter")
