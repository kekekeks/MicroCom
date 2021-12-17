using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicroCom.Runtime
{
    public unsafe class MicroComVtblBase
    {
        private List<IntPtr> _methods = new List<IntPtr>();
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int AddRefDelegate(Ccw* ccw);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int QueryInterfaceDelegate(Ccw* ccw, Guid* guid, void** ppv);

        public static IntPtr Vtable { get; } = new MicroComVtblBase().CreateVTable();
        public MicroComVtblBase()
        {
#if NET5_0_OR_GREATER
            AddMethod((delegate* unmanaged[Stdcall]<Ccw*, Guid*, void**, int>)&QueryInterface);
            AddMethod((delegate* unmanaged[Stdcall]<Ccw*, int>)&AddRef);
            AddMethod((delegate* unmanaged[Stdcall]<Ccw*, int>)&Release);
#else
            AddMethod((QueryInterfaceDelegate)QueryInterface);
            AddMethod((AddRefDelegate)AddRef);
            AddMethod((AddRefDelegate)Release);
#endif
        }

        protected void AddMethod(Delegate d)
        {
            GCHandle.Alloc(d);
            _methods.Add(Marshal.GetFunctionPointerForDelegate(d));
        }
        
        protected void AddMethod(void* m)
        {
            _methods.Add(new IntPtr(m));
        }

        protected unsafe IntPtr CreateVTable()
        {
            var ptr = (IntPtr*)Marshal.AllocHGlobal((IntPtr.Size + 1) * _methods.Count);
            for (var c = 0; c < _methods.Count; c++)
                ptr[c] = _methods[c];
            return new IntPtr(ptr);
        }

#if NET5_0_OR_GREATER
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
#endif
        static int QueryInterface(Ccw* ccw, Guid* guid, void** ppv) => ccw->GetShadow().QueryInterface(ccw, guid, ppv);
#if NET5_0_OR_GREATER
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
#endif
        static int AddRef(Ccw* ccw) => ccw->GetShadow().AddRef(ccw);
#if NET5_0_OR_GREATER
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
#endif
        static int Release(Ccw* ccw) => ccw->GetShadow().Release(ccw);
    }
}
