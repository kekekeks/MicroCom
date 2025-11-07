using System;

namespace MicroCom.Runtime
{
    public unsafe ref struct MicroComDisposableNativeReferenceStruct
    {
        public IntPtr IntPtr { get; }
        public void* Pointer => (void*)_ptr;
        private IntPtr _ptr;
        private readonly bool _owned;

        public MicroComDisposableNativeReferenceStruct(IntPtr value)
        {
            _ptr = value;
            _owned = true;
        }

        /// <summary>
        /// We are using non-owning references for COM pointers extracted from RCWs.
        /// RCWs don't have a "we are reusing a CCW that might be destroyed before we pass it to the native side"
        /// problem, so it's safe to not AddRef/Release them.
        /// </summary>
        internal MicroComDisposableNativeReferenceStruct(IntPtr value, bool owned)
        {
            _ptr = value;
            _owned = owned;
        }

        public void Dispose()
        {
            if (_ptr != IntPtr.Zero)
            {
                if (_owned)
                {
                    var ppv = (void***)_ptr;
                    ((delegate* unmanaged[Stdcall]<void*, int>)(*ppv)[2])(ppv);
                }

                _ptr = IntPtr.Zero;
            }
        }
    }
}