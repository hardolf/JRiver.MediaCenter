// MyImpl.cpp : Implementation of CMyImpl
// Copyright (C) 2002-2008 J. River, Inc. All rights reserved
// Go to www.jrmediacenter.com to download lates version of J. River Media Center
// Use in commercial applications requires written permission
// This software is provided "as is", with no warranty.
#include "stdafx.h"
#include "MyEncoder.h"
#include "MyImpl.h"
#include "process.h"

// verify that the wave header conforms to our overly strict interpretation (for this simple example program)
BOOL UnsupportedHeaderFormat(WAVEFILEHDR hdr, long fileLength)
{
	if (strnicmp(hdr.format, "RIFF", 4) != 0)
		return TRUE;
	if (strnicmp(hdr.data, "data", 4) != 0)
		return TRUE;
	if (strnicmp(hdr.wave_fmt, "WAVEfmt ", 8) != 0)
		return TRUE;
	if (hdr.bits_per_sample != 8 && hdr.bits_per_sample != 16)
		return TRUE;
	if (hdr.num_channels != 1 && hdr.num_channels != 2)
		return TRUE;
	if (hdr.fmt_tag != WAVE_FORMAT_PCM)
		return TRUE;
	if (hdr.fmt_len != 16)
		return TRUE;
	if (hdr.blk_align != (hdr.num_channels * (hdr.bits_per_sample / 8)))
		return TRUE;
	if (hdr.f_len != (unsigned long)fileLength - 8)
		return TRUE;
	if (hdr.data_len != (fileLength - sizeof(WAVEFILEHDR)))
		return TRUE;

	return FALSE;  // header is OK!
}

void ReverseBuffer(WAVEFILEHDR hdr, char *buf, int len)
{
	for (int i=0; i<(len / hdr.blk_align)/2; i++) {
		int chunkLoc1 = (i*hdr.blk_align);
		int chunkLoc2 = (len - hdr.blk_align) - chunkLoc1;
		for (int j=0; j<hdr.blk_align; j++) {
			char tmpByte = buf[chunkLoc1 + j];
			buf[chunkLoc1 + j] = buf[chunkLoc2 + j];
			buf[chunkLoc2 + j] = tmpByte;
		}
	}
}

/////////////////////////////////////////////////////////////////////////////
// CMyImpl


STDMETHODIMP CMyImpl::Options(long *error)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// TODO: Add your implementation code here
	*error = 0;
	return S_OK;
}

STDMETHODIMP CMyImpl::Start(long *error)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// this thread reads the input file and sends buffers to Mpc
	*error = 0;
	m_bSendProgressMessages = TRUE;
	m_bDeleteSourceFile = FALSE;
	m_hEncodeThread = (HANDLE)_beginthread(EncThread, 0, this);

	if (m_hEncodeThread == INVALID_HANDLE_VALUE) {
		*error = -1;
		return S_FALSE;
	}

	return S_OK;
}

STDMETHODIMP CMyImpl::Stop()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	m_bStopped = TRUE;
	while(m_hEncodeThread != INVALID_HANDLE_VALUE)
	{
		MSG message;
		if( ::GetMessage(&message, NULL, 0, 0))
		{
			::TranslateMessage(&message);
			::DispatchMessage(&message);
		}
	}
	
	if (m_bDeleteSourceFile) {
		::DeleteFile(m_SourceFile);
	}

	return S_OK;
}

STDMETHODIMP CMyImpl::get_DestinationFile(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = _bstr_t(m_DestinationFile);

	return S_OK;
}

STDMETHODIMP CMyImpl::put_DestinationFile(BSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	m_DestinationFile = "";
	m_DestinationFile += CString(newVal);
	m_DestinationFile += ".";
	m_DestinationFile += MY_EXTENSION;
	return S_OK;
}

STDMETHODIMP CMyImpl::get_Quality(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// TODO: Add your implementation code here

	return S_OK;
}

STDMETHODIMP CMyImpl::put_Quality(BSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// TODO: Add your implementation code here

	return S_OK;
}

STDMETHODIMP CMyImpl::get_QualityList(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	char buf[100];
	strcpy (buf, "Backwards");
	*pVal = _bstr_t(buf);
	return S_OK;
}

STDMETHODIMP CMyImpl::get_SourceFile(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = _bstr_t(m_SourceFile);

	return S_OK;
}

STDMETHODIMP CMyImpl::put_SourceFile(BSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	m_SourceFile = "";
	m_SourceFile += CString(newVal);
	
	return S_OK;
}

