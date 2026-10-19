template<class T> struct MicroComImpl;
#ifndef MICROCOM_IMPL_BASE_DEFINED
#define MICROCOM_IMPL_BASE_DEFINED
// Non-virtual primary base for adapters, so they never take over the primary base of an interface (Itanium ABI)
template<class T> struct MicroComImplBase
{
    virtual ~MicroComImplBase() = default;
};
#endif
struct ILock;
struct IImage;
struct IBitmap;
COMINTERFACE(ILock, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 31) : IUnknown
{
    virtual void* GetAddress () = 0;
};
COMINTERFACE(IImage, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 32) : IUnknown
{
    virtual HRESULT GetWidth (
        int* width
    ) = 0;
    virtual IImage* impl_Clone () = 0;
    ComPtr<IImage> Clone ();
};
COMINTERFACE(IBitmap, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 33) : IImage
{
    virtual HRESULT impl_Lock (
        ILock** ppv
    ) = 0;
    HRESULT Lock (
        ComPtr<ILock>& ppv
    );
};
inline ComPtr<IImage> IImage::Clone ()
{
    ComPtr<IImage> __microcom_rv;
    __microcom_rv._microcom_attach(impl_Clone());
    return __microcom_rv;
}
inline HRESULT IBitmap::Lock (
    ComPtr<ILock>& ppv
)
{
    return impl_Lock(ppv._microcom_out());
}
template<>
struct MicroComImpl<ILock> : public MicroComImplBase<ILock>, public ILock
{
};
template<>
struct MicroComImpl<IImage> : public MicroComImplBase<IImage>, public IImage
{
    virtual ComPtr<IImage> Clone () = 0;
    IImage* impl_Clone () final
    {
        return Clone()._microcom_detach();
    }
};
template<>
struct MicroComImpl<IBitmap> : public MicroComImplBase<IBitmap>, public IBitmap
{
    virtual HRESULT Lock (
        ComPtr<ILock>& ppv
    ) = 0;
    virtual ComPtr<IImage> Clone () = 0;
    HRESULT impl_Lock (
        ILock** ppv
    ) final
    {
        ComPtr<ILock> __microcom_ppv;
        auto __microcom_r = Lock(__microcom_ppv);
        if (ppv) *ppv = __microcom_ppv._microcom_detach();
        return __microcom_r;
    }
    IImage* impl_Clone () final
    {
        return Clone()._microcom_detach();
    }
};
