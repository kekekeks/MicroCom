using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace MicroCom.Runtime
{
    public unsafe class MicroComShadow : IDisposable
    {
        private readonly object _lock = new object();
        internal object SyncRoot => _lock;
        private readonly Dictionary<Type, IntPtr> _shadows = new Dictionary<Type, IntPtr>();
        private readonly Dictionary<IntPtr, Type> _backShadows = new Dictionary<IntPtr, Type>();
        private GCHandle? _handle;
        // This doesn't include references from C# codegen
        private volatile int _nativeRefCount;
        private volatile int _totalRefCount;
        private bool _referencedFromNative;
        internal IMicroComShadowContainer Target { get; }
        internal MicroComShadow(IMicroComShadowContainer target)
        {
            Target = target;
            Target.Shadow = this;
        }
        
        internal int QueryInterface(Ccw* ccw, Guid* guid, void** ppv)
        {
            if (MicroComRuntime.TryGetTypeForGuid(*guid, out var type))
                return QueryInterface(type, ppv);
            else if (*guid == MicroComRuntime.ManagedObjectInterfaceGuid)
            {
                Interlocked.Increment(ref _totalRefCount);
                *ppv = ccw;
                return 0;
            }
            else
                return unchecked((int)0x80004002u);
        }

        internal int QueryInterface(Type type, void** ppv)
        {
            if (!type.IsInstanceOfType(Target))
                return unchecked((int)0x80004002u);

            var rv = GetOrCreateNativePointer(type, ppv);
            if (rv == 0)
                AddRef((Ccw*)*ppv);
            return rv;
        }

        internal int GetOrCreateNativePointer(Type type, void** ppv)
        {
            if (!MicroComRuntime.GetVtableFor(type, out var vtable))
                return unchecked((int)0x80004002u);
            lock (_lock)
            {

                if (_shadows.TryGetValue(type, out var shadow))
                {
                    var targetCcw = (Ccw*)shadow;
                    *ppv = targetCcw;
                    return 0;
                }
                else
                {
                    var intPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Ccw>());
                    var targetCcw = (Ccw*)intPtr;
                    *targetCcw = default;
                    targetCcw->VTable = vtable;
                    if (_handle == null)
                        _handle = GCHandle.Alloc(this);
                    targetCcw->GcShadowHandle = GCHandle.ToIntPtr(_handle.Value);
#if LOG_CCW_CALLS
                    targetCcw->Guid = MicroComRuntime.GetGuidFor(type);
#endif
                    _shadows[type] = intPtr;
                    _backShadows[intPtr] = type;
                    *ppv = targetCcw;

                    return 0;
                }
            }
        }

        /// <summary>
        /// This doesn't trigger ReferencedFromNative
        /// </summary>
        internal void AddTransientCallRef()
        {
            Interlocked.Increment(ref _totalRefCount);
        }
        
        internal void RemoveTransientCallRef()
        {
            var cnt = Interlocked.Decrement(ref _totalRefCount);
            if (cnt == 0)
                FreeCcws();
        }

        internal int AddRef(Ccw* ccw)
        {
            var count = Interlocked.Increment(ref _totalRefCount);
            if (Interlocked.Increment(ref _nativeRefCount) == 1)
            {
                lock (_lock)
                {
                    try
                    {
                        _referencedFromNative = true;
                        Target.OnReferencedFromNative();
                    }
                    catch (Exception e)
                    {
                        MicroComRuntime.UnhandledException(Target, e);
                    }
                }
            }

            return count;
        }

        internal int Release(Ccw* ccw)
        {
            Interlocked.Decrement(ref _nativeRefCount);
            var cnt = Interlocked.Decrement(ref _totalRefCount);
            if (cnt == 0)
                return FreeCcws();

            return cnt;
        }

        int FreeCcws()
        {
            lock (_lock)
            {
                // Shadow somehow got resurrected (i. e. we've passed this object to native code from a different thread
                // or Dispose called from managed code when there are native references 
                if (_totalRefCount != 0)
                    return _totalRefCount;

                foreach (var shadow in _shadows) 
                    Marshal.FreeHGlobal(shadow.Value);
                _shadows.Clear();
                _backShadows.Clear();
                
                _handle?.Free();
                _handle = null;
                try
                {
                    if (_referencedFromNative)
                    {
                        _referencedFromNative = false;
                        Target.OnUnreferencedFromNative();
                    }
                }
                catch(Exception e)
                {
                    MicroComRuntime.UnhandledException(Target, e);
                }
                
            }

            return 0;
        }

        /*
         Needs to be called to support the following scenario:
         1) Object created
         2) Non-owning GetNativePointer obtained, shadow is created, CCW is created
         3) Native side has never called AddRef
         
         In that case the GC handle to the shadow object is still alive
         */
        
        public void Dispose() => FreeCcws();
    }
    
    [StructLayout(LayoutKind.Sequential)]
    struct Ccw
    {
        public IntPtr VTable;
        public IntPtr GcShadowHandle;
        #if LOG_CCW_CALLS
        public Guid Guid;
        #endif
        public MicroComShadow GetShadow() => (MicroComShadow)GCHandle.FromIntPtr(GcShadowHandle).Target;
    }
}
