struct ISimpleInterface;
COMINTERFACE(ISimpleInterface, 5b695a79, 8357, 498e, 9f, a0, e1, ea, 3e, 47, ab, 6b) : IUnknown
{
    virtual void DoWork () = 0;
    virtual int GetValue () = 0;
};
