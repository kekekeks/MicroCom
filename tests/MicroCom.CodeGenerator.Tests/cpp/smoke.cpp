// Implements generated MicroComImpl adapters and calls every COM slot raw, the way the C# side does
#include <cstdio>
#include <cstring>
#include "shim.h"
#include "Virtual_Inherits_Chains_Adapters.h"
#include "Non_Virtual_Inherits_Flattens_Adapters.h"
#include "Wraps_Returns_And_Out_Params.h"

static int g_preambleCalls = 0;
#define CALL_PREAMBLE g_preambleCalls++
#define CALL_PREAMBLE_SECOND_LINE (void)0
#include "Call_Preamble_And_Retention.h"

static int g_failures = 0;
static int g_live = 0;
static bool g_inCall = false;
static int g_destroyedInCall = 0;

#define CHECK(cond) do { if (!(cond)) { printf("%s:%d: CHECK failed: %s\n", __FILE__, __LINE__, #cond); g_failures++; } } while (0)

class ComObject : public virtual IUnknown
{
    ULONG _rc = 1;
public:
    ComObject() { g_live++; }
    virtual ~ComObject()
    {
        g_live--;
        if (g_inCall)
            g_destroyedInCall++;
    }
    ULONG AddRef() override { return ++_rc; }
    ULONG Release() override
    {
        auto rv = --_rc;
        if (rv == 0)
            delete this;
        return rv;
    }
    HRESULT QueryInterface(const void*, void**) override { return E_NOINTERFACE; }
    ULONG RefCount() const { return _rc; }
};

#define FORWARD_IUNKNOWN() \
    ULONG AddRef() override { return ComObject::AddRef(); } \
    ULONG Release() override { return ComObject::Release(); } \
    HRESULT QueryInterface(const void* riid, void** ppv) override { return ComObject::QueryInterface(riid, ppv); }

template<class R, class... A> R Slot(void* itf, int slot, A... args)
{
    auto vtable = *(void***)itf;
    return ((R (*)(void*, A...))vtable[slot])(itf, args...);
}

static bool CheckSlotsNotNull(const char* name, void* itf, int count)
{
    auto vtable = *(void***)itf;
    bool rv = true;
    for (int c = 0; c < count; c++)
    {
        if (!vtable[c])
        {
            printf("%s: slot %d is NULL\n", name, c);
            g_failures++;
            rv = false;
        }
    }
    return rv;
}

template<class T, class TOwner> static T* Make()
{
    auto rv = new TOwner();
    T* itf = rv;
    return itf;
}

// Virtual inheritance chain (Avalonia shape)

class StringImpl : public ComObject, public MicroComImpl<IAvnString>
{
    int _length;
public:
    FORWARD_IUNKNOWN()
    explicit StringImpl(int length) : _length(length) {}
    int Length() override { return _length; }
};

class TopLevelImpl : public virtual ComObject, public virtual MicroComImpl<IAvnTopLevel>
{
public:
    FORWARD_IUNKNOWN()
    HRESULT SetTitle(int value) override { return 1000 + value; }
    HRESULT GetName(ComPtr<IAvnString>& ppv) override
    {
        ppv._microcom_attach(new StringImpl(4), true);
        return S_OK;
    }
};

class WindowBaseImpl : public virtual TopLevelImpl, public virtual MicroComImpl<IAvnWindowBase>
{
public:
    HRESULT Show() override { return 1005; }
    ComPtr<IAvnString> GetText() override
    {
        ComPtr<IAvnString> rv;
        rv._microcom_attach(new StringImpl(6), true);
        return rv;
    }
};

class WindowImpl : public virtual WindowBaseImpl, public virtual MicroComImpl<IAvnWindow>
{
public:
    HRESULT SetEnabled(int enabled) override { return 1007 + enabled; }
};

static void CheckTopLevel(const char* name, void* itf)
{
    CHECK(Slot<HRESULT>(itf, 3, 3) == 1003u);
    IAvnString* str = nullptr;
    CHECK(Slot<HRESULT>(itf, 4, &str) == S_OK);
    CHECK(str != nullptr);
    if (str)
    {
        CHECK(Slot<int>(str, 3) == 4);
        CHECK(Slot<ULONG>(str, 2) == 0u);
    }
    CHECK(Slot<HRESULT>(itf, 4, (IAvnString**)nullptr) == S_OK);
    (void)name;
}

static void CheckWindowBase(const char* name, void* itf)
{
    CheckTopLevel(name, itf);
    CHECK(Slot<HRESULT>(itf, 5) == 1005u);
    auto str = Slot<IAvnString*>(itf, 6);
    CHECK(str != nullptr);
    if (str)
    {
        CHECK(Slot<int>(str, 3) == 6);
        CHECK(Slot<ULONG>(str, 2) == 0u);
    }
}

