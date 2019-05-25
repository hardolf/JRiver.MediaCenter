// MJRAWInputSource.h: interface for the CMJRAWInputSource class.
//
//////////////////////////////////////////////////////////////////////
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

#pragma once

#include "MJInputSource.h"

class CMJRAWInputSource : public CMJInputSource  
{
public:
	CMJRAWInputSource();
	virtual ~CMJRAWInputSource();

	int Open(LPCTSTR pFilename);
	int AddDataToBuffer();
	
	int GetInformation(MJ_INPUT_SOURCE_INFO_FIELDS nInfoID, LPTSTR pResult);
	int SetInformation(MJ_INPUT_SOURCE_INFO_FIELDS nInfoID, LPCTSTR pValue);
	int DoCommand(MJ_INPUT_SOURCE_COMMANDS nCommandID, int nParam1 = 0, int nParam2 = 0);
	
private:
	long InitializeVariables();
	long Close();
};
