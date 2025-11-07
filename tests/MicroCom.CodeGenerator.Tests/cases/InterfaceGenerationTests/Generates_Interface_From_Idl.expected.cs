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
    internal unsafe partial interface ISimpleInterface : global::MicroCom.Runtime.IUnknown
    {
        void DoWork();
        int Value
        {
            get;
        }
    }
}

namespace Test.Impl
{
    internal unsafe partial class __MicroComISimpleInterfaceProxy : global::MicroCom.Runtime.MicroComProxyBase, ISimpleInterface
    {
        public void DoWork()
        {
            ((delegate* unmanaged[Stdcall]<void*, void>)(*PPV)[base.VTableSize + 0])(PPV);
        }

        public int Value
        {
            get
            {
                int __result;
                __result = (int)((delegate* unmanaged[Stdcall]<void*, int>)(*PPV)[base.VTableSize + 1])(PPV);
                return __result;
            }
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(ISimpleInterface), new Guid("5b695a79-8357-498e-9fa0-e1ea3e47ab6b"), (p, owns) => new __MicroComISimpleInterfaceProxy(p, owns));
        }

        protected __MicroComISimpleInterfaceProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 2;
    }

    unsafe class __MicroComISimpleInterfaceVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate void DoWorkDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void DoWork(void* @this)
        {
            ISimpleInterface __target = null;
            try
            {
                {
                    __target = (ISimpleInterface)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.DoWork();
                }
            }
            catch (System.Exception __exception__)
            {
                global::MicroCom.Runtime.MicroComRuntime.UnhandledException(__target, __exception__);
                ;
            }
        }

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int GetValueDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int GetValue(void* @this)
        {
            ISimpleInterface __target = null;
            try
            {
                {
                    __target = (ISimpleInterface)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
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

        protected __MicroComISimpleInterfaceVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void>)&DoWork); 
#else
            base.AddMethod((DoWorkDelegate)DoWork); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int>)&GetValue); 
#else
            base.AddMethod((GetValueDelegate)GetValue); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(ISimpleInterface), new __MicroComISimpleInterfaceVTable().CreateVTable());
    }
}