static void TestVirtualChain()
{
    auto window = new WindowImpl();
    IAvnTopLevel* topLevel = window;
    IAvnWindowBase* windowBase = window;
    IAvnWindow* iwindow = window;

    auto slotsOk = CheckSlotsNotNull("IAvnTopLevel", topLevel, 5);
    slotsOk &= CheckSlotsNotNull("IAvnWindowBase", windowBase, 7);
    slotsOk &= CheckSlotsNotNull("IAvnWindow", iwindow, 8);
    if (!slotsOk)
    {
        window->Release();
        return;
    }

    CHECK(Slot<ULONG>(iwindow, 1) == 2u);
    CHECK(Slot<ULONG>(windowBase, 2) == 1u);

    CheckTopLevel("IAvnTopLevel", topLevel);
    CheckWindowBase("IAvnWindowBase", windowBase);
    CheckWindowBase("IAvnWindow", iwindow);
    CHECK(Slot<HRESULT>(iwindow, 7, 1) == 1008u);

    {
        ComPtr<IAvnString> name;
        CHECK(iwindow->GetName(name) == S_OK);
        CHECK(name->Length() == 4);
        CHECK(iwindow->GetName(name) == S_OK);
        auto text = iwindow->GetText();
        CHECK(text->Length() == 6);
        CHECK(g_live == 3);
    }
    CHECK(g_live == 1);
    CHECK(window->RefCount() == 1u);
    window->Release();
}

// Non-virtual inheritance, including a duplicate base interface copy

class LockImpl : public ComObject, public MicroComImpl<ILock>
{
public:
    FORWARD_IUNKNOWN()
    void* GetAddress() override { return this; }
};

class BitmapImpl : public ComObject, public MicroComImpl<IBitmap>, public MicroComImpl<IImage>
{
public:
    FORWARD_IUNKNOWN()
    HRESULT GetWidth(int* width) override
    {
        *width = 42;
        return S_OK;
    }
    ComPtr<IImage> Clone() override
    {
        ComPtr<IImage> rv;
        rv._microcom_attach(static_cast<MicroComImpl<IImage>*>(new BitmapImpl()), true);
        return rv;
    }
    HRESULT Lock(ComPtr<ILock>& ppv) override
    {
        ppv._microcom_attach(Make<ILock, LockImpl>(), true);
        return S_OK;
    }
};

static void CheckImage(const char* name, void* itf)
{
    if (!CheckSlotsNotNull(name, itf, 5))
        return;
    int width = 0;
    CHECK(Slot<HRESULT>(itf, 3, &width) == S_OK);
    CHECK(width == 42);
    auto clone = Slot<IImage*>(itf, 4);
    CHECK(clone != nullptr);
    if (clone)
        CHECK(Slot<ULONG>(clone, 2) == 0u);
}

static void TestNonVirtual()
{
    auto bitmap = new BitmapImpl();
    IBitmap* ibitmap = bitmap;
    IImage* imageFromBitmap = ibitmap;
    IImage* imageCopy = static_cast<MicroComImpl<IImage>*>(bitmap);
    CHECK((void*)imageFromBitmap != (void*)imageCopy);

    if (!CheckSlotsNotNull("IBitmap", ibitmap, 6))
        return;
    CheckImage("IImage (IBitmap)", imageFromBitmap);
    CheckImage("IImage (copy)", imageCopy);

    ILock* lock = nullptr;
    CHECK(Slot<HRESULT>(ibitmap, 5, &lock) == S_OK);
    CHECK(lock != nullptr);
    if (lock)
    {
        CheckSlotsNotNull("ILock", lock, 4);
        CHECK(Slot<ULONG>(lock, 2) == 0u);
    }

    {
        ComPtr<ILock> l;
        CHECK(ibitmap->Lock(l) == S_OK);
        auto clone = imageCopy->Clone();
        CHECK(g_live == 3);
    }
    CHECK(g_live == 1);
    bitmap->Release();
}

// Wrapper shapes

class FooImpl : public ComObject, public MicroComImpl<IFoo>
{
    int _value;
public:
    FORWARD_IUNKNOWN()
    explicit FooImpl(int value) : _value(value) {}
    int GetValue() override { return _value; }
};

static ComPtr<IFoo> NewFoo(int value)
{
    ComPtr<IFoo> rv;
    rv._microcom_attach(new FooImpl(value), true);
    return rv;
}

class HostImpl : public ComObject, public MicroComImpl<IHost>
{
public:
    FORWARD_IUNKNOWN()
    ComPtr<IFoo> GetFoo(int index) override { return NewFoo(index); }
    HRESULT CreateFoo(const char* name, ComPtr<IFoo>& ppv) override
    {
        ppv._microcom_attach(NewFoo((int)strlen(name))._microcom_detach(), true);
        return S_OK;
    }
    ComPtr<IFoo> GetFooAndHost(ComPtr<IHost>& host) override
    {
        host._microcom_attach(new HostImpl(), true);
        return NewFoo(7);
    }
    void FillFoo(ComPtr<IFoo>& ppv) override { ppv._microcom_attach(new FooImpl(8), true); }
    ComPtr<IUnknown> AsUnknown() override
    {
        ComPtr<IUnknown> rv;
        AddRef();
        rv._microcom_attach(static_cast<ComObject*>(this), true);
        return rv;
    }
    HRESULT SetFoo(const ComPtr<IFoo>& foo) override { return foo->GetValue(); }
    HRESULT GetCount(int* count) override
    {
        *count = 9;
        return S_OK;
    }
};

