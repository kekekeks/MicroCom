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
    internal unsafe partial interface IAvnString : global::MicroCom.Runtime.IUnknown
    {
        int Length();
    }

    internal unsafe partial interface IAvnTopLevel : global::MicroCom.Runtime.IUnknown
    {
        void SetTitle(int value);
        IAvnString Name
        {
            get;
        }
    }

    internal unsafe partial interface IAvnWindowBase : IAvnTopLevel
    {
        void Show();
        IAvnString Text
        {
            get;
        }
    }

    internal unsafe partial interface IAvnWindow : IAvnWindowBase
    {
        void SetEnabled(int enabled);
    }
}

namespace Test.Impl
{
    internal unsafe partial class __MicroComIAvnStringProxy : global::MicroCom.Runtime.MicroComProxyBase, IAvnString
    {
        public int Length()
        {
            int __result;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, int>)(*PPV)[base.VTableSize + 0])(PPV);
            return __result;
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IAvnString), new Guid("0b5a6a43-6d3e-4f3c-9d3a-7a2b0e0c1f21"), (p, owns) => new __MicroComIAvnStringProxy(p, owns));
        }

        protected __MicroComIAvnStringProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 1;
    }

    unsafe class __MicroComIAvnStringVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int LengthDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int Length(void* @this)
        {
            IAvnString __target = null;
            try
            {
                {
                    __target = (IAvnString)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.Length();
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

        protected __MicroComIAvnStringVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int>)&Length); 
#else
            base.AddMethod((LengthDelegate)Length); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IAvnString), new __MicroComIAvnStringVTable().CreateVTable());
    }

    internal unsafe partial class __MicroComIAvnTopLevelProxy : global::MicroCom.Runtime.MicroComProxyBase, IAvnTopLevel
    {
        public void SetTitle(int value)
        {
            int __result;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, int, int>)(*PPV)[base.VTableSize + 0])(PPV, value);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("SetTitle failed", __result);
        }

        public IAvnString Name
        {
            get
            {
                int __result;
                void* __marshal_ppv = null;
                __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 1])(PPV, &__marshal_ppv);
                if (__result != 0)
                    throw new System.Runtime.InteropServices.COMException("GetName failed", __result);
                return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IAvnString>(__marshal_ppv, true);
            }
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IAvnTopLevel), new Guid("0b5a6a43-6d3e-4f3c-9d3a-7a2b0e0c1f22"), (p, owns) => new __MicroComIAvnTopLevelProxy(p, owns));
        }

        protected __MicroComIAvnTopLevelProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 2;
    }

    unsafe class __MicroComIAvnTopLevelVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int SetTitleDelegate(void* @this, int value);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int SetTitle(void* @this, int value)
        {
            IAvnTopLevel __target = null;
            try
            {
                {
                    __target = (IAvnTopLevel)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.SetTitle(value);
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
        delegate int GetNameDelegate(void* @this, void** ppv);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int GetName(void* @this, void** ppv)
        {
            IAvnTopLevel __target = null;
            try
            {
                {
                    __target = (IAvnTopLevel)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.Name;
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

        protected __MicroComIAvnTopLevelVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int, int>)&SetTitle); 
#else
            base.AddMethod((SetTitleDelegate)SetTitle); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void**, int>)&GetName); 
#else
            base.AddMethod((GetNameDelegate)GetName); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IAvnTopLevel), new __MicroComIAvnTopLevelVTable().CreateVTable());
    }

    internal unsafe partial class __MicroComIAvnWindowBaseProxy : __MicroComIAvnTopLevelProxy, IAvnWindowBase
    {
        public void Show()
        {
            int __result;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, int>)(*PPV)[base.VTableSize + 0])(PPV);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("Show failed", __result);
        }

        public IAvnString Text
        {
            get
            {
                void* __result;
                __result = (void*)((delegate* unmanaged[Stdcall]<void*, void*>)(*PPV)[base.VTableSize + 1])(PPV);
                return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IAvnString>(__result, true);
            }
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IAvnWindowBase), new Guid("0b5a6a43-6d3e-4f3c-9d3a-7a2b0e0c1f23"), (p, owns) => new __MicroComIAvnWindowBaseProxy(p, owns));
        }

        protected __MicroComIAvnWindowBaseProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 2;
    }

    unsafe class __MicroComIAvnWindowBaseVTable : __MicroComIAvnTopLevelVTable
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int ShowDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int Show(void* @this)
        {
            IAvnWindowBase __target = null;
            try
            {
                {
                    __target = (IAvnWindowBase)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.Show();
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
        delegate void* GetTextDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void* GetText(void* @this)
        {
            IAvnWindowBase __target = null;
            try
            {
                {
                    __target = (IAvnWindowBase)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.Text;
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

        protected __MicroComIAvnWindowBaseVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int>)&Show); 
#else
            base.AddMethod((ShowDelegate)Show); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void*>)&GetText); 
#else
            base.AddMethod((GetTextDelegate)GetText); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IAvnWindowBase), new __MicroComIAvnWindowBaseVTable().CreateVTable());
    }

    internal unsafe partial class __MicroComIAvnWindowProxy : __MicroComIAvnWindowBaseProxy, IAvnWindow
    {
        public void SetEnabled(int enabled)
        {
            int __result;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, int, int>)(*PPV)[base.VTableSize + 0])(PPV, enabled);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("SetEnabled failed", __result);
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IAvnWindow), new Guid("0b5a6a43-6d3e-4f3c-9d3a-7a2b0e0c1f24"), (p, owns) => new __MicroComIAvnWindowProxy(p, owns));
        }

        protected __MicroComIAvnWindowProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 1;
    }

    unsafe class __MicroComIAvnWindowVTable : __MicroComIAvnWindowBaseVTable
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int SetEnabledDelegate(void* @this, int enabled);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int SetEnabled(void* @this, int enabled)
        {
            IAvnWindow __target = null;
            try
            {
                {
                    __target = (IAvnWindow)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.SetEnabled(enabled);
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

        protected __MicroComIAvnWindowVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int, int>)&SetEnabled); 
#else
            base.AddMethod((SetEnabledDelegate)SetEnabled); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IAvnWindow), new __MicroComIAvnWindowVTable().CreateVTable());
    }
}