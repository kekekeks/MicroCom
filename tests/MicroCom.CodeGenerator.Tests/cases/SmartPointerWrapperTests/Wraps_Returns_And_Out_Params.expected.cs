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
    internal unsafe partial interface IFoo : global::MicroCom.Runtime.IUnknown
    {
        int Value
        {
            get;
        }
    }

    internal unsafe partial interface IHost : global::MicroCom.Runtime.IUnknown
    {
        /// <summary>Returns a foo by index</summary>
        /// <param name="index">Foo index</param>
        IFoo GetFoo(int index);
        IFoo CreateFoo(string name);
        IFoo GetFooAndHost(IHost host);
        void FillFoo(IFoo ppv);
        IUnknown AsUnknown();
        void SetFoo(IFoo foo);
        void GetCount(int* count);
    }
}

namespace Test.Impl
{
    internal unsafe partial class __MicroComIFooProxy : global::MicroCom.Runtime.MicroComProxyBase, IFoo
    {
        public int Value
        {
            get
            {
                int __result;
                __result = (int)((delegate* unmanaged[Stdcall]<void*, int>)(*PPV)[base.VTableSize + 0])(PPV);
                return __result;
            }
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IFoo), new Guid("0b5a6a43-6d3e-4f3c-9d3a-7a2b0e0c1f01"), (p, owns) => new __MicroComIFooProxy(p, owns));
        }

        protected __MicroComIFooProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 1;
    }

    unsafe class __MicroComIFooVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int GetValueDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int GetValue(void* @this)
        {
            IFoo __target = null;
            try
            {
                {
                    __target = (IFoo)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.Value;
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

        protected __MicroComIFooVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int>)&GetValue); 
#else
            base.AddMethod((GetValueDelegate)GetValue); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IFoo), new __MicroComIFooVTable().CreateVTable());
    }

    internal unsafe partial class __MicroComIHostProxy : global::MicroCom.Runtime.MicroComProxyBase, IHost
    {
        public IFoo GetFoo(int index)
        {
            void* __result;
            __result = (void*)((delegate* unmanaged[Stdcall]<void*, int, void*>)(*PPV)[base.VTableSize + 0])(PPV, index);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IFoo>(__result, true);
        }

        public IFoo CreateFoo(string name)
        {
            int __result;
            var __bytemarshal_name = new byte[System.Text.Encoding.UTF8.GetByteCount(name) + 1];
            System.Text.Encoding.UTF8.GetBytes(name, 0, name.Length, __bytemarshal_name, 0);
            void* __marshal_ppv = null;
            fixed (byte* __fixedmarshal_name = __bytemarshal_name)
                __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, void*, int>)(*PPV)[base.VTableSize + 1])(PPV, __fixedmarshal_name, &__marshal_ppv);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("CreateFoo failed", __result);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IFoo>(__marshal_ppv, true);
        }

        public IFoo GetFooAndHost(IHost host)
        {
            void* __result;
            __result = (void*)((delegate* unmanaged[Stdcall]<void*, void*, void*>)(*PPV)[base.VTableSize + 2])(PPV, &__marshal_host);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IFoo>(__result, true);
        }

        public void FillFoo(IFoo ppv)
        {
            ((delegate* unmanaged[Stdcall]<void*, void*, void>)(*PPV)[base.VTableSize + 3])(PPV, &__marshal_ppv);
        }

        public IUnknown AsUnknown()
        {
            void* __result;
            __result = (void*)((delegate* unmanaged[Stdcall]<void*, void*>)(*PPV)[base.VTableSize + 4])(PPV);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IUnknown>(__result, true);
        }

        public void SetFoo(IFoo foo)
        {
            int __result;
            using var __foo = global::MicroCom.Runtime.MicroComRuntime.LeaseNativePointerForCall(foo);
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 5])(PPV, __foo.Pointer);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("SetFoo failed", __result);
        }

        public void GetCount(int* count)
        {
            int __result;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 6])(PPV, count);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("GetCount failed", __result);
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IHost), new Guid("0b5a6a43-6d3e-4f3c-9d3a-7a2b0e0c1f02"), (p, owns) => new __MicroComIHostProxy(p, owns));
        }

        protected __MicroComIHostProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 7;
    }

    unsafe class __MicroComIHostVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate void* GetFooDelegate(void* @this, int index);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void* GetFoo(void* @this, int index)
        {
            IHost __target = null;
            try
            {
                {
                    __target = (IHost)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.GetFoo(index);
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

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int CreateFooDelegate(void* @this, byte* name, void** ppv);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int CreateFoo(void* @this, byte* name, void** ppv)
        {
            IHost __target = null;
            try
            {
                {
                    __target = (IHost)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.CreateFoo((name == null ? null : System.Runtime.InteropServices.Marshal.PtrToStringAnsi(new IntPtr(name))));
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

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate void* GetFooAndHostDelegate(void* @this, void** host);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void* GetFooAndHost(void* @this, void** host)
        {
            IHost __target = null;
            try
            {
                {
                    __target = (IHost)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.GetFooAndHost(INVALID);
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

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate void FillFooDelegate(void* @this, void** ppv);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void FillFoo(void* @this, void** ppv)
        {
            IHost __target = null;
            try
            {
                {
                    __target = (IHost)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.FillFoo(INVALID);
                }
            }
            catch (System.Exception __exception__)
            {
                global::MicroCom.Runtime.MicroComRuntime.UnhandledException(__target, __exception__);
                ;
            }
        }

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate void* AsUnknownDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void* AsUnknown(void* @this)
        {
            IHost __target = null;
            try
            {
                {
                    __target = (IHost)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.AsUnknown();
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

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int SetFooDelegate(void* @this, void* foo);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int SetFoo(void* @this, void* foo)
        {
            IHost __target = null;
            try
            {
                {
                    __target = (IHost)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.SetFoo(global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IFoo>(foo, false));
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
        delegate int GetCountDelegate(void* @this, int* count);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int GetCount(void* @this, int* count)
        {
            IHost __target = null;
            try
            {
                {
                    __target = (IHost)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.GetCount(count);
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

        protected __MicroComIHostVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int, void*>)&GetFoo); 
#else
            base.AddMethod((GetFooDelegate)GetFoo); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, byte*, void**, int>)&CreateFoo); 
#else
            base.AddMethod((CreateFooDelegate)CreateFoo); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void**, void*>)&GetFooAndHost); 
#else
            base.AddMethod((GetFooAndHostDelegate)GetFooAndHost); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void**, void>)&FillFoo); 
#else
            base.AddMethod((FillFooDelegate)FillFoo); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void*>)&AsUnknown); 
#else
            base.AddMethod((AsUnknownDelegate)AsUnknown); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void*, int>)&SetFoo); 
#else
            base.AddMethod((SetFooDelegate)SetFoo); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int*, int>)&GetCount); 
#else
            base.AddMethod((GetCountDelegate)GetCount); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IHost), new __MicroComIHostVTable().CreateVTable());
    }
}