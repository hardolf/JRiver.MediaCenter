#ifndef SMARTPTR_H
#define SMARTPTR_H

// disable the operator -> on UDT warning
#pragma warning( push )
#pragma warning( disable : 4284 )

#define SMART_PTR_FLAG_DELETE		1
#define SMART_PTR_FLAG_ARRAY		2

#define SMART_PTR_FLAG_DEFAULTS		(SMART_PTR_FLAG_DELETE)

/*************************************************************************************************
CSmartPtr - a simple smart pointer class that can automatically initialize and free memory
	note: (doesn't do garbage collection / reference counting because of the many pitfalls)
*************************************************************************************************/
template <class TYPE> class CSmartPtr
{
public:

	// pointer (first data so pointer to object is the pointer)
	TYPE * m_pObject;

	// construction
	CSmartPtr()
	{
		m_nFlags = SMART_PTR_FLAG_DEFAULTS;
		m_pObject = NULL;
	}
	CSmartPtr(TYPE * a_pObject, BOOL a_bArray = FALSE, BOOL a_bDelete = TRUE)
	{
		m_nFlags = SMART_PTR_FLAG_DEFAULTS;
		m_pObject = NULL;
		Assign(a_pObject, a_bArray, a_bDelete);
	}

	// destruction
	~CSmartPtr()
	{
		Delete();
	}

	// assign
	inline void Assign(TYPE * a_pObject, BOOL a_bArray = FALSE, BOOL a_bDelete = TRUE)
	{
		Delete();

		m_nFlags = (a_bArray ? SMART_PTR_FLAG_ARRAY : 0) | (a_bDelete ? SMART_PTR_FLAG_DELETE : 0);
		m_pObject = a_pObject;
	}

	// delete
	inline void Delete()
	{
		if (m_pObject != NULL)
		{
			// store object and set to NULL first
			TYPE * pObject = m_pObject;
			m_pObject = NULL;

			// delete (after object is NULL so trying to delete again while in the destructor
			// will not cause a double deletion)
			if (m_nFlags & SMART_PTR_FLAG_DELETE)
			{
				if (m_nFlags & SMART_PTR_FLAG_ARRAY)
					delete [] pObject;
				else
					delete pObject;
			}
		}		
	}

	// flags
	inline void SetDelete(const BOOL a_bDelete)
	{
		if (a_bDelete)
			m_nFlags |= SMART_PTR_FLAG_DELETE;
		else
			m_nFlags &= ~SMART_PTR_FLAG_DELETE;
	}

	// pointer access
    inline TYPE * GetPtr() const { return m_pObject; }
    inline operator TYPE * () const { return m_pObject; }
    inline TYPE * operator ->() const { return m_pObject; }

	// declare assignment and copy constructor, but don't implement (compiler error if we try to use)
	// this way we can't carelessly use the smart pointer and drop memory, etc.
	CSmartPtr(const CSmartPtr<TYPE> & Copy);
	inline void * operator =(void *) const;

protected:

	// flags
	unsigned int m_nFlags;
};

#pragma warning( pop ) 

#endif // SMARTPTR_H
