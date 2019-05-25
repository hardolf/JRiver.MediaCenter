#include "stdafx.h"
#include "CharacterHelper.h"

/*******************************************************************************************
Character set conversion helpers

Usage:

All functions allocate a block of memory that you must later free.  Use CSmartPtr to make
freeing of memory automatic. (#include "SmartPtr.h")

Example (UTF16 to ANSI):

// get a UTF16 string
const wchar_t * pUTF16 = L"Hi there!";

// get a smart pointer of the ANSI string
CSmartPtr<char> spANSI(GetANSIFromUTF16(pUTF16), TRUE);

// use the smart pointer just like a regular string (i.e. strlen(spANSI), etc.)
// note: there is nothing to free in this case -- CSmartPtr will do it for you

*******************************************************************************************/

/******************************************************************************************
ANSI
******************************************************************************************/
char * GetANSIFromUTF8(const unsigned char * pUTF8, UINT CodePage)
{
	if (pUTF8 == NULL) return NULL;

	wchar_t * pUTF16 = GetUTF16FromUTF8(pUTF8);
	char * pANSI = GetANSIFromUTF16(pUTF16, CodePage);
	delete [] pUTF16;
	return pANSI;
}

char * GetANSIFromUTF16(const wchar_t * pUTF16, UINT CodePage)
{
	if (pUTF16 == NULL) return NULL;

#ifdef _WINDOWS_
	char * pANSI = NULL;
	int nBytes = WideCharToMultiByte(CodePage, 0, pUTF16, -1, NULL, 0, NULL, NULL);
	if (nBytes > 0)
	{
		pANSI = new char [nBytes + 1]; pANSI[nBytes] = 0;
		if (WideCharToMultiByte(CodePage, 0, pUTF16, -1, pANSI, nBytes, NULL, NULL) == 0)
		{
			delete [] pANSI;
			pANSI = NULL;
		}
	}
	return pANSI;
#else
	const int nCharacters = wcslen(pUTF16);
	unsigned char * pANSI = new unsigned char [nCharacters + 1];
	for (int z = 0; z < nCharacters; z++)
	{
		pANSI[z] = (pUTF16[z] >= 256) ? '?' : (unsigned char) pUTF16[z];
	}
	pANSI[nCharacters] = 0;
	return (char *) pANSI;
#endif
}

/******************************************************************************************
UTF-16
******************************************************************************************/
wchar_t * GetUTF16FromANSI(const char * pANSI)
{
	if (pANSI == NULL) return NULL;

#ifdef _WINDOWS_
	wchar_t * pUTF16 = NULL;
	int nCharacters = MultiByteToWideChar(CP_ACP, 0, pANSI, -1, NULL, 0);
	if (nCharacters > 0)
	{
		pUTF16 = new wchar_t [nCharacters + 1]; pUTF16[nCharacters] = 0;
		if (MultiByteToWideChar(CP_ACP, 0, pANSI, -1, pUTF16, nCharacters) == 0)
		{
			delete [] pUTF16;
			pUTF16 = NULL;
		}
	}
	return pUTF16;
#else
	const int nCharacters = strlen(pANSI);
	wchar_t * pUTF16 = new wchar_t [nCharacters + 1];
	for (int z = 0; z < nCharacters; z++)
	{
		pUTF16[z] = (wchar_t) ((unsigned char) pANSI[z]);
	}
	pUTF16[nCharacters] = 0;
	return pUTF16;
#endif
}

wchar_t * GetUTF16FromUTF8(const unsigned char * pUTF8)
{
	if (pUTF8 == NULL) return NULL;

	const int nCharacters = GetUTF8Characters(pUTF8);
	wchar_t * pUTF16 = new wchar_t [nCharacters + 1];
	DecodeUTF8ToUTF16(pUTF8, pUTF16);
	return pUTF16; 
}

/******************************************************************************************
UTF-8
******************************************************************************************/
unsigned char * GetUTF8FromANSI(const char * pANSI)
{
	if (pANSI == NULL) return NULL;

	wchar_t * pUTF16 = GetUTF16FromANSI(pANSI);
	unsigned char * pUTF8 = GetUTF8FromUTF16(pUTF16);
	delete [] pUTF16;
	return pUTF8;
}

unsigned char * GetUTF8FromUTF16(const wchar_t * pUTF16, int nCharacters)
{
	if (pUTF16 == NULL) return NULL;

	// get the size(s)
	if (nCharacters < 0)
		nCharacters = wcslen(pUTF16);

	// get the UTF-8 bytes
	int nUTF8Bytes = GetUTF8Bytes(pUTF16, TRUE, nCharacters);

	// allocate a UTF-8 string
	unsigned char * pUTF8 = new unsigned char [nUTF8Bytes];

	// create the UTF-8 string
	DecodeUTF16ToUTF8(pUTF16, pUTF8, TRUE, nCharacters);

	// return the UTF-8 string
	return pUTF8;
}

