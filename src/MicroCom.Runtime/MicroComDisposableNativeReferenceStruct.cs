using System;

namespace MicroCom.Runtime
{
    public unsafe ref struct MicroComDisposableNativeReferenceStruct
    {
        public IntPtr IntPtr { get; }
        public void* Pointer => (void*)_ptr;
        private IntPtr _ptr;
        
        public MicroComDisposableNativeReferenceStruct()
        {
            _ptr = IntPtr.Zero;
        }

        public void Dispose()
        {
            if (_ptr != IntPtr.Zero)
            {
                var ppv = (void***)_ptr;
                ((delegate* unmanaged[Stdcall]<void*, int>)(*ppv)[2])(ppv);
                _ptr = IntPtr.Zero;
            }
        }
    }
}