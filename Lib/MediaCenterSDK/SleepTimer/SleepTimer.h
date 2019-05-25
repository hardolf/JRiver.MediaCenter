

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0603 */
/* at Tue Apr 10 13:52:56 2018
 */
/* Compiler settings for SleepTimer.idl:
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

#ifndef __SleepTimer_h__
#define __SleepTimer_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __ISleepTimerCtrl_FWD_DEFINED__
#define __ISleepTimerCtrl_FWD_DEFINED__
typedef interface ISleepTimerCtrl ISleepTimerCtrl;

#endif 	/* __ISleepTimerCtrl_FWD_DEFINED__ */


#ifndef __SleepTimerCtrl_FWD_DEFINED__
#define __SleepTimerCtrl_FWD_DEFINED__

#ifdef __cplusplus
typedef class SleepTimerCtrl SleepTimerCtrl;
#else
typedef struct SleepTimerCtrl SleepTimerCtrl;
#endif /* __cplusplus */

#endif 	/* __SleepTimerCtrl_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __ISleepTimerCtrl_INTERFACE_DEFINED__
#define __ISleepTimerCtrl_INTERFACE_DEFINED__

/* interface ISleepTimerCtrl */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_ISleepTimerCtrl;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("E12D9E7F-B55D-4F87-904A-2DE8E261DC3F")
    ISleepTimerCtrl : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Init( 
            /* [in] */ LPDISPATCH pDisp) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct ISleepTimerCtrlVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ISleepTimerCtrl * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ISleepTimerCtrl * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ISleepTimerCtrl * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ISleepTimerCtrl * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ISleepTimerCtrl * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ISleepTimerCtrl * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ISleepTimerCtrl * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Init )( 
            ISleepTimerCtrl * This,
            /* [in] */ LPDISPATCH pDisp);
        
        END_INTERFACE
    } ISleepTimerCtrlVtbl;

    interface ISleepTimerCtrl
    {
        CONST_VTBL struct ISleepTimerCtrlVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISleepTimerCtrl_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ISleepTimerCtrl_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ISleepTimerCtrl_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ISleepTimerCtrl_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ISleepTimerCtrl_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ISleepTimerCtrl_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ISleepTimerCtrl_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ISleepTimerCtrl_Init(This,pDisp)	\
    ( (This)->lpVtbl -> Init(This,pDisp) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ISleepTimerCtrl_INTERFACE_DEFINED__ */



#ifndef __SLEEPTIMERLib_LIBRARY_DEFINED__
#define __SLEEPTIMERLib_LIBRARY_DEFINED__

/* library SLEEPTIMERLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_SLEEPTIMERLib;

EXTERN_C const CLSID CLSID_SleepTimerCtrl;

#ifdef __cplusplus

class DECLSPEC_UUID("1F277DBB-337E-4C61-B936-DE1105CBA323")
SleepTimerCtrl;
#endif
#endif /* __SLEEPTIMERLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


