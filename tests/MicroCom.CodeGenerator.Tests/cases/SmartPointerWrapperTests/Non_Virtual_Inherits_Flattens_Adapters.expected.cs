#pragma warning disable 108
// ReSharper disable RedundantUsingDirective
// ReSharper disable JoinDeclarationAndInitializer
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedType.Local
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantCast
// ReSharper disable IdentifierTypo
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantUnsafeContext
// ReSharper disable RedundantBaseQualifier
// ReSharper disable EmptyStatement
// ReSharper disable RedundantAttributeParentheses
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using MicroCom.Runtime;

namespace Test
{
    internal unsafe partial interface ILock : global::MicroCom.Runtime.IUnknown
    {
        void* Address
        {
            get;
        }
    }

    internal unsafe partial interface IImage : global::MicroCom.Runtime.IUnknown
    {
        void GetWidth(int* width);
        IImage Clone();
    }

    internal unsafe partial interface IBitmap : IImage
    {
        ILock Lock();
    }
}

namespace Test.Impl
{
    internal unsafe partial class __MicroComILockProxy : global::MicroCom.Runtime.MicroComProxyBase, ILock
    {
        public void* Address
        {
            get
            {
                void* __result;
                __result = (void*)((delegate* unmanaged[Stdcall]<void*, void*>)(*PPV)[base.VTableSize + 0])(PPV);
                return __result;
            }
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(ILock), new Guid("0b5a6a43-6d3e-4f3c-9d3a-7a2b0e0c1f31"), (p, owns) => new __MicroComILockProxy(p, owns));
        }

        protected __MicroComILockProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 1;
    }

    unsafe class __MicroComILockVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate void* GetAddressDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void* GetAddress(void* @this)
        {
            ILock __target = null;
            try
            {
                {
                    __target = (ILock)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.Address;
                        return __result;
                    }
                }
            }
            catch (System.Exception __exception__)
            {
                global::MicroCom.Runtime.MicroComRuntime.UnhandledException(__target, __exception__);
                return default;
            }
        }

        protected __MicroComILockVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void*>)&GetAddress); 
#else
            base.AddMethod((GetAddressDelegate)GetAddress); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(ILock), new __MicroComILockVTable().CreateVTable());
    }

    internal unsafe partial class __MicroComIImageProxy : global::MicroCom.Runtime.MicroComProxyBase, IImage
    {
        public void GetWidth(int* width)
        {
            int __result;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 0])(PPV, width);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("GetWidth failed", __result);
        }

        public IImage Clone()
        {
            void* __result;
            __result = (void*)((delegate* unmanaged[Stdcall]<void*, void*>)(*PPV)[base.VTableSize + 1])(PPV);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IImage>(__result, true);
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IImage), new Guid("0b5a6a43-6d3e-4f3c-9d3a-7a2b0e0c1f32"), (p, owns) => new __MicroComIImageProxy(p, owns));
        }

        protected __MicroComIImageProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 2;
    }

    unsafe class __MicroComIImageVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int GetWidthDelegate(void* @this, int* width);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int GetWidth(void* @this, int* width)
        {
            IImage __target = null;
            try
            {
                {
                    __target = (IImage)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.GetWidth(width);
                }
            }
            catch (System.Runtime.InteropServices.COMException __com_exception__)
            {
                return __com_exception__.ErrorCode;
            }
            catch (System.Exception __exception__)
            {
                global::MicroCom.Runtime.MicroComRuntime.UnhandledException(__target, __exception__);
                return unchecked((int)0x80004005u);
            }

            return 0;
        }

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate void* CloneDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void* Clone(void* @this)
        {
            IImage __target = null;
            try
            {
                {
                    __target = (IImage)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.Clone();
                        return global::MicroCom.Runtime.MicroComRuntime.GetNativePointer(__result, true);
                    }
                }
            }
            catch (System.Exception __exception__)
            {
                global::MicroCom.Runtime.MicroComRuntime.UnhandledException(__target, __exception__);
                return default;
            }
        }

        protected __MicroComIImageVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int*, int>)&GetWidth); 
#else
            base.AddMethod((GetWidthDelegate)GetWidth); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void*>)&Clone); 
#else
            base.AddMethod((CloneDelegate)Clone); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IImage), new __MicroComIImageVTable().CreateVTable());
    }

    internal unsafe partial class __MicroComIBitmapProxy : __MicroComIImageProxy, IBitmap
    {
        public ILock Lock()
        {
            int __result;
            void* __marshal_ppv = null;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 0])(PPV, &__marshal_ppv);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("Lock failed", __result);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<ILock>(__marshal_ppv, true);
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IBitmap), new Guid("0b5a6a43-6d3e-4f3c-9d3a-7a2b0e0c1f33"), (p, owns) => new __MicroComIBitmapProxy(p, owns));
        }

        protected __MicroComIBitmapProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 1;
    }

    unsafe class __MicroComIBitmapVTable : __MicroComIImageVTable
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int LockDelegate(void* @this, void** ppv);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int Lock(void* @this, void** ppv)
        {
            IBitmap __target = null;
            try
            {
                {
                    __target = (IBitmap)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.Lock();
                        *ppv = global::MicroCom.Runtime.MicroComRuntime.GetNativePointer(__result, true);
                    }
                }
            }
            catch (System.Runtime.InteropServices.COMException __com_exception__)
            {
                return __com_exception__.ErrorCode;
            }
            catch (System.Exception __exception__)
            {
                global::MicroCom.Runtime.MicroComRuntime.UnhandledException(__target, __exception__);
                return unchecked((int)0x80004005u);
            }

            return 0;
        }

        protected __MicroComIBitmapVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void**, int>)&Lock); 
#else
            base.AddMethod((LockDelegate)Lock); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IBitmap), new __MicroComIBitmapVTable().CreateVTable());
    }
}