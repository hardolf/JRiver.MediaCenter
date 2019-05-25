#ifndef GENERAL_READER_H
#define GENERAL_READER_H

/*
IReader is the interface specification for a generic IO object.
The name "...Reader" is used because it was originally a generic reader object
It is used to create specific readers such as a local file reader or a URL/Internet Reader,
or even a web server reader.

It also exports a GetGenericReader function (implemented in GeneralReaderImpl.h/cpp).
This takes a string which can be a file name or a URL, guesses which one it is, and
constructs the right kind of object.

All readers inherit from VBaseReader, which inherits from this. See the VBaseReader
class for more info.

The old General Reader interface is supported as a wrapper for the new stuff.
*/

// Reader Types:
enum ReaderType
{
	READER_UNDEFINED = -1,
	READER_LOCAL, 
	READER_INTERNET, 
	READER_MUSIC_TRANSCODER, 
	READER_STRING, 
	READER_TRUNCATING, 
	READER_TCP_SOCKET, 
	READER_UDP_SOCKET,
	READER_BUFFERED,
	READER_WRAPPER,
	READER_CIRCLING,
};

// GetInfo(...) defines
#define READER_INFO_CAN_TRUNCATE							_T("Can Truncate")
#define READER_INFO_LENGTH_ESTIMATE							_T("Length Estimate")
#define READER_INFO_TRANSCODING								_T("Transcoding")

#define READER_INFO_INET_ERROR								_T("Error")
#define READER_INFO_INET_HEADER_STATUS_CODE					_T("StatusCode")
#define READER_INFO_INET_HEADER_STATUS_TEXT					_T("StatusText")
#define READER_INFO_INET_HEADER_ALL							_T("AllHeaders")
#define READER_INFO_INET_HEADER_CONTENT_TYPE				_T("ContentType")
#define READER_INFO_INET_HEADER_CONTENT_ENCODING			_T("ContentEncoding")
#define READER_INFO_INET_HEADER_CONTENT_LENGTH				_T("ContentLength")
#define READER_INFO_INET_HEADER_LOCATION					_T("Location")

// SetOption(...) defines
#define READER_OPTION_READ_ONLY								_T("READ_ONLY")

#define READER_OPTION_INET_AUTH_USERNAME					_T("AUTH_USERNAME")
#define READER_OPTION_INET_AUTH_PASSWORD					_T("AUTH_PASSWORD")
#define READER_OPTION_INET_INFO_HEADERS						_T("INFO_HEADERS")
#define READER_OPTION_INET_CONNECT_PROXY					_T("CONNECT_PROXY")
#define READER_OPTION_INET_CONNECT_PROXYBYPASS				_T("CONNECT_PROXYBYPASS")
#define READER_OPTION_INET_HTTPREQUEST_TYPE					_T("HTTPREQUEST_TYPE")
#define READER_OPTION_INET_CONNECT_TIMEOUT					_T("CONNECT_TIMEOUT")
#define READER_OPTION_INET_USE_CACHE						_T("USE_CACHE")
#define READER_OPTION_INET_RANGE_START						_T("RANGE_START")
#define READER_OPTION_INET_RANGE_FINISH						_T("RANGE_FINISH")
#define READER_OPTION_INET_USER_AGENT						_T("USER_AGENT")
#define READER_OPTION_INET_SUCCEED_ON_404_ERROR				_T("SUCCEED_ON_404_ERROR")

/*************************************************************************************
IReader
*************************************************************************************/
class IReader
{
public:

	IReader() {};
	virtual ~IReader() {};

	// get the actual type of the reader
	virtual ReaderType GetType() = 0;

	// open / close
	virtual BOOL Create() = 0;
	virtual BOOL Open() = 0;
	virtual void Close() = 0;

	// read / write from a buffer
	virtual DWORD Read(void * pBuffer, DWORD BytesToRead) = 0;
	virtual DWORD Write(const void * pBuffer, DWORD BytesToWrite) = 0;

	// position and size
	virtual INT64 GetLength() = 0;
	virtual INT64 GetPosition() = 0;
	virtual INT64 SetPosition(INT64 Distance, UINT MoveMethod = FILE_BEGIN) = 0;
	virtual BOOL SetEndOfFile() = 0;
	virtual BOOL SetMaxBytes(INT64 MaxBytes) = 0;

	// query
	virtual BOOL GetIsStream() = 0;
	virtual BOOL GetIsRemote() = 0;
	virtual BOOL GetIsValid() = 0;
	virtual BOOL GetIsReadOnly() = 0;
	virtual BOOL GetInfo(LPCTSTR pName, BSTR *pResult) = 0;

	// options
	virtual void SetTimeout(DWORD nTimeout) = 0;
	virtual void SetOptionalData(LPVOID pOptionalData, DWORD nDataLen) = 0;
	virtual void SetOptions(LPCTSTR pMap) = 0;
	virtual void SetOption(LPCTSTR OptionName, LPCTSTR OptionValue) = 0;
	virtual void SetDeleteWhenDone(BOOL DeleteWhenDone = TRUE) = 0;

	// new functions (must add to end to maintain interface compatibility)
};

/*************************************************************************************
IGeneralReader (backwards-compatibility interface)
*************************************************************************************/
class IGeneralReader
{
public:

	IGeneralReader() {};
	virtual ~IGeneralReader() {};

	virtual BOOL Open(LPCTSTR pURL) = 0;
	virtual DWORD Read(void * pBuffer, DWORD BytesToRead) = 0;
	virtual DWORD Write(const void * pBuffer, DWORD BytesToWrite) = 0;
	virtual void Close() = 0;

	virtual DWORD GetLength() = 0;
	virtual DWORD GetPosition() = 0;
	virtual DWORD SetPosition(INT Distance, UINT MoveMethod = FILE_BEGIN) = 0;
	virtual DWORD SetEndOfFile() = 0;

	virtual BOOL GetIsStream() = 0;
	virtual BOOL GetIsRemote() = 0;
	virtual BOOL GetIsSecure() = 0;
	virtual BOOL GetIsValid() = 0;
	virtual BOOL GetIsReadOnly() = 0;
	// DEPRECATED: Do not use GetFileHandle, it doesn't work.
	virtual HANDLE GetFileHandle() = 0;

	virtual DWORD GetInfo(LPCTSTR pHeader, LPTSTR pRetVal, DWORD nRetValBytes) = 0;
};

// DLL_EXPORT may not be defined for plug-ins
#ifndef DLL_EXPORT
#define DLL_EXPORT __declspec(dllexport)
#endif

extern "C"
{
	DLL_EXPORT BOOL JRReaderInitialize(BOOL bInsideJRReader);
	typedef BOOL (* JRReaderInitializeFunc)(BOOL bInsideJRReader);
	DLL_EXPORT BOOL JRReaderUninitialize(BOOL bInsideJRReader);
	typedef BOOL (* JRReaderUninitializeFunc)(BOOL bInsideJRReader);
	DLL_EXPORT IReader * GetGenericReader(LPCTSTR ResourceName);
	typedef IReader * (* GetGenericReaderFunc)(LPCTSTR ResourceName);
	DLL_EXPORT IGeneralReader * GetGeneralReader();
	typedef IGeneralReader * (* GetGeneralReaderFunc)();
}

#endif