/******************************************************************************************
UTF-8 Helpers
******************************************************************************************/
int GetUTF8CharacterBytes(const unsigned char * pUTF8, int nInvalidReturn)
{
	if ((pUTF8[0] & 0x80) == 0)
	{
		return 1;
	}
	else if (((pUTF8[0] & 0xE0) == 0xC0) &&
		((pUTF8[1] & 0xC0) == 0x80))
	{
		return 2;
	}
	else if (((pUTF8[0] & 0xE0) == 0xE0) &&
		((pUTF8[1] & 0xC0) == 0x80) &&
		((pUTF8[2] & 0xC0) == 0x80))
	{
		return 3;
	}

	// error condition where the UTF-8 multibyte bits are set, but the following
	// codes don't match (can happen when ANSI is passed through the UTF-8 decoder)
	return nInvalidReturn;
}

int GetUTF8Bytes(const wchar_t * pUTF16, BOOL bAccountForNullTerminator, int nCharacters)
{
	int nUTF8Bytes = 0; int nUTF16Index = 0;
	while (((nCharacters == -1) ? (pUTF16[nUTF16Index] != 0) : (nUTF16Index < nCharacters)))
	{
		if (pUTF16[nUTF16Index] < 0x0080)
			nUTF8Bytes += 1;
		else if (pUTF16[nUTF16Index] < 0x0800)
			nUTF8Bytes += 2;
		else
			nUTF8Bytes += 3;

		nUTF16Index++;
	}
	if (bAccountForNullTerminator)
		nUTF8Bytes++;
	return nUTF8Bytes;
}

int GetUTF8Characters(const unsigned char * pUTF8, int nUTF8Bytes)
{
	int nCharacters = 0; int nIndex = 0;
	if (nUTF8Bytes == -1)
	{
		while (pUTF8[nIndex] != 0)
		{
			nIndex += GetUTF8CharacterBytes(&pUTF8[nIndex]);
			nCharacters += 1;
		}
	}
	else
	{
		while (nIndex < nUTF8Bytes)
		{
			nIndex += GetUTF8CharacterBytes(&pUTF8[nIndex]);
			nCharacters += 1;
		}
	}

	return nCharacters;
}

void DecodeUTF16ToUTF8(const wchar_t * pUTF16, unsigned char * pUTF8, BOOL bNullTerminate, int nCharacters)
{
	// create the UTF-8 string
	int nUTF8Index = 0; int nUTF16Index = 0;
	while (((nCharacters == -1) ? (pUTF16[nUTF16Index] != 0) : (nUTF16Index < nCharacters)))
	{
		if (pUTF16[nUTF16Index] < 0x0080)
		{
			pUTF8[nUTF8Index++] = (unsigned char) pUTF16[nUTF16Index];
		}
		else if (pUTF16[nUTF16Index] < 0x0800)
		{
			pUTF8[nUTF8Index++] = 0xC0 | (pUTF16[nUTF16Index] >> 6);
			pUTF8[nUTF8Index++] = 0x80 | (pUTF16[nUTF16Index] & 0x3F);
		}
		else
		{
			pUTF8[nUTF8Index++] = 0xE0 | (pUTF16[nUTF16Index] >> 12);
			pUTF8[nUTF8Index++] = 0x80 | ((pUTF16[nUTF16Index] >> 6) & 0x3F);
			pUTF8[nUTF8Index++] = 0x80 | (pUTF16[nUTF16Index] & 0x3F);
		}

		nUTF16Index++;
	}
	if (bNullTerminate)
		pUTF8[nUTF8Index++] = 0;
}

void DecodeUTF8ToUTF16(const unsigned char * pUTF8, wchar_t * pUTF16, BOOL bNullTerminate)
{
	int nIndex = 0; int nCharacters = 0;
	while (pUTF8[nIndex] != 0)
	{
		int nCharacterBytes = GetUTF8CharacterBytes(&pUTF8[nIndex]);
		if (nCharacterBytes == 2)
		{			
			pUTF16[nCharacters] = ((pUTF8[nIndex] & 0x3F) << 6) | (pUTF8[nIndex + 1] & 0x3F);
			nIndex += 2;
		}
		else if (nCharacterBytes == 3)
		{
			pUTF16[nCharacters] = ((pUTF8[nIndex] & 0x1F) << 12) | ((pUTF8[nIndex + 1] & 0x3F) << 6) | (pUTF8[nIndex + 2] & 0x3F);
			nIndex += 3;
		}
		else
		{
			pUTF16[nCharacters] = pUTF8[nIndex];
			nIndex += 1;
		}

		nCharacters += 1;
	}
	if (bNullTerminate)
		pUTF16[nCharacters] = 0;
}

