template<class T> struct MicroComImpl;
#ifndef MICROCOM_IMPL_BASE_DEFINED
#define MICROCOM_IMPL_BASE_DEFINED
// Non-virtual primary base for adapters, so they never take over the primary base of an interface (Itanium ABI)
template<class T> struct MicroComImplBase
{
    virtual ~MicroComImplBase() = default;
};
#endif
struct IFoo;
struct IHost;
COMINTERFACE(IFoo, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 01) : IUnknown
{
    virtual int GetValue () = 0;
};
COMINTERFACE(IHost, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 02) : IUnknown
{
    /**
     * Returns a foo by index
     * @param index Foo index
     */
    virtual IFoo* impl_GetFoo (
        int index
    ) = 0;
    /**
     * Returns a foo by index
     * @param index Foo index
     */
    ComPtr<IFoo> GetFoo (
        int index
    );
    virtual HRESULT impl_CreateFoo (
        const char* name, 
        IFoo** ppv
    ) = 0;
    HRESULT CreateFoo (
        const char* name, 
        ComPtr<IFoo>& ppv
    );
    virtual IFoo* impl_GetFooAndHost (
        IHost** host
    ) = 0;
    ComPtr<IFoo> GetFooAndHost (
        ComPtr<IHost>& host
    );
    virtual void impl_FillFoo (
        IFoo** ppv
    ) = 0;
    void FillFoo (
        ComPtr<IFoo>& ppv
    );
    virtual IUnknown* impl_AsUnknown () = 0;
    ComPtr<IUnknown> AsUnknown ();
    virtual HRESULT impl_SetFoo (
        IFoo* foo
    ) = 0;
    HRESULT SetFoo (
        const ComPtr<IFoo>& foo
    );
    virtual HRESULT GetCount (
        int* count
    ) = 0;
};
inline ComPtr<IFoo> IHost::GetFoo (
    int index
)
{
    ComPtr<IFoo> __microcom_rv;
    __microcom_rv._microcom_attach(impl_GetFoo(index), true);
    return __microcom_rv;
}
inline HRESULT IHost::CreateFoo (
    const char* name, 
    ComPtr<IFoo>& ppv
)
{
    return impl_CreateFoo(name, ppv._microcom_out());
}
inline ComPtr<IFoo> IHost::GetFooAndHost (
    ComPtr<IHost>& host
)
{
    ComPtr<IFoo> __microcom_rv;
    __microcom_rv._microcom_attach(impl_GetFooAndHost(host._microcom_out()), true);
    return __microcom_rv;
}
inline void IHost::FillFoo (
    ComPtr<IFoo>& ppv
)
{
    impl_FillFoo(ppv._microcom_out());
}
inline ComPtr<IUnknown> IHost::AsUnknown ()
{
    ComPtr<IUnknown> __microcom_rv;
    __microcom_rv._microcom_attach(impl_AsUnknown(), true);
    return __microcom_rv;
}
inline HRESULT IHost::SetFoo (
    const ComPtr<IFoo>& foo
)
{
    return impl_SetFoo(foo._microcom_get());
}
template<>
struct MicroComImpl<IFoo> : public MicroComImplBase<IFoo>, public IFoo
{
};
template<>
struct MicroComImpl<IHost> : public MicroComImplBase<IHost>, public IHost
{
    /**
     * Returns a foo by index
     * @param index Foo index
     */
    virtual ComPtr<IFoo> GetFoo (
        int index
    ) = 0;
    virtual HRESULT CreateFoo (
        const char* name, 
        ComPtr<IFoo>& ppv
    ) = 0;
    virtual ComPtr<IFoo> GetFooAndHost (
        ComPtr<IHost>& host
    ) = 0;
    virtual void FillFoo (
        ComPtr<IFoo>& ppv
    ) = 0;
    virtual ComPtr<IUnknown> AsUnknown () = 0;
    virtual HRESULT SetFoo (
        const ComPtr<IFoo>& foo
    ) = 0;
    IFoo* impl_GetFoo (
        int index
    ) final
    {
        ComPtr<IHost> __microcom_self;
        __microcom_self._microcom_attach(static_cast<IHost*>(this), false);
        return GetFoo(index)._microcom_detach();
    }
    HRESULT impl_CreateFoo (
        const char* name, 
        IFoo** ppv
    ) final
    {
        ComPtr<IHost> __microcom_self;
        __microcom_self._microcom_attach(static_cast<IHost*>(this), false);
        ComPtr<IFoo> __microcom_ppv;
        auto __microcom_r = CreateFoo(name, __microcom_ppv);
        if (ppv) *ppv = __microcom_ppv._microcom_detach();
        return __microcom_r;
    }
    IFoo* impl_GetFooAndHost (
        IHost** host
    ) final
    {
        ComPtr<IHost> __microcom_self;
        __microcom_self._microcom_attach(static_cast<IHost*>(this), false);
        ComPtr<IHost> __microcom_host;
        auto __microcom_r = GetFooAndHost(__microcom_host)._microcom_detach();
        if (host) *host = __microcom_host._microcom_detach();
        return __microcom_r;
    }
    void impl_FillFoo (
        IFoo** ppv
    ) final
    {
        ComPtr<IHost> __microcom_self;
        __microcom_self._microcom_attach(static_cast<IHost*>(this), false);
        ComPtr<IFoo> __microcom_ppv;
        FillFoo(__microcom_ppv);
        if (ppv) *ppv = __microcom_ppv._microcom_detach();
    }
    IUnknown* impl_AsUnknown () final
    {
        ComPtr<IHost> __microcom_self;
        __microcom_self._microcom_attach(static_cast<IHost*>(this), false);
        return AsUnknown()._microcom_detach();
    }
    HRESULT impl_SetFoo (
        IFoo* foo
    ) final
    {
        ComPtr<IHost> __microcom_self;
        __microcom_self._microcom_attach(static_cast<IHost*>(this), false);
        ComPtr<IFoo> __microcom_foo;
        __microcom_foo._microcom_attach(foo, false);
        return SetFoo(__microcom_foo);
    }
};
