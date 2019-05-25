// StutterDlg.cpp : implementation file
//
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

#include "stdafx.h"
#include "dsp_Stutter.h"
#include "StutterDlg.h"
#include "MJStutterDSPPlugin.h"


/////////////////////////////////////////////////////////////////////////////
// CStutterDlg dialog


CStutterDlg::CStutterDlg(CMJStutterDSPPlugin * pPlugin, CWnd * pParent)
	: CDialog(CStutterDlg::IDD, pParent)
{
	m_pPlugin = pPlugin;
	
	//{{AFX_DATA_INIT(CStutterDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CStutterDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CStutterDlg)
	DDX_Control(pDX, IDC_STUTTER_DURATION, m_ctrlStutterDuration);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CStutterDlg, CDialog)
	//{{AFX_MSG_MAP(CStutterDlg)
	ON_WM_HSCROLL()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CStutterDlg message handlers

BOOL CStutterDlg::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	m_ctrlStutterDuration.SetRange(1, 1000);
	m_ctrlStutterDuration.SetPos(m_pPlugin->GetStutterDuration());
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CStutterDlg::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar) 
{
	if ((pScrollBar) && (pScrollBar->GetDlgCtrlID() == IDC_STUTTER_DURATION))
	{
		CSliderCtrl * pSlider = (CSliderCtrl *) pScrollBar;
		m_pPlugin->SetStutterDuration(pSlider->GetPos());
	}
		
	CDialog::OnHScroll(nSBCode, nPos, pScrollBar);
}
