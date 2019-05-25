// ImpactFillImpl.h : Declaration of the CImpactFillImpl

// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.


#ifndef __IMPACTFILL_H_
#define __IMPACTFILL_H_

#include "resource.h"       // main symbols
#include <atlctl.h>

#include "VisPlugin.h"
#include "IMenuEx.h"

/////////////////////////////////////////////////////////////////////////////
// CImpactFillImpl
class ATL_NO_VTABLE CImpactFillImpl : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public IDispatchImpl<IImpactFill, &IID_IImpactFill, &LIBID_IMPACTFILLLib>,
	public CComControl<CImpactFillImpl>,
	public IPersistStreamInitImpl<CImpactFillImpl>,
	public IOleControlImpl<CImpactFillImpl>,
	public IOleObjectImpl<CImpactFillImpl>,
	public IOleInPlaceActiveObjectImpl<CImpactFillImpl>,
	public IViewObjectExImpl<CImpactFillImpl>,
	public IOleInPlaceObjectWindowlessImpl<CImpactFillImpl>,
	public IPersistStorageImpl<CImpactFillImpl>,
	public ISpecifyPropertyPagesImpl<CImpactFillImpl>,
	public IQuickActivateImpl<CImpactFillImpl>,
	public IDataObjectImpl<CImpactFillImpl>,
	public IProvideClassInfo2Impl<&CLSID_ImpactFill, NULL, &LIBID_IMPACTFILLLib>,
	public CComCoClass<CImpactFillImpl, &CLSID_ImpactFill>
{
public:
	CImpactFillImpl();
	~CImpactFillImpl();

DECLARE_REGISTRY_RESOURCEID(IDR_IMPACTFILL)
DECLARE_NOT_AGGREGATABLE(CImpactFillImpl)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CImpactFillImpl)
	COM_INTERFACE_ENTRY(IImpactFill)
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

BEGIN_PROP_MAP(CImpactFillImpl)
	PROP_DATA_ENTRY("_cx", m_sizeExtent.cx, VT_UI4)
	PROP_DATA_ENTRY("_cy", m_sizeExtent.cy, VT_UI4)
	// Example entries
	// PROP_ENTRY("Property Description", dispid, clsid)
	// PROP_PAGE(CLSID_StockColorPage)
END_PROP_MAP()

BEGIN_MSG_MAP(CImpactFillImpl)
	MESSAGE_HANDLER(WM_DESTROY, OnDestroy)
	MESSAGE_HANDLER(WM_ERASEBKGND, OnEraseBkgnd)
	MESSAGE_HANDLER(UM_CONTROL_BAR_COMMAND, OnControlBarCommand)
	CHAIN_MSG_MAP(CComControl<CImpactFillImpl>)
	DEFAULT_REFLECTION_HANDLER()
END_MSG_MAP()
// Handler prototypes:
//  LRESULT MessageHandler(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
//  LRESULT CommandHandler(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
//  LRESULT NotifyHandler(int idCtrl, LPNMHDR pnmh, BOOL& bHandled);

// IViewObjectEx
	DECLARE_VIEW_STATUS(VIEWSTATUS_SOLIDBKGND | VIEWSTATUS_OPAQUE)

// IImpactFill
public:

	LRESULT OnDestroy(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnEraseBkgnd(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	HRESULT OnDrawAdvanced(ATL_DRAWINFO& di);
	LRESULT OnRButtonUp(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnKeyDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnLButtonDblClk(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnControlBarCommand(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	STDMETHOD(Init)(IDispatch* pMJ);
	STDMETHOD(Terminate)();


private:
	
	unsigned int GetFillColor();

	IMJAutomationPtr m_pMJ;

	IVisRedrawHelper * m_pVisRedrawHelper;
	IVisData * m_pVisData;
	int m_nPreset;
	unsigned int m_rgbFillColor;
	BOOL m_bShowFramerate;
};

#endif //__IMPACTFILL_H_
