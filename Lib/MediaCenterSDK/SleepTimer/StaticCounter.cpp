// StaticCounter.cpp : implementation file


#include "stdafx.h"
#include "StaticCounter.h"


/////////////////////////////////////////////////////////////////////////////
// CStaticCounter

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
CStaticCounter::CStaticCounter()
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
{
	m_crBackground = RGB(0,0,0);
	m_crForeground = RGB(100, 255, 100);

	m_bLDown = m_bRDown = false;

	m_uID=0;

	m_nBarHeight = BARHEIGHTMIN + 4;
	m_nLastAmount = 0;

	m_bAllowInteraction = true;
	m_bFloat=false;
	m_strDisplay = "0";
	m_strFormat="%.3d";
	m_bSpecifiedFadeColour = false;
	m_bDrawFadedNotches = true;
	m_fPos = 0;
	SetRange(0,100);
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
CStaticCounter::~CStaticCounter()
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
{
}

BEGIN_MESSAGE_MAP(CStaticCounter, CStatic)
	//{{AFX_MSG_MAP(CStaticCounter)
	ON_WM_PAINT()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_KEYDOWN()
	ON_WM_RBUTTONDOWN()
	ON_WM_RBUTTONUP()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CStaticCounter message handlers

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
void CStaticCounter::CalculateMetrics()
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
{
	// Calculate the character metrics in proportion to the size of the control:
	m_nTopPadding = 2;
	do
	{
		int nHeight = m_recClient.bottom - (2 * m_nTopPadding);
	
		m_nMargin = (nHeight * 0.06) < 1 ? 1 : (int)(nHeight * 0.06);
		m_nNotchLength = (nHeight * 0.35) < 1 ? 1 : (int)(nHeight * 0.35);

		m_nNotchWidth = m_nMargin;
		m_nLeftPadding = (m_recClient.Width() - GetDisplayWidth(m_strDisplay)) / 2;

		m_nTopPadding += 1;

	} while ((m_nLeftPadding) < 4 && (m_nMargin != 1) && (m_nNotchLength != 1));

	m_nTopPadding -= 1;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
int CStaticCounter::GetDisplayWidth(const CString & strDisplay) 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
{
	int nWidth = 0;

	for (int nCount = 0; nCount <= strDisplay.GetLength(); nCount++)
	{
		if (nCount > 0)
		{
			if ((nCount < strDisplay.GetLength()) && (strDisplay[nCount] == ':'))	
				nWidth += m_nNotchLength + m_nMargin;
			else if (strDisplay[nCount - 1] == ':')	
				nWidth += m_nNotchLength + 1;
			else
				nWidth += m_nNotchLength + (m_nMargin * 4);
		}
	}

	return nWidth - (m_nMargin * 2);
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
void CStaticCounter::OnPaint() 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
{
	GetClientRect(&m_recClient);
	CPaintDC dc(this);
	CStaticDC memDC(&dc, m_recClient);
	CStaticDC* pDC = &memDC;

	pDC->FillSolidRect(&m_recClient, m_crBackground);

	CalculateMetrics();

	int nColPos = m_nLeftPadding;

	CString strFormatted = m_strDisplay;

	for (int nCount = 0; nCount<strFormatted.GetLength(); nCount++)
	{
		// Calculate the position of the next character:

		if ( nCount > 0 )
		{
			if (strFormatted[nCount] == ':')	
				nColPos+= m_nNotchLength+m_nMargin;
			else if (strFormatted[nCount-1] == ':' )	
				nColPos+= m_nNotchLength+1;
			else
				nColPos += m_nNotchLength + (m_nMargin*4);
		}

		// First 'lay down' the faded notches:
		if (m_bDrawFadedNotches && strFormatted[nCount] != ':' )	
			Draw( pDC, STCOUNTERALL, nColPos );

		if		  ( strFormatted[nCount] == '0' ) Draw( pDC, STCOUNTER0, nColPos );
		else if ( strFormatted[nCount] == '1' )	Draw( pDC, STCOUNTER1, nColPos );
		else if ( strFormatted[nCount] == '2' )	Draw( pDC, STCOUNTER2, nColPos );
		else if ( strFormatted[nCount] == '3' )	Draw( pDC, STCOUNTER3, nColPos );
		else if ( strFormatted[nCount] == '4' )	Draw( pDC, STCOUNTER4, nColPos );
		else if ( strFormatted[nCount] == '5' )	Draw( pDC, STCOUNTER5, nColPos );
		else if ( strFormatted[nCount] == '6' )	Draw( pDC, STCOUNTER6, nColPos );
		else if ( strFormatted[nCount] == '7' )	Draw( pDC, STCOUNTER7, nColPos );
		else if ( strFormatted[nCount] == '8' )	Draw( pDC, STCOUNTER8, nColPos );
		else if ( strFormatted[nCount] == '9' )	Draw( pDC, STCOUNTER9, nColPos );
		else if ( strFormatted[nCount] == '-' )	Draw( pDC, STCOUNTER10, nColPos );
		else if ( strFormatted[nCount] == '.' )	Draw( pDC, STCOUNTER11, nColPos );
		else if ( strFormatted[nCount] == ':' )	Draw( pDC, STCOUNTER12, nColPos );
	}
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
void CStaticCounter::Draw(CStaticDC* pDC, DWORD dwChar, int nCol)
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
{
	COLORREF crNotchColor = m_crForeground;
	
	if (dwChar == STCOUNTERALL && !m_bSpecifiedFadeColour)	// The colour used will be a dim version of normal foreground
	{
		int r = GetRValue(m_crForeground)/3;
		int g = GetGValue(m_crForeground)/3;
		int b = GetBValue(m_crForeground)/3;
		crNotchColor = RGB(r,g,b);
	}
	else if (dwChar == STCOUNTERALL && m_bSpecifiedFadeColour)
		crNotchColor = m_crDimForeground;

	// Create the Pen accordingly
	CPen pen(PS_SOLID | PS_ENDCAP_ROUND, m_nNotchWidth, crNotchColor);
	CPen* pOldPen=pDC->SelectObject(&pen);


	if ( (dwChar & NOTCH1) || dwChar == STCOUNTERALL)	{	// should I draw the first bar in the display?
		pDC->MoveTo( nCol + m_nMargin*2, m_nMargin + m_nTopPadding);
		pDC->LineTo( nCol + m_nNotchLength, m_nMargin + m_nTopPadding);
	}

	if ( dwChar & NOTCH2 || dwChar == STCOUNTERALL)	{	// should I draw the 2nd bar in the display? [minus sign]
		pDC->MoveTo(nCol + m_nNotchLength + m_nMargin, m_nMargin*2 + m_nTopPadding);
		pDC->LineTo(nCol + m_nNotchLength + m_nMargin, m_nNotchLength + (m_nMargin*2) + m_nTopPadding);
	}

	if ( dwChar & NOTCH3 || dwChar == STCOUNTERALL)	{	// should I draw the 3rd bar in the display?
		pDC->MoveTo(nCol + m_nNotchLength + m_nMargin, m_nNotchLength + (m_nMargin*4) + m_nTopPadding);
		pDC->LineTo(nCol + m_nNotchLength + m_nMargin, m_nNotchLength*2 + (m_nMargin*3) + m_nTopPadding);
	}

	if ( dwChar & NOTCH4 || dwChar == STCOUNTERALL)	{	// should I draw the 4th bar in the display?
		pDC->MoveTo( nCol + m_nMargin*2, m_nNotchLength*2 + (m_nMargin*4) + m_nTopPadding);
		pDC->LineTo( nCol + m_nNotchLength, m_nNotchLength*2 + (m_nMargin*4) + m_nTopPadding);
	}

	if ( dwChar & NOTCH5 || dwChar == STCOUNTERALL)	{	// should I draw the 5th bar in the display?
		pDC->MoveTo(nCol + m_nMargin, m_nNotchLength + (m_nMargin*4) + m_nTopPadding);
		pDC->LineTo(nCol + m_nMargin, m_nNotchLength*2 + (m_nMargin*3) + m_nTopPadding);
	}

	if ( dwChar & NOTCH6 || dwChar == STCOUNTERALL)	{	// should I draw the 6th bar in the display?
		pDC->MoveTo(nCol + m_nMargin, m_nMargin*2 + m_nTopPadding);
		pDC->LineTo(nCol + m_nMargin, m_nNotchLength + (m_nMargin*2) + m_nTopPadding);
	}

	if ( dwChar & NOTCH7 || dwChar == STCOUNTERALL)	{	// should I draw the 7th bar in the display?
		pDC->MoveTo(nCol + m_nMargin*2, m_nNotchLength + (m_nMargin*3) + m_nTopPadding);
		pDC->LineTo(nCol + m_nMargin + m_nNotchLength - m_nMargin, m_nNotchLength + (m_nMargin*3) + m_nTopPadding);
	}

	if ( dwChar == STCOUNTER11 )	{	// should I draw the point?
		pDC->MoveTo( nCol + m_nMargin*2, m_nNotchLength*2 + (m_nMargin*4) + m_nTopPadding);
		pDC->LineTo( nCol + (m_nNotchLength/2), m_nNotchLength*2 + (m_nMargin*4) + m_nTopPadding);
	}

	if ( dwChar == STCOUNTER12 )	{	// should I draw the colon?
		// Upper dot:
		pDC->MoveTo( nCol + m_nMargin*2+(m_nMargin*2), m_nNotchLength + m_nTopPadding);
		pDC->LineTo( nCol + (m_nNotchLength/2)+(m_nMargin*2), m_nNotchLength + m_nTopPadding);

		// Lower dot:
		pDC->MoveTo( nCol + m_nMargin*2+(m_nMargin*2), m_nNotchLength*2 + (m_nMargin) + m_nTopPadding);
		pDC->LineTo( nCol + (m_nNotchLength/2)+(m_nMargin*2), m_nNotchLength*2 + (m_nMargin) + m_nTopPadding);
	}

	pDC->SelectObject(pOldPen);
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
CRect CStaticCounter::GetRect(UINT uID, CDialog *pDlg)
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// I wrote this static function here to save me re-writing it for every dialog app that uses this class (about time!)
{
	CWnd* pWnd = pDlg->GetDlgItem(uID);
	ASSERT(pWnd);

	CRect rect;
	pWnd->GetWindowRect( &rect );
	pDlg->ScreenToClient( &rect );

	return rect;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
void CStaticCounter::OnRButtonDown(UINT nFlags, CPoint point) 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
{
	SetCapture();
	m_bRDown = true;
	m_nMovement = point.x;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
void CStaticCounter::OnRButtonUp(UINT nFlags, CPoint point) 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
{
	ReleaseCapture();
	
	if (m_nLastAmount==0 && m_bAllowInteraction)	// ALLOW UNIT CHANGES WHEN IT'S HARD TO DRAG FOR SMALL NUMBERS
	{
		m_fPos+=(m_bFloat?0.01f:1);	// INCREMENT

		if (m_fPos<m_fMin) m_fPos = m_fMin;
		if (m_fPos>m_fMax) m_fPos = m_fMax;
		if (m_bFloat)
			DisplayFloat(m_fPos);
		else
			DisplayInt((int)m_fPos);
	}

	#ifdef WM_UPDATE_STATIC
		//#define WM_UPDATE_STATIC (WM_USER+0x411)		// user defined WM message
		::PostMessage(  GetParent()->m_hWnd,  WM_UPDATE_STATIC, (WPARAM)m_uID, (LPARAM)m_fPos );
	#endif

	m_nLastAmount = 0;
	m_bRDown = false;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
void CStaticCounter::OnLButtonDown(UINT nFlags, CPoint point) 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
{
	m_bLDown = true;
	m_nMovement = point.x;
	SetCapture();
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
void CStaticCounter::OnLButtonUp(UINT nFlags, CPoint point) 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
{
	ReleaseCapture();
	
	if (m_nLastAmount==0 && m_bAllowInteraction)	// ALLOW UNIT CHANGES WHEN IT'S HARD TO DRAG FOR SMALL NUMBERS
	{
		m_fPos-=(m_bFloat?0.01f:1);	// DECREMENT

		if (m_fPos<m_fMin) m_fPos = m_fMin;
		if (m_fPos>m_fMax) m_fPos = m_fMax;
		if (m_bFloat)
			DisplayFloat(m_fPos);
		else
			DisplayInt((int)m_fPos);
	}

	#ifdef WM_UPDATE_STATIC
		//#define WM_UPDATE_STATIC (WM_USER+0x411)		// user defined WM message
		::PostMessage(  GetParent()->m_hWnd,  WM_UPDATE_STATIC, (WPARAM)m_uID, (LPARAM)m_fPos );
	#endif

	m_nLastAmount = 0;
	m_bLDown = false;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Function Header
void CStaticCounter::OnMouseMove(UINT nFlags, CPoint point) 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
{
	if (! m_bLDown && !m_bRDown)	return;		// If neither mouse buttons are down, don't come here thinking this is an off-license, ok!
	if (! m_bAllowInteraction)	return;					// "Sorry, I'm grounded and my dad says I'm not allowed visitors"

	m_nLastAmount = point.x-m_nMovement;
	m_nMovement = point.x;

	if (m_bLDown)
		m_fPos+=(float)m_nLastAmount;
	else
		m_fPos+=(float)((float)m_nLastAmount/(float)100);

	if (m_fPos<m_fMin) m_fPos = m_fMin;
	if (m_fPos>m_fMax) m_fPos = m_fMax;

	if (m_bFloat)
		DisplayFloat(m_fPos);
	else
		DisplayInt((int)m_fPos);
}
