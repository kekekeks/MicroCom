template<class T> struct MicroComImpl;
#ifndef MICROCOM_IMPL_BASE_DEFINED
#define MICROCOM_IMPL_BASE_DEFINED
// Non-virtual primary base for adapters, so they never take over the primary base of an interface (Itanium ABI)
template<class T> struct MicroComImplBase
{
    virtual ~MicroComImplBase() = default;
};
#endif
struct IValue;
struct ICallTarget;
COMINTERFACE(IValue, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 51) : IUnknown
{
    virtual int impl_Get () = 0;
    int Get ();
};
COMINTERFACE(ICallTarget, 0b5a6a43, 6d3e, 4f3c, 9d, 3a, 7a, 2b, 0e, 0c, 1f, 52) : IUnknown
{
    virtual int impl_GetNumber () = 0;
    int GetNumber ();
    virtual HRESULT impl_Close () = 0;
    HRESULT Close ();
    virtual int impl_Consume (
        IValue* value, 
        int extra
    ) = 0;
    int Consume (
        const ComPtr<IValue>& value, 
        int extra
    );
};
inline int IValue::Get ()
{
    return impl_Get();
}
inline int ICallTarget::GetNumber ()
{
    return impl_GetNumber();
}
inline HRESULT ICallTarget::Close ()
{
    return impl_Close();
}
inline int ICallTarget::Consume (
    const ComPtr<IValue>& value, 
    int extra
)
{
    return impl_Consume(value._microcom_get(), extra);
}
template<>
struct MicroComImpl<IValue> : public MicroComImplBase<IValue>, public IValue
{
    virtual int Get () = 0;
    int impl_Get () final
    {
        CALL_PREAMBLE;
        CALL_PREAMBLE_SECOND_LINE;
        ComPtr<IValue> __microcom_self;
        __microcom_self._microcom_attach(static_cast<IValue*>(this), false);
        return Get();
    }
};
template<>
struct MicroComImpl<ICallTarget> : public MicroComImplBase<ICallTarget>, public ICallTarget
{
    virtual int GetNumber () = 0;
    virtual HRESULT Close () = 0;
    virtual int Consume (
        const ComPtr<IValue>& value, 
        int extra
    ) = 0;
    int impl_GetNumber () final
    {
        CALL_PREAMBLE;
        CALL_PREAMBLE_SECOND_LINE;
        ComPtr<ICallTarget> __microcom_self;
        __microcom_self._microcom_attach(static_cast<ICallTarget*>(this), false);
        return GetNumber();
    }
    HRESULT impl_Close () final
    {
        CALL_PREAMBLE;
        CALL_PREAMBLE_SECOND_LINE;
        ComPtr<ICallTarget> __microcom_self;
        __microcom_self._microcom_attach(static_cast<ICallTarget*>(this), false);
        return Close();
    }
    int impl_Consume (
        IValue* value, 
        int extra
    ) final
    {
        CALL_PREAMBLE;
        CALL_PREAMBLE_SECOND_LINE;
        ComPtr<ICallTarget> __microcom_self;
        __microcom_self._microcom_attach(static_cast<ICallTarget*>(this), false);
        ComPtr<IValue> __microcom_value;
        __microcom_value._microcom_attach(value, false);
        return Consume(__microcom_value, extra);
    }
};
