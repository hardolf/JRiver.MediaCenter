

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0603 */
/* at Tue Apr 10 13:49:01 2018
 */
/* Compiler settings for MyEncoder.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 8.00.0603 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 500
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __MyEncoder_h__
#define __MyEncoder_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IMyImpl_FWD_DEFINED__
#define __IMyImpl_FWD_DEFINED__
typedef interface IMyImpl IMyImpl;

#endif 	/* __IMyImpl_FWD_DEFINED__ */


#ifndef ___IMyImplEvents_FWD_DEFINED__
#define ___IMyImplEvents_FWD_DEFINED__
typedef interface _IMyImplEvents _IMyImplEvents;

#endif 	/* ___IMyImplEvents_FWD_DEFINED__ */


#ifndef ___IMyBufferInputImpl_FWD_DEFINED__
#define ___IMyBufferInputImpl_FWD_DEFINED__
typedef interface _IMyBufferInputImpl _IMyBufferInputImpl;

#endif 	/* ___IMyBufferInputImpl_FWD_DEFINED__ */


#ifndef __MyImpl_FWD_DEFINED__
#define __MyImpl_FWD_DEFINED__

#ifdef __cplusplus
typedef class MyImpl MyImpl;
#else
typedef struct MyImpl MyImpl;
#endif /* __cplusplus */

#endif 	/* __MyImpl_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __IMyImpl_INTERFACE_DEFINED__
#define __IMyImpl_INTERFACE_DEFINED__

/* interface IMyImpl */
/* [unique][helpstring][uuid][object] */ 


