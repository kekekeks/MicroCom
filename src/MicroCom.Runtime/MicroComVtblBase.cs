using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MicroCom.Runtime
{
    public unsafe class MicroComVtblBase
    {
        private List<IntPtr> _methods = new List<IntPtr>();
        
#if !NET5_0_OR_GREATER
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int AddRefDelegate(Ccw* ccw);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int QueryInterfaceDelegate(Ccw* ccw, Guid* guid, void** ppv);
#endif

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

#if NET5_0_OR_GREATER
        protected void AddMethod(void* m)
        {
            _methods.Add(new IntPtr(m));
        }
#else
        protected void AddMethod<TDelegate>(TDelegate d)
        {
            GCHandle.Alloc(d);
            _methods.Add(Marshal.GetFunctionPointerForDelegate(d));
        }
#endif 

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
        static int QueryInterface(Ccw* ccw, Guid* guid, void** ppv)
        {
            #if LOG_CCW_CALLS
            Console.WriteLine($"QueryInterface: {DebugHelpers.PrettyPrintGuid(*guid)} from {DebugHelpers.PrettyPrintCcw(ccw)}");
            #endif
            var rv = ccw->GetShadow().QueryInterface(ccw, guid, ppv);
            if (rv != 0)
                *ppv = null;
            #if LOG_CCW_CALLS
            Console.WriteLine($"QueryInterface return: {rv}, {DebugHelpers.PrettyPrintCcw((Ccw*)*ppv)}");
            #endif
            return rv;
        }
#if NET5_0_OR_GREATER
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
#endif
        static int AddRef(Ccw* ccw)
        {
#if LOG_CCW_CALLS
            Console.WriteLine($"AddRef: {DebugHelpers.PrettyPrintCcw(ccw)}\"");
#endif
            return ccw->GetShadow().AddRef(ccw);
        }
#if NET5_0_OR_GREATER
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
#endif
        static int Release(Ccw* ccw)
        {
#if LOG_CCW_CALLS
            Console.WriteLine($"Release: {DebugHelpers.PrettyPrintCcw(ccw)}\"");
#endif
            return ccw->GetShadow().Release(ccw);
        }
    }
}
