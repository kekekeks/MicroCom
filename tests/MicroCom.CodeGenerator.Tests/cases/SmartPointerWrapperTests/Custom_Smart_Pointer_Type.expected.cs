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
        IFoo Clone();
    }
}

namespace Test.Impl
{
    internal unsafe partial class __MicroComIFooProxy : global::MicroCom.Runtime.MicroComProxyBase, IFoo
    {
        public IFoo Clone()
        {
            void* __result;
            __result = (void*)((delegate* unmanaged[Stdcall]<void*, void*>)(*PPV)[base.VTableSize + 0])(PPV);
            return global::MicroCom.Runtime.MicroComRuntime.CreateProxyOrNullFor<IFoo>(__result, true);
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IFoo), new Guid("0b5a6a43-6d3e-4f3c-9d3a-7a2b0e0c1f11"), (p, owns) => new __MicroComIFooProxy(p, owns));
        }

        protected __MicroComIFooProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 1;
    }

    unsafe class __MicroComIFooVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate void* CloneDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void* Clone(void* @this)
        {
            IFoo __target = null;
            try
            {
                {
                    __target = (IFoo)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
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

        protected __MicroComIFooVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void*>)&Clone); 
#else
            base.AddMethod((CloneDelegate)Clone); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IFoo), new __MicroComIFooVTable().CreateVTable());
    }
}