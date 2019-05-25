/************************************************************************************
Media Jukebox DSP Plugin Architecture
Copyright (c) 2001-2008 J. River, Inc.

Note(s):
	- all data represented by 0.24 floats (standard WAVE_FORMAT_IEEE_FLOAT representation)
	- must implement IMJDSPPlugin * GetDSPPlugin(int nIndex) as the main export to get the plugin
************************************************************************************/

#pragma once

/************************************************************************************
DSP plugins MUST be built in Unicode
************************************************************************************/
#if (!defined(UNICODE) || !defined(_UNICODE))
#error "DSP plugins must be built in Unicode mode!"
#endif

/************************************************************************************
Includes
************************************************************************************/
#include <mmreg.h>

/************************************************************************************
IMJDSPOutput
************************************************************************************/
class IMJDSPOutput
{
public:

	virtual int Output(float * pBuffer, int nBlocks) = 0;
};

/************************************************************************************
IMJDSPNotification
************************************************************************************/
class IMJDSPNotification
{
public:
	
	virtual int SettingsChanged(BOOL bTurnOn = TRUE) = 0;
	virtual int SizeChanged(SIZE sizeIdeal) = 0;
};

/************************************************************************************
Defines and enumerations
************************************************************************************/
enum MJ_DSP_INFO_FIELDS
{
	MJ_DSP_INFO_FIELD_PLUGIN_DESCRIPTION,			// text description (i.e. "Effects")
	MJ_DSP_INFO_FIELD_AUTHORIZATION,				// authorization code (use MJ_DSP_PLUGIN_AUTHORIZATION_UNSAFE or contact J. River to get an authorization code)
};

enum MJ_DSP_COMMANDS
{
	MJ_DSP_COMMAND_SHOW_ABOUT,						// [HWND parent, ignore] -> ignored
	MJ_DSP_COMMAND_GET_DISPLAY_WINDOW,				// [HWND parent, SIZE * pIdealSize] -> HWND to window
	MJ_DSP_COMMAND_RELEASE_DISPLAY_WINDOW,			// [HWND display, ignore] -> ignored
	MJ_DSP_COMMAND_SET_ENABLED,						// [BOOL bEnabled, ignore] -> ignored
	MJ_DSP_COMMAND_UPDATE_SETTINGS,					// [ignore, ignore] -> ignored
	MJ_DSP_COMMAND_GET_CAN_NOT_CHANGE_LENGTH,		// [ignore, ignore] -> 1 means you will *never* change the length / any other value means you *could* change the length
};

#ifndef MJ_DSP_PLUGIN_AUTHORIZATION_UNSAFE_DEFINED
	#define MJ_DSP_PLUGIN_AUTHORIZATION_UNSAFE_DEFINED
	static const GUID MJ_DSP_PLUGIN_AUTHORIZATION_UNSAFE = { 0x00000000, 0x0000, 0x0000, { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } };
#endif

/************************************************************************************
DSP plugin interface
************************************************************************************/
class IMJDSPPlugin
{
public:
	
	// destruction
	virtual ~IMJDSPPlugin() {}

	// core DSP plugin functions
	virtual int Process(float * pBuffer, int nBlocks, IMJDSPOutput * pOutput) = 0;
	virtual int Flush() = 0;
	virtual int SetAudioFormat(const WAVEFORMATEX * pwfeInput) = 0;

	// generic interface functions (extensible)
	virtual int	GetInformation(MJ_DSP_INFO_FIELDS nInfoID, LPTSTR pResult) = 0;
	virtual int	SetInformation(MJ_DSP_INFO_FIELDS nInfoID, LPCTSTR pValue) = 0;
	virtual int	DoCommand(MJ_DSP_COMMANDS nCommandID, int nParam1 = 0, int nParam2 = 0) = 0;
};

// the index of the plugin, the notification pointer, and the zone index
typedef IMJDSPPlugin * (* procGetDSPPlugin)(int, IMJDSPNotification *, int);
