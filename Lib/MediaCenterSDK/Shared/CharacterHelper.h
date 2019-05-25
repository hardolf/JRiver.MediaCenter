/*******************************************************************************************
Character set conversion helpers
*******************************************************************************************/

#ifndef CHARACTER_HELPER_H
#define CHARACTER_HELPER_H

// ANSI
char * GetANSIFromUTF8(const unsigned char * pUTF8, UINT CodePage = CP_ACP);
char * GetANSIFromUTF16(const wchar_t * pUTF16, UINT CodePage = CP_ACP);

// UTF-16
wchar_t * GetUTF16FromANSI(const char * pANSI);
wchar_t * GetUTF16FromUTF8(const unsigned char * pUTF8);

// UTF-8
unsigned char * GetUTF8FromANSI(const char * pANSI);
unsigned char * GetUTF8FromUTF16(const wchar_t * pUTF16, int nCharacters = -1);

// UTF-8 Helpers
int GetUTF8CharacterBytes(const unsigned char * pUTF8, int nInvalidReturn = 1);
int GetUTF8Bytes(const wchar_t * pUTF16, BOOL bAccountForNullTerminator = FALSE, int nCharacters = -1);
int GetUTF8Characters(const unsigned char * pUTF8, int nUTF8Bytes = -1);
void DecodeUTF16ToUTF8(const wchar_t * pUTF16, unsigned char * pUTF8, BOOL bNullTerminate = TRUE, int nCharacters = -1);
void DecodeUTF8ToUTF16(const unsigned char * pUTF8, wchar_t * pUTF16, BOOL bNullTerminate = TRUE);
void DecodeUTF8ToUTF16(const unsigned char * pUTF8, int nUTF8Bytes, wchar_t * pUTF16, BOOL bNullTerminate = TRUE);

// JRString Helpers
#ifdef JRFC_H
void DecodeUTF8ToJRString(const unsigned char * pUTF8, JRString & String, int nUTF8Bytes = -1);
void DecodeUTF16ToJRString(const wchar_t * pUTF16, JRString & String, int nUTF16Bytes = -1);
#endif

// MFC Helpers
#ifdef _MFC_VER
void DecodeUTF8ToCString(const unsigned char * pUTF8, CString & String, int nUTF8Bytes = -1);
void DecodeUTF16ToCString(const wchar_t * pUTF16, CString & String, int nUTF16Bytes = -1);
#endif

#endif // CHARACTER_HELPER_H
