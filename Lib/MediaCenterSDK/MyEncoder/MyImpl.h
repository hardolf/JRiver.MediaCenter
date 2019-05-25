// MyImpl.h : Declaration of the CMyImpl

// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.

#ifndef __MYIMPL_H_
#define __MYIMPL_H_

#include "resource.h"       // main symbols
#include "MyEncoderCP.h"
#include "mmsystem.h"

#define ENCODE_FILE_ERROR	1000
#define REGISTRY_PATH_MJ_PLUGINS_ENCODERS _T("Software\\J. River\\Media Jukebox\\Plugins\\Encoders\\")
#define MY_EXTENSION _T("wav")

enum ISCONFIGURABLE
{
	ISCONFIGURABLE_NO,
	ISCONFIGURABLE_YES,
	ISCONFIGURABLE_MANDATORY
};

struct WAVEFILEHDR {
	WAVEFILEHDR ()
	{
		strcpy (format, "RIFF");
		f_len = 0;
		data_len = 0;
		strcpy (wave_fmt, "WAVEfmt ");
		fmt_len = 16;
		fmt_tag = WAVE_FORMAT_PCM;
		num_channels = 2;
		samples_per_sec = 44100;
		bits_per_sample = 16;
		blk_align = 4;
		avg_bytes_per_sec = num_channels * samples_per_sec * (bits_per_sample / 8);
		strcpy (data, "data");
	}
	char            format[4];         // "RIFF"
	unsigned long   f_len;             // filelength following this value (e.g. total file length minus 8 bytes)
	char            wave_fmt[8];       // "WAVEfmt "
	unsigned long   fmt_len;           // format length
	unsigned short  fmt_tag;           // format Tag
	unsigned short  num_channels;      // 1=Mono/2=Stereo
	unsigned long   samples_per_sec;	 // 44100 or 22050 for example
	unsigned long   avg_bytes_per_sec; // 176400 for stereo, 16 bit, 44100hz
	unsigned short  blk_align;				 // 4 for stereo, 16 bit
	unsigned short  bits_per_sample;	 // 8 or 16
	char            data[4];           // "data"
	unsigned long   data_len;          // data size in bytes following this header
};

/////////////////////////////////////////////////////////////////////////////
// CMyImpl
class ATL_NO_VTABLE CMyImpl : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CMyImpl, &CLSID_MyImpl>,
	public IConnectionPointContainerImpl<CMyImpl>,
	public IMyImpl,
	public _IMyBufferInputImpl,
	public CProxy_IMyImplEvents< CMyImpl >
{
public:
	CMyImpl()
	{
		m_bRawWaveData = FALSE;
		m_hEncodeThread = INVALID_HANDLE_VALUE;
		m_bStopped = FALSE;
		m_bSendProgressMessages = TRUE;
		m_bDeleteSourceFile = FALSE;
	}

DECLARE_REGISTRY_RESOURCEID(IDR_MYIMPL)

DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CMyImpl)
	COM_INTERFACE_ENTRY(IMyImpl)
	COM_INTERFACE_ENTRY(IConnectionPointContainer)
	COM_INTERFACE_ENTRY(_IMyBufferInputImpl)
	COM_INTERFACE_ENTRY_IMPL(IConnectionPointContainer)
END_COM_MAP()
BEGIN_CONNECTION_POINT_MAP(CMyImpl)
CONNECTION_POINT_ENTRY(IID__IMyImplEvents)
END_CONNECTION_POINT_MAP()


// IMyImpl
public:
	STDMETHOD(SetName)(BSTR Name);
	STDMETHOD(InitBufferInput)(void *pWaveFormat, BOOL bRawWaveData, long inputFileSize, long *error);
	STDMETHOD(EncodeBuffer)(unsigned char *buffer, long buflen, long *error);
	STDMETHOD(FinishBufferInput)();
	STDMETHOD(get_SourceFile)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_SourceFile)(/*[in]*/ BSTR newVal);
	STDMETHOD(get_QualityList)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_Quality)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_Quality)(/*[in]*/ BSTR newVal);
	STDMETHOD(get_DestinationFile)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_DestinationFile)(/*[in]*/ BSTR newVal);
	STDMETHOD(Stop)();
	STDMETHOD(Start)(long *error);
	STDMETHOD(Options)(long *error);

private:
	static void EncThread(void *pVoidThis);
	void SendError(long error, char *errorString);
	HANDLE	m_hEncodeThread;
	BOOL m_bStopped;
	CString m_DestinationFile;
	CString m_SourceFile;
	BOOL m_bRawWaveData;
	BOOL m_bSendProgressMessages;
	BOOL m_bDeleteSourceFile;

	// for encoding from memory buffers rather than a file...
	WAVEFILEHDR m_waveFileHdr;
	int m_fileSize;
	CFile m_outputFile;
	int m_bytesWritten;
};

#endif //__MYIMPL_H_
