struct IFoo;
struct IBar;
COMINTERFACE(IFoo, 12345678, 1234, 1234, 12, 34, 12, 34, 56, 78, 9a, bc) : IUnknown
{
    virtual void Bar (
        IFoo* foo, 
        IBar* bar, 
        int x
    ) = 0;
    virtual HRESULT TryGetBar (
        IBar** bar
    ) = 0;
    virtual HRESULT GetBar (
        IBar** bar
    ) = 0;
};
COMINTERFACE(IBar, 12345678, 1234, 1234, 12, 34, 12, 34, 56, 78, 9a, bd) : IUnknown
{
};
