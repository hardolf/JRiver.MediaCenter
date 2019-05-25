

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0603 */
/* at Tue Apr 10 14:02:15 2018
 */
/* Compiler settings for ImpactFill.idl:
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

#ifndef __ImpactFill_h__
#define __ImpactFill_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IImpactFill_FWD_DEFINED__
#define __IImpactFill_FWD_DEFINED__
typedef interface IImpactFill IImpactFill;

#endif 	/* __IImpactFill_FWD_DEFINED__ */


#ifndef __ImpactFill_FWD_DEFINED__
#define __ImpactFill_FWD_DEFINED__

#ifdef __cplusplus
typedef class ImpactFill ImpactFill;
#else
typedef struct ImpactFill ImpactFill;
#endif /* __cplusplus */

#endif 	/* __ImpactFill_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __IImpactFill_INTERFACE_DEFINED__
#define __IImpactFill_INTERFACE_DEFINED__

/* interface IImpactFill */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_IImpactFill;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("DC9DF95E-C7B5-4DC5-8756-0BED3B8B6ABC")
    IImpactFill : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Init( 
            /* [in] */ IDispatch *pMJ) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Terminate( void) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IImpactFillVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IImpactFill * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IImpactFill * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IImpactFill * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IImpactFill * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IImpactFill * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IImpactFill * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IImpactFill * This,
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
            IImpactFill * This,
            /* [in] */ IDispatch *pMJ);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Terminate )( 
            IImpactFill * This);
        
        END_INTERFACE
    } IImpactFillVtbl;

    interface IImpactFill
    {
        CONST_VTBL struct IImpactFillVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IImpactFill_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IImpactFill_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IImpactFill_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IImpactFill_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IImpactFill_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IImpactFill_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IImpactFill_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IImpactFill_Init(This,pMJ)	\
    ( (This)->lpVtbl -> Init(This,pMJ) ) 

#define IImpactFill_Terminate(This)	\
    ( (This)->lpVtbl -> Terminate(This) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IImpactFill_INTERFACE_DEFINED__ */



#ifndef __IMPACTFILLLib_LIBRARY_DEFINED__
#define __IMPACTFILLLib_LIBRARY_DEFINED__

/* library IMPACTFILLLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_IMPACTFILLLib;

EXTERN_C const CLSID CLSID_ImpactFill;

#ifdef __cplusplus

class DECLSPEC_UUID("DD362372-A4AD-466A-B497-6E93587CE070")
ImpactFill;
#endif
#endif /* __IMPACTFILLLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