void DecodeUTF8ToUTF16(const unsigned char * pUTF8, int nUTF8Bytes, wchar_t * pUTF16, BOOL bNullTerminate)
{
	int nIndex = 0; int nCharacters = 0;
	while (nIndex < nUTF8Bytes)
	{
		int nCharacterBytes = GetUTF8CharacterBytes(&pUTF8[nIndex]);
		if (nCharacterBytes == 2)
		{			
			pUTF16[nCharacters] = ((pUTF8[nIndex] & 0x3F) << 6) | (pUTF8[nIndex + 1] & 0x3F);
			nIndex += 2;
		}
		else if (nCharacterBytes == 3)
		{
			pUTF16[nCharacters] = ((pUTF8[nIndex] & 0x1F) << 12) | ((pUTF8[nIndex + 1] & 0x3F) << 6) | (pUTF8[nIndex + 2] & 0x3F);
			nIndex += 3;
		}
		else
		{
			pUTF16[nCharacters] = pUTF8[nIndex];
			nIndex += 1;
		}

		nCharacters += 1;
	}
	if (bNullTerminate)
		pUTF16[nCharacters] = 0;
}

/******************************************************************************************
JRString Helpers
******************************************************************************************/
#ifdef JRFC_H
void DecodeUTF8ToJRString(const unsigned char * pUTF8, JRString & String, int nUTF8Bytes)
{
	if (pUTF8 != NULL)
	{
		if (nUTF8Bytes == -1)
		{
			int nCharacters = GetUTF8Characters((unsigned char *) pUTF8);
			DecodeUTF8ToUTF16(pUTF8, String.GetBuffer(nCharacters));
			String.ReleaseBuffer(nCharacters);
		}
		else
		{
			int nCharacters = GetUTF8Characters((unsigned char *) pUTF8, nUTF8Bytes);
			DecodeUTF8ToUTF16(pUTF8, nUTF8Bytes, String.GetBuffer(nCharacters));
			String.ReleaseBuffer(nCharacters);
		}
	}
	else
	{
		String.Empty();
	}
}

void DecodeUTF16ToJRString(const wchar_t * pUTF16, JRString & String, int nUTF16Bytes)
{
	if (pUTF16 != NULL)
	{
		if (nUTF16Bytes == -1)
		{
			String = pUTF16;
		}
		else
		{
			int nCharacters = nUTF16Bytes / sizeof(wchar_t);
			wchar_t * pOutput = String.GetBuffer(nCharacters + 1);
			memcpy(pOutput, pUTF16, nUTF16Bytes);
			pOutput[nCharacters] = 0;
			String.ReleaseBuffer();
		}
	}
	else
	{
		String.Empty();
	}
}
#endif

/******************************************************************************************
MFC Helpers
******************************************************************************************/
#ifdef _MFC_VER

void DecodeUTF8ToCString(const unsigned char * pUTF8, CString & String, int nUTF8Bytes)
{
	if (pUTF8 != NULL)
	{
		if (nUTF8Bytes == -1)
		{
			int nCharacters = GetUTF8Characters((unsigned char *) pUTF8);
			DecodeUTF8ToUTF16(pUTF8, String.GetBuffer(nCharacters));
			String.ReleaseBuffer(nCharacters);
		}
		else
		{
			int nCharacters = GetUTF8Characters((unsigned char *) pUTF8, nUTF8Bytes);
			DecodeUTF8ToUTF16(pUTF8, nUTF8Bytes, String.GetBuffer(nCharacters));
			String.ReleaseBuffer(nCharacters);
		}
	}
	else
	{
		String.Empty();
	}
}

void DecodeUTF16ToCString(const wchar_t * pUTF16, CString & String, int nUTF16Bytes)
{
	if (pUTF16 != NULL)
	{
		if (nUTF16Bytes == -1)
		{
			String = pUTF16;
		}
		else
		{
			int nCharacters = nUTF16Bytes / sizeof(wchar_t);
			wchar_t * pOutput = String.GetBuffer(nCharacters + 1);
			memcpy(pOutput, pUTF16, nUTF16Bytes);
			pOutput[nCharacters] = 0;
			String.ReleaseBuffer();
		}
	}
	else
	{
		String.Empty();
	}
}

#endif
