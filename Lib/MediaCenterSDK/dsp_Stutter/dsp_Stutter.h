// dsp_Stutter.h : main header file for the DSP_STUTTER DLL
//
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

#pragma once

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CDsp_StutterApp
// See dsp_Stutter.cpp for the implementation of this class
//

class CDsp_StutterApp : public CWinApp
{
public:
	CDsp_StutterApp();

// Overrides
	public:

	DECLARE_MESSAGE_MAP()
};
