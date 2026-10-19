template<class T> struct MicroComImpl;
#ifndef MICROCOM_IMPL_BASE_DEFINED
#define MICROCOM_IMPL_BASE_DEFINED
// Non-virtual primary base for adapters, so they never take over the primary base of an interface (Itanium ABI)
template<class T> struct MicroComImplBase
{
    virtual ~MicroComImplBase() = default;
};
#endif
struct IAvnString;
struct IAvnTopLevel;
struct IAvnWindowBase;
struct IAvnWindow;
COMINTERFACE(IAvnString, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 21) : IUnknown
{
    virtual int Length () = 0;
};
COMINTERFACE(IAvnTopLevel, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 22) : IUnknown
{
    virtual HRESULT SetTitle (
        int value
    ) = 0;
    virtual HRESULT impl_GetName (
        IAvnString** ppv
    ) = 0;
    HRESULT GetName (
        ComPtr<IAvnString>& ppv
    );
};
COMINTERFACE(IAvnWindowBase, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 23) : virtual IAvnTopLevel
{
    virtual HRESULT Show () = 0;
    virtual IAvnString* impl_GetText () = 0;
    ComPtr<IAvnString> GetText ();
};
COMINTERFACE(IAvnWindow, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 24) : virtual IAvnWindowBase
{
    virtual HRESULT SetEnabled (
        int enabled
    ) = 0;
};
inline HRESULT IAvnTopLevel::GetName (
    ComPtr<IAvnString>& ppv
)
{
    return impl_GetName(ppv._microcom_out());
}
inline ComPtr<IAvnString> IAvnWindowBase::GetText ()
{
    ComPtr<IAvnString> __microcom_rv;
    __microcom_rv._microcom_attach(impl_GetText(), true);
    return __microcom_rv;
}
template<>
struct MicroComImpl<IAvnString> : public MicroComImplBase<IAvnString>, public IAvnString
{
};
template<>
struct MicroComImpl<IAvnTopLevel> : public MicroComImplBase<IAvnTopLevel>, public virtual IAvnTopLevel
{
    virtual HRESULT GetName (
        ComPtr<IAvnString>& ppv
    ) = 0;
    HRESULT impl_GetName (
        IAvnString** ppv
    ) final
    {
        ComPtr<IAvnTopLevel> __microcom_self;
        __microcom_self._microcom_attach(static_cast<IAvnTopLevel*>(this), false);
        ComPtr<IAvnString> __microcom_ppv;
        auto __microcom_r = GetName(__microcom_ppv);
        if (ppv) *ppv = __microcom_ppv._microcom_detach();
        return __microcom_r;
    }
};
template<>
struct MicroComImpl<IAvnWindowBase> : public MicroComImplBase<IAvnWindowBase>, public virtual IAvnWindowBase, public virtual MicroComImpl<IAvnTopLevel>
{
    virtual ComPtr<IAvnString> GetText () = 0;
    IAvnString* impl_GetText () final
    {
        ComPtr<IAvnWindowBase> __microcom_self;
        __microcom_self._microcom_attach(static_cast<IAvnWindowBase*>(this), false);
        return GetText()._microcom_detach();
    }
};
template<>
struct MicroComImpl<IAvnWindow> : public MicroComImplBase<IAvnWindow>, public IAvnWindow, public virtual MicroComImpl<IAvnWindowBase>
{
};
