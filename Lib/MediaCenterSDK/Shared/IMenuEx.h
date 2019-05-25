#pragma once

/***************************************************************************************************************
Defines
***************************************************************************************************************/
const UINT UM_CONTROL_BAR_COMMAND = RegisterWindowMessage(_T("J. River Playback Engine Control Bar Command"));

// menu flags
#define MF_SCROLL					0x010000
#define MF_POPUP_SELECTABLE			0x020000
#define MF_ADDTOSWITCHBAR			0x040000
#define MF_CHILD_WINDOW				0x080000
#define MF_ON_THE_FLY				0x100000
#define MF_CHILD_WINDOW_NO_DELETE	0x200000

// menu commands
#define CB_FILL_MENU				100
#define CB_PROCESS_MENU_COMMAND		101
#define CB_GET_DISPLAY_NAME			102
#define CB_MENU_SHOW				104
#define CB_MENU_HIDE				105
#define CB_FILL_MENU_MODES			106

// menu IDs
#define MJ_MENU_ID_MIN				100000
#define PLAYBACK_ENGINE_MENU_ID_MIN	2000

#define SLAVE_MENU_ID_MIN			1
#define SLAVE_MENU_ID_MAX			1999

#define PLUGIN_MENU_ID_MIN			100
#define PLUGIN_MENU_ID_PRESET_MIN	190
#define PLUGIN_MENU_ID_MAX			199

#define DISPLAY_MENU_ID_MIN			1000
#define DISPLAY_MENU_ID_MAX			1999

class IMenuEx;
class CMenuExManager;

/***************************************************************************************************************
MENU_INFO
***************************************************************************************************************/
class MENU_INFO
{
public:

	MENU_INFO()
	{
		Empty();
	}
	MENU_INFO(const MENU_INFO & a_Source)
	{
		CopyData(&a_Source);
	}
	
	MENU_INFO & operator=(MENU_INFO & a_Source)
	{
		CopyData(&a_Source);
		return *this;
	}

	virtual void Empty()
	{
		nID = -1;
		cText[0] = 0;
		pMenu = NULL;
	}
	
	int nID;
	TCHAR cText[1024];

	IMenuEx * pMenu;

protected:
	
	virtual void CopyData(const MENU_INFO * a_pSource)
	{
		nID = a_pSource->nID;
		_tcscpy(cText, a_pSource->cText);
		pMenu = a_pSource->pMenu;
	}
};

/***************************************************************************************************************
MENU_INFO_EX
***************************************************************************************************************/
// This is for J. River Internal Use Only.
// For third party plugins, please define THIRD_PARTY_PLUGIN in the preprocessor
#ifndef THIRD_PARTY_PLUGIN
class MENU_INFO_EX : public MENU_INFO
{
public:

	MENU_INFO_EX()
	{
		Empty();
	}
	MENU_INFO_EX(const MENU_INFO_EX & a_Source)
	{
		CopyData(&a_Source);
	}

	MENU_INFO_EX & operator=(MENU_INFO_EX & a_Source)
	{
		CopyData(&a_Source);
		return *this;
	}

	void Empty()
	{
		nID = -1;
		strText.Empty();
		strPath.Empty();
		aryPath.RemoveAll();
		aryItemData.RemoveAll();
		pMenu = NULL;
		nManagerKey = -1;

		MENU_INFO::Empty();
	}

	int GetItemData() const
	{
		return (aryItemData.GetSize() > 0) ? aryItemData[aryItemData.GetUpperBound()] : 0;
	}
	
	JRString strText;
	JRString strItemData;

	JRString strPath;
	JRStringArray aryPath;
	JRUIntArray aryItemData;

	int nManagerKey;

protected:
	
	void CopyData(const MENU_INFO_EX * a_pSource)
	{
		strText = a_pSource->strText;
		strPath = a_pSource->strPath;
		strItemData = a_pSource->strItemData;
		aryPath.RemoveAll(); for (int z = 0; z < a_pSource->aryPath.GetSize(); z++) aryPath.Add(a_pSource->aryPath[z]);
		aryItemData.RemoveAll(); for (int z = 0; z < a_pSource->aryItemData.GetSize(); z++) aryItemData.Add(a_pSource->aryItemData[z]);
		nManagerKey = a_pSource->nManagerKey;
		MENU_INFO::CopyData(a_pSource);
	}
};
#endif

/***************************************************************************************************************
IMenuChildWindow
***************************************************************************************************************/
class IMenuChildWindow
{
public:

	virtual ~IMenuChildWindow() { }
	virtual SIZE GetMenuIdealSize() = 0;
	virtual BOOL Show(HWND hwndParent, RECT rectPosition, void * pvoidBackground) = 0;
	virtual BOOL Destroy() = 0;
};

/***************************************************************************************************************
IMenuEx
***************************************************************************************************************/
class IMenuEx
{
public:

	virtual void Add(int nID, LPCTSTR lpszText, int nFlags = 0, DWORD dwItemData = 0, LPCTSTR lpszItemData = NULL) = 0;
	virtual void AddPath(int nID, LPCTSTR lpszText, int nFlags = 0, DWORD dwItemData = 0, LPCTSTR lpszItemData = NULL, BOOL bCreateSubMenusSelectable = FALSE) = 0;
	virtual void AddSeparator() = 0;
	virtual IMenuEx * AddSubMenu(LPCTSTR lpszText, int nID = -1, BOOL bSelectable = FALSE, DWORD dwItemData = 0, LPCTSTR lpszItemData = NULL) = 0;
	virtual void Sort(BOOL bRecurse) = 0;
	virtual int  GetMaxMenuPixelHeight() = 0;
	virtual void SetMaxMenuPixelHeight(int a_nValue) = 0;
	virtual BOOL IsPtOnMenu(POINT a_ptMouse) = 0;
	virtual void AddChildWindow(IMenuChildWindow * pChild, BOOL bTakeOwnership = TRUE) = 0;
	virtual void InsertAt(int nIndex, int nID, LPCTSTR lpszText, int nFlags = 0, DWORD dwItemData = 0, LPCTSTR lpszItemData = NULL) = 0;
};
