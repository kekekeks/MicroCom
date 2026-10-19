# MicroCom

IDL-based COM interop codegen.
Consumes MIDL-like IDL file, generates efficient cross-platform C# interop and C++ header files.


## C++ smart pointer wrappers

Opt-in per IDL file via `CppSmartPointerWrappers` item metadata (`--cpp-smart-pointer-wrappers` for the CLI):

```xml
<MicroComIdl Include="avn.idl" CppHeaderPath="avn.h" CppSmartPointerWrappers="true" />
```

Methods that use interface pointers (returns, `IFoo*` arguments, `IFoo**` out-arguments) keep their vtable slots, but are renamed to `impl_*`. Interfaces get non-virtual wrappers with the original names:

```cpp
virtual IFoo* impl_GetFoo() = 0;                      // vtable slot
ComPtr<IFoo> GetFoo();                                // wrapper
virtual HRESULT impl_GetBar(IBar** ppv) = 0;
HRESULT GetBar(ComPtr<IBar>& ppv);
virtual HRESULT impl_SetBaz(IBaz* baz) = 0;
HRESULT SetBaz(const ComPtr<IBaz>& baz);
```

Every interface also gets a `MicroComImpl<IFoo>` adapter that implements `impl_*` methods by forwarding to pure virtual smart pointer variants. The adapter holds a reference to the object itself and to every interface argument for the duration of the call, so they stay alive even if the callee drops the last external reference (e.g. from a managed callback). Implementations should inherit from `MicroComImpl<IFoo>` (virtually, if the interface is used with `cpp-virtual-inherits`), never from a raw interface: a raw interface base can make the compiler emit NULL vtable slots for inherited methods.

`@cpp-call-preamble` inserts code at the beginning of every adapter call and makes all methods wrapped, not only the ones using interface pointers:

```
@cpp-call-preamble START_ARP_CALL;
```

The smart pointer template (`ComPtr` by default, `CppSmartPointerType` metadata or `--cpp-smart-pointer-type` to change) must be defined before the generated header, e.g. via `@cpp-preamble`, and provide:

- a default constructor
- `void _microcom_attach(T* obj, bool owns)`: stores `obj`, calling `AddRef` unless `owns` is true, and releases the previous value
- `T* _microcom_get() const`: returns the stored pointer
- `T** _microcom_out()`: releases the current value, returns the address to receive an owned pointer
- `T* _microcom_detach()`: returns the owned pointer without `Release` and clears the smart pointer