static void TestWrappers()
{
    auto host = Make<IHost, HostImpl>();
    if (!CheckSlotsNotNull("IHost", host, 10))
        return;

    auto foo = Slot<IFoo*>(host, 3, 5);
    CHECK(Slot<int>(foo, 3) == 5);
    CHECK(Slot<HRESULT>(host, 8, foo) == 5u);
    Slot<ULONG>(foo, 2);

    foo = nullptr;
    CHECK(Slot<HRESULT>(host, 4, "abc", &foo) == S_OK);
    CHECK(foo && Slot<int>(foo, 3) == 3);
    if (foo)
        Slot<ULONG>(foo, 2);

    IHost* otherHost = nullptr;
    foo = Slot<IFoo*>(host, 5, &otherHost);
    CHECK(foo && Slot<int>(foo, 3) == 7);
    CHECK(otherHost != nullptr);
    if (foo)
        Slot<ULONG>(foo, 2);
    if (otherHost)
        Slot<ULONG>(otherHost, 2);

    Slot<void>(host, 6, (IFoo**)nullptr);
    foo = nullptr;
    Slot<void>(host, 6, &foo);
    CHECK(foo && Slot<int>(foo, 3) == 8);
    if (foo)
        Slot<ULONG>(foo, 2);

    auto unk = Slot<IUnknown*>(host, 7);
    CHECK(unk != nullptr);
    if (unk)
        CHECK(Slot<ULONG>(unk, 2) == 1u);

    int count = 0;
    CHECK(Slot<HRESULT>(host, 9, &count) == S_OK && count == 9);

    {
        auto f = host->GetFoo(11);
        CHECK(f->GetValue() == 11);
        ComPtr<IFoo> created;
        CHECK(host->CreateFoo("abcd", created) == S_OK);
        CHECK(created->GetValue() == 4);
        ComPtr<IHost> other;
        auto f2 = host->GetFooAndHost(other);
        CHECK(f2->GetValue() == 7 && other.get() != nullptr);
        host->FillFoo(created);
        CHECK(created->GetValue() == 8);
        auto u = host->AsUnknown();
        CHECK(u.get() != nullptr);
        CHECK(host->SetFoo(f) == 11u);
    }
    CHECK(g_live == 1);
    host->Release();
}

// Call preamble, self and argument retention

class ValueImpl : public ComObject, public MicroComImpl<IValue>
{
public:
    FORWARD_IUNKNOWN()
    int Get() override { return 21; }
};

static IValue* g_valueToDrop = nullptr;

class CallTargetImpl : public ComObject, public MicroComImpl<ICallTarget>
{
    int _number = 5;
public:
    FORWARD_IUNKNOWN()
    int GetNumber() override { return _number; }

    // Simulates a C# callback dropping the last external reference while the call is in progress
    HRESULT Close() override
    {
        g_inCall = true;
        Release();
        g_inCall = false;
        return _number == 5 ? S_OK : E_NOINTERFACE;
    }

    int Consume(const ComPtr<IValue>& value, int extra) override
    {
        g_inCall = true;
        g_valueToDrop->Release();
        g_inCall = false;
        return value->Get() + extra;
    }
};

static void TestRetention()
{
    auto target = Make<ICallTarget, CallTargetImpl>();
    if (!CheckSlotsNotNull("ICallTarget", target, 6))
        return;

    CHECK(Slot<int>(target, 3) == 5);
    CHECK(g_preambleCalls == 1);
    CHECK(target->GetNumber() == 5);
    CHECK(g_preambleCalls == 2);

    auto value = Make<IValue, ValueImpl>();
    g_valueToDrop = value;
    CHECK(Slot<int>(target, 5, value, 1) == 22);
    CHECK(g_destroyedInCall == 0);
    CHECK(g_live == 1);

    CHECK(Slot<HRESULT>(target, 4) == S_OK);
    CHECK(g_destroyedInCall == 0);
    CHECK(g_live == 0);
    // GetNumber twice, Consume, IValue::Get inside Consume, Close
    CHECK(g_preambleCalls == 5);
}

int main()
{
    setvbuf(stdout, nullptr, _IONBF, 0);
    TestVirtualChain();
    TestNonVirtual();
    TestWrappers();
    TestRetention();
    if (g_failures == 0)
        CHECK(g_live == 0);
    if (g_failures == 0)
        printf("OK\n");
    return g_failures == 0 ? 0 : 1;
}
