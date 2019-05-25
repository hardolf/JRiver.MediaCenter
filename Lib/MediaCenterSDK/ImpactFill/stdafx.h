#pragma once

// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.


#define STRICT
#ifndef _WIN32_WINNT
#define _WIN32_WINNT 0x0400
#endif
#define _ATL_APARTMENT_THREADED

#include <atlbase.h>
//You may derive a class from CComModule and use it if you want to override
//something, but do not change the name of _Module
extern CComModule _Module;
#include <atlcom.h>
#include <atlctl.h>
#include <comdef.h>

#import "..\Shared\MCPlayerLib.tlb"  no_namespace, named_guids

#define REGISTRY_PATH_MJ_PLUGINS_IMPACTFILL _T("Software\\J. River\\Media Center 12\\Plugins\\Display\\ImpactFill")

#define THIRD_PARTY_PLUGIN
