#ifndef _MYENCODERCP_H_
#define _MYENCODERCP_H_

template <class T>
class CProxy_IMyImplEvents : public IConnectionPointImpl<T, &IID__IMyImplEvents, CComDynamicUnkArray>
{
	//Warning this class may be recreated by the wizard.
public:
	HRESULT Fire_Failure(LONG error, BSTR * errstring)
	{
		HRESULT ret;
		T* pT = static_cast<T*>(this);
		int nConnectionIndex;
		int nConnections = m_vec.GetSize();
		
		for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)
		{
			pT->Lock();
			CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);
			pT->Unlock();
			_IMyImplEvents* p_IMyImplEvents = reinterpret_cast<_IMyImplEvents*>(sp.p);
			if (p_IMyImplEvents != NULL)
				ret = p_IMyImplEvents->Failure(error, errstring);
		}	return ret;
	
	}
	HRESULT Fire_Finished()
	{
		HRESULT ret;
		T* pT = static_cast<T*>(this);
		int nConnectionIndex;
		int nConnections = m_vec.GetSize();
		
		for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)
		{
			pT->Lock();
			CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);
			pT->Unlock();
			_IMyImplEvents* p_IMyImplEvents = reinterpret_cast<_IMyImplEvents*>(sp.p);
			if (p_IMyImplEvents != NULL)
				ret = p_IMyImplEvents->Finished();
		}	return ret;
	
	}
	HRESULT Fire_Progress(LONG position)
	{
		HRESULT ret;
		T* pT = static_cast<T*>(this);
		int nConnectionIndex;
		int nConnections = m_vec.GetSize();
		
		for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)
		{
			pT->Lock();
			CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);
			pT->Unlock();
			_IMyImplEvents* p_IMyImplEvents = reinterpret_cast<_IMyImplEvents*>(sp.p);
			if (p_IMyImplEvents != NULL)
				ret = p_IMyImplEvents->Progress(position);
		}	return ret;
	
	}
};
#endif