STDMETHODIMP CMyImpl::FinishBufferInput()
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if (m_fileSize == 0) {
		m_outputFile.Seek(0, CFile::begin);
		m_waveFileHdr.f_len = m_bytesWritten + sizeof(WAVEFILEHDR) - 8;
		m_waveFileHdr.data_len = m_bytesWritten;
		m_outputFile.Write(&m_waveFileHdr, sizeof(WAVEFILEHDR));
		m_outputFile.Close();
		m_hEncodeThread = (HANDLE)_beginthread(EncThread, 0, this);

		if (m_hEncodeThread == INVALID_HANDLE_VALUE) {
			SendError(-1, "Error during the reversal thread.");
		}
		while (m_hEncodeThread != INVALID_HANDLE_VALUE)
			Sleep (50);

		::DeleteFile(m_SourceFile);  // delete the temp file
	}
	else {
		m_outputFile.Close();
	}

	return S_OK;
}

STDMETHODIMP CMyImpl::EncodeBuffer(unsigned char *buffer, long buflen, long *error)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*error = 0;
	if (m_fileSize == 0) {
		m_outputFile.Write(buffer, buflen);
	}
	else {
		int offset = m_bytesWritten + buflen;
		m_outputFile.Seek(-offset, CFile::end);
		ReverseBuffer(m_waveFileHdr, (char *)buffer, buflen);
		m_outputFile.Write(buffer, buflen);
	}
	m_bytesWritten += buflen;

	return S_OK;
}

STDMETHODIMP CMyImpl::InitBufferInput(void *pWaveFormat, BOOL bRawWaveData, long inputFileSize, long *error)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*error = 0;

	m_bytesWritten = 0;
	m_bSendProgressMessages = FALSE;

	// for this example program, we're not handling memory buffer input which includes a header. This never
	// actually happens in Media Jukebox anyway.
	if (!bRawWaveData) {
		*error = -1;
		return S_OK;
	}

	// if inputfilesize is less than 1, it means the calling application doesn't know the size in advance
	if (inputFileSize < 1)
		m_fileSize = 0;
	else
		m_fileSize = inputFileSize;

	WAVEFILEHDR hdr;
	int fileSize;
	if (pWaveFormat != NULL) {
		WAVEFORMATEX *pWavFmt = (WAVEFORMATEX *)pWaveFormat;
		hdr.avg_bytes_per_sec = pWavFmt->nAvgBytesPerSec;
		hdr.bits_per_sample = pWavFmt->wBitsPerSample;
		hdr.blk_align = pWavFmt->nBlockAlign;
		hdr.fmt_tag = pWavFmt->wFormatTag;
		hdr.num_channels = pWavFmt->nChannels;
		hdr.samples_per_sec = pWavFmt->nSamplesPerSec;
	}

	if (m_fileSize == 0) {
		hdr.data_len = 0;
		hdr.f_len = sizeof(WAVEFILEHDR) - 8;
		fileSize = sizeof(WAVEFILEHDR);
	}
	else {
		hdr.data_len = inputFileSize;
		hdr.f_len = inputFileSize + sizeof(WAVEFILEHDR) - 8;
	}
	if (UnsupportedHeaderFormat(hdr, m_fileSize + sizeof(WAVEFILEHDR))) {
		*error = -1;
		return S_OK;
	}
	
	// Now write the wave header to the output file. If we know the data size in advance, we will write
	// directly to the given destination file, reversing buffers as we go. If we don't know the size in
	// advance, we have to create a temp. file and then at the end (in FinishBufferInput) we'll process
	// this temp file into the final destination file.
	if (m_fileSize == 0) {
		TCHAR drive[_MAX_DRIVE];
		TCHAR dir[_MAX_DIR];
		TCHAR fname[_MAX_FNAME];
		TCHAR ext[_MAX_EXT];
		_tsplitpath(m_DestinationFile, drive, dir, fname, ext);
		m_SourceFile.Format(_T("%s%s"), drive, dir);
		TCHAR *name = _ttempnam(m_SourceFile, _T(""));
		m_SourceFile = name;
		free(name);
		m_outputFile.Open(m_SourceFile, CFile::modeCreate | CFile::modeWrite);
		m_outputFile.Write(&hdr, sizeof(WAVEFILEHDR));
		m_bDeleteSourceFile = TRUE;
	}
	else {
		m_outputFile.Open(m_DestinationFile, CFile::modeCreate | CFile::modeWrite);
		m_outputFile.SetLength(m_fileSize + sizeof(WAVEFILEHDR));
		m_outputFile.Seek(0, CFile::begin);
		m_outputFile.Write(&hdr, sizeof(WAVEFILEHDR));
	}
	m_waveFileHdr = hdr;

	return S_OK;
}