EXTERN_C const IID IID_IMyImpl;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("F4A6C190-1072-471C-81CF-6BA2E0B878C8")
    IMyImpl : public IUnknown
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Start( 
            long *error) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Stop( void) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Options( 
            long *error) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_DestinationFile( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_DestinationFile( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_SourceFile( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_SourceFile( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_QualityList( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Quality( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_Quality( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetName( 
            BSTR Name) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IMyImplVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IMyImpl * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IMyImpl * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IMyImpl * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Start )( 
            IMyImpl * This,
            long *error);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Stop )( 
            IMyImpl * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Options )( 
            IMyImpl * This,
            long *error);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DestinationFile )( 
            IMyImpl * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DestinationFile )( 
            IMyImpl * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SourceFile )( 
            IMyImpl * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_SourceFile )( 
            IMyImpl * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_QualityList )( 
            IMyImpl * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Quality )( 
            IMyImpl * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Quality )( 
            IMyImpl * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetName )( 
            IMyImpl * This,
            BSTR Name);
        
        END_INTERFACE
    } IMyImplVtbl;

    interface IMyImpl
    {
        CONST_VTBL struct IMyImplVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IMyImpl_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IMyImpl_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IMyImpl_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IMyImpl_Start(This,error)	\
    ( (This)->lpVtbl -> Start(This,error) ) 

#define IMyImpl_Stop(This)	\
    ( (This)->lpVtbl -> Stop(This) ) 

#define IMyImpl_Options(This,error)	\
    ( (This)->lpVtbl -> Options(This,error) ) 

#define IMyImpl_get_DestinationFile(This,pVal)	\
    ( (This)->lpVtbl -> get_DestinationFile(This,pVal) ) 

#define IMyImpl_put_DestinationFile(This,newVal)	\
    ( (This)->lpVtbl -> put_DestinationFile(This,newVal) ) 

#define IMyImpl_get_SourceFile(This,pVal)	\
    ( (This)->lpVtbl -> get_SourceFile(This,pVal) ) 

#define IMyImpl_put_SourceFile(This,newVal)	\
    ( (This)->lpVtbl -> put_SourceFile(This,newVal) ) 

#define IMyImpl_get_QualityList(This,pVal)	\
    ( (This)->lpVtbl -> get_QualityList(This,pVal) ) 

#define IMyImpl_get_Quality(This,pVal)	\
    ( (This)->lpVtbl -> get_Quality(This,pVal) ) 

#define IMyImpl_put_Quality(This,newVal)	\
    ( (This)->lpVtbl -> put_Quality(This,newVal) ) 

#define IMyImpl_SetName(This,Name)	\
    ( (This)->lpVtbl -> SetName(This,Name) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IMyImpl_INTERFACE_DEFINED__ */



#ifndef __MYENCODERLib_LIBRARY_DEFINED__
#define __MYENCODERLib_LIBRARY_DEFINED__

/* library MYENCODERLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_MYENCODERLib;

#ifndef ___IMyImplEvents_INTERFACE_DEFINED__
#define ___IMyImplEvents_INTERFACE_DEFINED__

/* interface _IMyImplEvents */
/* [object][helpstring][uuid] */ 


EXTERN_C const IID IID__IMyImplEvents;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("0035F4B8-FD31-49EA-A51A-B4FC5BB23E56")
    _IMyImplEvents : public IUnknown
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Finished( void) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Failure( 
            long error,
            BSTR *errstring) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Progress( 
            long position) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct _IMyImplEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _IMyImplEvents * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _IMyImplEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _IMyImplEvents * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Finished )( 
            _IMyImplEvents * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Failure )( 
            _IMyImplEvents * This,
            long error,
            BSTR *errstring);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Progress )( 
            _IMyImplEvents * This,
            long position);
        
        END_INTERFACE
    } _IMyImplEventsVtbl;

    interface _IMyImplEvents
    {
        CONST_VTBL struct _IMyImplEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _IMyImplEvents_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _IMyImplEvents_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _IMyImplEvents_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _IMyImplEvents_Finished(This)	\
    ( (This)->lpVtbl -> Finished(This) ) 

#define _IMyImplEvents_Failure(This,error,errstring)	\
    ( (This)->lpVtbl -> Failure(This,error,errstring) ) 

#define _IMyImplEvents_Progress(This,position)	\
    ( (This)->lpVtbl -> Progress(This,position) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ___IMyImplEvents_INTERFACE_DEFINED__ */


#ifndef ___IMyBufferInputImpl_INTERFACE_DEFINED__
#define ___IMyBufferInputImpl_INTERFACE_DEFINED__

/* interface _IMyBufferInputImpl */
/* [object][helpstring][uuid] */ 


EXTERN_C const IID IID__IMyBufferInputImpl;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("39045225-0E7F-4367-A75A-5E86B5E1B380")
    _IMyBufferInputImpl : public IUnknown
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE InitBufferInput( 
            void *pWaveFormat,
            BOOL bRawWaveData,
            long approximateTotalSize,
            long *error) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE EncodeBuffer( 
            unsigned char *buffer,
            long buflen,
            long *error) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE FinishBufferInput( void) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct _IMyBufferInputImplVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _IMyBufferInputImpl * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _IMyBufferInputImpl * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _IMyBufferInputImpl * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *InitBufferInput )( 
            _IMyBufferInputImpl * This,
            void *pWaveFormat,
            BOOL bRawWaveData,
            long approximateTotalSize,
            long *error);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *EncodeBuffer )( 
            _IMyBufferInputImpl * This,
            unsigned char *buffer,
            long buflen,
            long *error);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *FinishBufferInput )( 
            _IMyBufferInputImpl * This);
        
        END_INTERFACE
    } _IMyBufferInputImplVtbl;

    interface _IMyBufferInputImpl
    {
        CONST_VTBL struct _IMyBufferInputImplVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _IMyBufferInputImpl_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _IMyBufferInputImpl_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _IMyBufferInputImpl_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _IMyBufferInputImpl_InitBufferInput(This,pWaveFormat,bRawWaveData,approximateTotalSize,error)	\
    ( (This)->lpVtbl -> InitBufferInput(This,pWaveFormat,bRawWaveData,approximateTotalSize,error) ) 

#define _IMyBufferInputImpl_EncodeBuffer(This,buffer,buflen,error)	\
    ( (This)->lpVtbl -> EncodeBuffer(This,buffer,buflen,error) ) 

#define _IMyBufferInputImpl_FinishBufferInput(This)	\
    ( (This)->lpVtbl -> FinishBufferInput(This) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* ___IMyBufferInputImpl_INTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_MyImpl;

#ifdef __cplusplus

class DECLSPEC_UUID("5950775D-D626-4615-BB01-572E409B1F9E")
MyImpl;
#endif
#endif /* __MYENCODERLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long *, BSTR * ); 

unsigned long             __RPC_USER  BSTR_UserSize64(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal64(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal64(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree64(     unsigned long *, BSTR * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


