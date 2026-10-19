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
COMINTERFACE(IFoo, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 11) : IUnknown
{
    virtual IFoo* impl_Clone () = 0;
    MyPtr<IFoo> Clone ();
};
inline MyPtr<IFoo> IFoo::Clone ()
{
    MyPtr<IFoo> __microcom_rv;
    __microcom_rv._microcom_attach(impl_Clone());
    return __microcom_rv;
}
template<>
struct MicroComImpl<IFoo> : public MicroComImplBase<IFoo>, public IFoo
{
    virtual MyPtr<IFoo> Clone () = 0;
    IFoo* impl_Clone () final
    {
        return Clone()._microcom_detach();
    }
};
