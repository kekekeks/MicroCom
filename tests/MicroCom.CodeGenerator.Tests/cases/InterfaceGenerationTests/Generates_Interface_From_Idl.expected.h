struct ISimpleInterface;
COMINTERFACE(ISimpleInterface, 5b695a79, 8357, 498e, 9f, a0, e1, ea, 3e, 47, ab, 6b) : IUnknown
{
    virtual HRESULT GetValueHr (
        int* rv
    ) = 0;
    virtual void Generic (
        KeyValuePair<int, float> pair
    ) = 0;
    virtual KeyValuePair<int, float>* GetGenericValue () = 0;
    virtual HRESULT GetGenericValueHr (
        KeyValuePair<int, float>* pair
    ) = 0;
    virtual void DoWork () = 0;
    virtual int GetValue () = 0;
};
