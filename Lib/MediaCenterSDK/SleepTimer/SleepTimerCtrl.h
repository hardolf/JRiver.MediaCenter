// SleepTimerCtrl.h : Declaration of the CSleepTimerCtrl
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

#ifndef __SLEEPTIMERCTRL_H_
#define __SLEEPTIMERCTRL_H_

#include "resource.h"       // main symbols
#include <atlctl.h>

#include "StaticCounter.h"

/////////////////////////////////////////////////////////////////////////////
// CSleepTimerCtrl
class ATL_NO_VTABLE CSleepTimerCtrl : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public IDispatchImpl<ISleepTimerCtrl, &IID_ISleepTimerCtrl, &LIBID_SLEEPTIMERLib>,
	public CComCompositeControl<CSleepTimerCtrl>,
	public IPersistStreamInitImpl<CSleepTimerCtrl>,
	public IOleControlImpl<CSleepTimerCtrl>,
	public IOleObjectImpl<CSleepTimerCtrl>,
	public IOleInPlaceActiveObjectImpl<CSleepTimerCtrl>,
	public IViewObjectExImpl<CSleepTimerCtrl>,
	public IOleInPlaceObjectWindowlessImpl<CSleepTimerCtrl>,
	public IPersistStorageImpl<CSleepTimerCtrl>,
	public ISpecifyPropertyPagesImpl<CSleepTimerCtrl>,
	public IQuickActivateImpl<CSleepTimerCtrl>,
	public IDataObjectImpl<CSleepTimerCtrl>,
	public IProvideClassInfo2Impl<&CLSID_SleepTimerCtrl, NULL, &LIBID_SLEEPTIMERLib>,
	public CComCoClass<CSleepTimerCtrl, &CLSID_SleepTimerCtrl>
{
public:
	CSleepTimerCtrl();

DECLARE_REGISTRY_RESOURCEID(IDR_SLEEPTIMERCTRL)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CSleepTimerCtrl)
	COM_INTERFACE_ENTRY(ISleepTimerCtrl)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(IViewObjectEx)
	COM_INTERFACE_ENTRY(IViewObject2)
	COM_INTERFACE_ENTRY(IViewObject)
	COM_INTERFACE_ENTRY(IOleInPlaceObjectWindowless)
	COM_INTERFACE_ENTRY(IOleInPlaceObject)
	COM_INTERFACE_ENTRY2(IOleWindow, IOleInPlaceObjectWindowless)
	COM_INTERFACE_ENTRY(IOleInPlaceActiveObject)
	COM_INTERFACE_ENTRY(IOleControl)
	COM_INTERFACE_ENTRY(IOleObject)
	COM_INTERFACE_ENTRY(IPersistStreamInit)
	COM_INTERFACE_ENTRY2(IPersist, IPersistStreamInit)
	COM_INTERFACE_ENTRY(ISpecifyPropertyPages)
	COM_INTERFACE_ENTRY(IQuickActivate)
	COM_INTERFACE_ENTRY(IPersistStorage)
	COM_INTERFACE_ENTRY(IDataObject)
	COM_INTERFACE_ENTRY(IProvideClassInfo)
	COM_INTERFACE_ENTRY(IProvideClassInfo2)
END_COM_MAP()

BEGIN_PROP_MAP(CSleepTimerCtrl)
	PROP_DATA_ENTRY("_cx", m_sizeExtent.cx, VT_UI4)
	PROP_DATA_ENTRY("_cy", m_sizeExtent.cy, VT_UI4)
	// Example entries
	// PROP_ENTRY("Property Description", dispid, clsid)
	// PROP_PAGE(CLSID_StockColorPage)
END_PROP_MAP()

BEGIN_MSG_MAP(CSleepTimerCtrl)
	CHAIN_MSG_MAP(CComCompositeControl<CSleepTimerCtrl>)
	COMMAND_HANDLER(IDC_SLEEP, BN_CLICKED, OnClickedSleep)
	MESSAGE_HANDLER(WM_TIMER, OnTimer)
	MESSAGE_HANDLER(WM_SIZE, OnSize)
END_MSG_MAP()
// Handler prototypes:
//  LRESULT MessageHandler(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
//  LRESULT CommandHandler(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
//  LRESULT NotifyHandler(int idCtrl, LPNMHDR pnmh, BOOL& bHandled);

BEGIN_SINK_MAP(CSleepTimerCtrl)
	//Make sure the Event Handlers have __stdcall calling convention
END_SINK_MAP()

	STDMETHOD(OnAmbientPropertyChange)(DISPID dispid)
	{
		if (dispid == DISPID_AMBIENT_BACKCOLOR)
		{
			SetBackgroundColorFromAmbient();
			FireViewChange();
		}
		return IOleControlImpl<CSleepTimerCtrl>::OnAmbientPropertyChange(dispid);
	}



// IViewObjectEx
	DECLARE_VIEW_STATUS(0)

// ISleepTimerCtrl
public:
	STDMETHOD(Init)(/*[in]*/ LPDISPATCH pDisp);

	enum { IDD = IDD_SLEEPTIMERCTRL };

protected:
	
	IMJAutomationPtr m_pMJ;
	CStaticCounter m_ctrlCountdown;
	
	int m_nSleepSeconds;
	DWORD m_dwStartTickCount;
	BOOL m_bSleepActive;
	float m_fStartVolume;
	BOOL m_bFadeVolume;
	int m_nActionType;

	void ToggleSleep();

	LRESULT OnClickedSleep(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
	LRESULT OnTimer(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
};

#endif //__SLEEPTIMERCTRL_H_