void CMyImpl::EncThread(void *pVoidThis)
{
	WAVEFILEHDR waveFileHdr;

	CMyImpl *pMyImpl = (CMyImpl *) pVoidThis;

	// open the input file (wave)
	CFile inputFile, outputFile;
	if (!inputFile.Open(pMyImpl->m_SourceFile, CFile::modeRead)) {
		pMyImpl->SendError(1, "Error opening input file.");
		pMyImpl->m_hEncodeThread = INVALID_HANDLE_VALUE;
		return;
	}

	// get the input file size for tracking conversion progress
	DWORD inputFileSize = (DWORD)inputFile.GetLength();
	if (inputFileSize == 0) {
		pMyImpl->SendError(2, "Error determining input file length.");
		pMyImpl->m_hEncodeThread = INVALID_HANDLE_VALUE;
		return;
	}

	// read the wave header if it exists...
	if (!pMyImpl->m_bRawWaveData) {
		if (inputFile.Read(&waveFileHdr, sizeof(waveFileHdr)) != sizeof(waveFileHdr)) {
			pMyImpl->SendError(4, "Error reading wave file header.");
			pMyImpl->m_hEncodeThread = INVALID_HANDLE_VALUE;
			return;
		}
		if (UnsupportedHeaderFormat(waveFileHdr, inputFileSize)) {
			pMyImpl->SendError(5, "Unsupported header format.");
			pMyImpl->m_hEncodeThread = INVALID_HANDLE_VALUE;
			return;
		}
	}
	else {
		// if m_bRawWaveData is true, we assume 16 bit, stereo, 44100hz !!
		// set the length fields in the default header created by the constructor
		waveFileHdr.data_len = inputFileSize;
		waveFileHdr.f_len = inputFileSize + sizeof(waveFileHdr) - 8;
	}

	// open the output file and write the header
	if (!outputFile.Open(pMyImpl->m_DestinationFile, CFile::modeCreate | CFile::modeWrite)) {
		pMyImpl->SendError(6, "Error opening output file.");
		pMyImpl->m_hEncodeThread = INVALID_HANDLE_VALUE;
		return;
	}
	outputFile.Write(&waveFileHdr, sizeof(waveFileHdr));
	outputFile.SetLength(inputFileSize);

	// our conversion buffer holds one second of wave data
	int buflen = waveFileHdr.avg_bytes_per_sec;  
	char *buffer = new char[buflen];
	if (buffer == NULL) {
		pMyImpl->SendError(3, "Error allocating a memory buffer.");
		pMyImpl->m_hEncodeThread = INVALID_HANDLE_VALUE;
		return;
	}

	// loop through the file stopping at EOF or on a stop command
	DWORD totalBytesRead=0;
	DWORD numBytesRead;
	while (!pMyImpl->m_bStopped && (numBytesRead = inputFile.Read(buffer, buflen)) > 0) {
		int offset = totalBytesRead + numBytesRead;
		outputFile.Seek(-offset, CFile::end);
		ReverseBuffer(waveFileHdr, buffer, numBytesRead);
		outputFile.Write(buffer, numBytesRead);
		totalBytesRead += numBytesRead;
		if (pMyImpl->m_bSendProgressMessages) {
			DWORD percentComplete = (int)(((float)totalBytesRead / (float)inputFileSize) * 100.0 + .5);
			pMyImpl->Fire_Progress(percentComplete);
		}
	}
	inputFile.Close();

	delete [] buffer;
	
	pMyImpl->m_hEncodeThread = INVALID_HANDLE_VALUE;
	if (!pMyImpl->m_bStopped && pMyImpl->m_bSendProgressMessages) pMyImpl->Fire_Finished();  // don't need to fire finished if we were stopped
	return;
}

void CMyImpl::SendError(long error, char *errorString)
{
	BSTR bstrError;

	bstrError = _bstr_t(errorString);
	Fire_Failure(error, &bstrError);
}


STDMETHODIMP CMyImpl::SetName(BSTR Name)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	// TODO: Add your implementation code here

	return S_OK;
}
