#pragma once
// StutterDlg.h : header file

// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

class CMJStutterDSPPlugin;

/////////////////////////////////////////////////////////////////////////////
// CStutterDlg dialog

class CStutterDlg : public CDialog
{
// Construction
public:
	CStutterDlg(CMJStutterDSPPlugin * pPlugin, CWnd * pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CStutterDlg)
	enum { IDD = IDD_STUTTER };
	CSliderCtrl	m_ctrlStutterDuration;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CStutterDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CStutterDlg)
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	CMJStutterDSPPlugin * m_pPlugin;
};
