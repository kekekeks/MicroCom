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

namespace TestNamespace
{
    public unsafe partial interface IFoo : global::MicroCom.Runtime.IUnknown
    {
        void Bar(IFoo? foo, IBar bar, int x);
        IBar? TryGetBar();
        IBar Bar
        {
            get;
        }
    }

    public unsafe partial interface IBar : global::MicroCom.Runtime.IUnknown
    {
    }
}

namespace TestNamespace.Impl
{
    public unsafe partial class __MicroComIFooProxy : global::MicroCom.Runtime.MicroComProxyBase, IFoo
    {
        public void Bar(IFoo? foo, IBar bar, int x)
        {
            ((delegate* unmanaged[Stdcall]<void*, void*, void*, int, void>)(*PPV)[base.VTableSize + 0])(PPV, global::MicroCom.Runtime.MicroComRuntime.GetNativePointer(foo), global::MicroCom.Runtime.MicroComRuntime.GetNativePointer(bar), x);
        }

        public IBar? TryGetBar()
        {
            int __result;
            void* __marshal_bar = null;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 1])(PPV, &__marshal_bar);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("TryGetBar failed", __result);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IBar>(__marshal_bar, true);
        }

        public IBar Bar
        {
            get
            {
                int __result;
                void* __marshal_bar = null;
                __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 2])(PPV, &__marshal_bar);
                if (__result != 0)
                    throw new System.Runtime.InteropServices.COMException("GetBar failed", __result);
                return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IBar>(__marshal_bar, true);
            }
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IFoo), new Guid("12345678-1234-1234-1234-123456789abc"), (p, owns) => new __MicroComIFooProxy(p, owns));
        }

        protected __MicroComIFooProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 3;
    }

    unsafe class __MicroComIFooVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate void BarDelegate(void* @this, void* foo, void* bar, int x);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void Bar(void* @this, void* foo, void* bar, int x)
        {
            IFoo __target = null;
            try
            {
                {
                    __target = (IFoo)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.Bar(global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IFoo>(foo, false), global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IBar>(bar, false), x);
                }
            }
            catch (System.Exception __exception__)
            {
                global::MicroCom.Runtime.MicroComRuntime.UnhandledException(__target, __exception__);
                ;
            }
        }

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int TryGetBarDelegate(void* @this, void** bar);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int TryGetBar(void* @this, void** bar)
        {
            IFoo __target = null;
            try
            {
                {
                    __target = (IFoo)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.TryGetBar();
                        *bar = global::MicroCom.Runtime.MicroComRuntime.GetNativePointer(__result, true);
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
        delegate int GetBarDelegate(void* @this, void** bar);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int GetBar(void* @this, void** bar)
        {
            IFoo __target = null;
            try
            {
                {
                    __target = (IFoo)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.Bar;
                        *bar = global::MicroCom.Runtime.MicroComRuntime.GetNativePointer(__result, true);
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

        protected __MicroComIFooVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void*, void*, int, void>)&Bar); 
#else
            base.AddMethod((BarDelegate)Bar); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void**, int>)&TryGetBar); 
#else
            base.AddMethod((TryGetBarDelegate)TryGetBar); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void**, int>)&GetBar); 
#else
            base.AddMethod((GetBarDelegate)GetBar); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IFoo), new __MicroComIFooVTable().CreateVTable());
    }

    public unsafe partial class __MicroComIBarProxy : global::MicroCom.Runtime.MicroComProxyBase, IBar
    {
        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IBar), new Guid("12345678-1234-1234-1234-123456789abd"), (p, owns) => new __MicroComIBarProxy(p, owns));
        }

        protected __MicroComIBarProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 0;
    }

    unsafe class __MicroComIBarVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        protected __MicroComIBarVTable()
        {
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IBar), new __MicroComIBarVTable().CreateVTable());
    }
}