// Minimal COM environment for compiling generated headers in smart pointer wrappers mode
#pragma once

typedef unsigned int HRESULT;
typedef unsigned int ULONG;
#define S_OK 0x0L
#define E_NOINTERFACE 0x80004002L

struct IUnknown
{
    virtual HRESULT QueryInterface(const void* riid, void** ppvObject) = 0;
    virtual ULONG AddRef() = 0;
    virtual ULONG Release() = 0;
};

#define COMINTERFACE(name, d1, d2, d3, d41, d42, d43, d44, d45, d46, d47, d48) struct name

template<class T> class ComPtr
{
    T* _obj = nullptr;
public:
    ComPtr() = default;
    ComPtr(const ComPtr& other) : _obj(other._obj)
    {
        if (_obj)
            _obj->AddRef();
    }
    ComPtr& operator=(const ComPtr&) = delete;
    ~ComPtr()
    {
        if (_obj)
            _obj->Release();
    }

    T* operator->() const { return _obj; }
    T* get() const { return _obj; }

    void _microcom_attach(T* owned)
    {
        if (_obj)
            _obj->Release();
        _obj = owned;
    }

    T** _microcom_out()
    {
        _microcom_attach(nullptr);
        return &_obj;
    }

    T* _microcom_detach()
    {
        auto rv = _obj;
        _obj = nullptr;
        return rv;
    }
};
