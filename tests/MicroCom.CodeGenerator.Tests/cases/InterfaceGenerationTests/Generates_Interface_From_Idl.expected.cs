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
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal unsafe partial struct Foo<T>
        where T : unmanaged
    {
        public T Value;
    }

    internal unsafe partial interface ISimpleInterface : global::MicroCom.Runtime.IUnknown
    {
        int ValueHr
        {
            get;
        }

        void Generic(KeyValuePair<int, float> pair);
        KeyValuePair<int, float>* GenericValue
        {
            get;
        }

        void GetGenericValueHr(KeyValuePair<int, float>* pair);
        void DoWork();
        int Value
        {
            get;
        }

        void CustomStruct(Foo<int> foo);
    }
}

namespace Test.Impl
{
    internal unsafe partial class __MicroComISimpleInterfaceProxy : global::MicroCom.Runtime.MicroComProxyBase, ISimpleInterface
    {
        public int ValueHr
        {
            get
            {
                int __result;
                int rv = default;
                __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 0])(PPV, &rv);
                if (__result != 0)
                    throw new System.Runtime.InteropServices.COMException("GetValueHr failed", __result);
                return rv;
            }
        }

        public void Generic(KeyValuePair<int, float> pair)
        {
            ((delegate* unmanaged[Stdcall]<void*, KeyValuePair<int, float>, void>)(*PPV)[base.VTableSize + 1])(PPV, pair);
        }

        public KeyValuePair<int, float>* GenericValue
        {
            get
            {
                KeyValuePair<int, float>* __result;
                __result = (KeyValuePair<int, float>*)((delegate* unmanaged[Stdcall]<void*, void*>)(*PPV)[base.VTableSize + 2])(PPV);
                return __result;
            }
        }

        public void GetGenericValueHr(KeyValuePair<int, float>* pair)
        {
            int __result;
            __result = (int)((delegate* unmanaged[Stdcall]<void*, void*, int>)(*PPV)[base.VTableSize + 3])(PPV, pair);
            if (__result != 0)
                throw new System.Runtime.InteropServices.COMException("GetGenericValueHr failed", __result);
        }

        public void DoWork()
        {
            ((delegate* unmanaged[Stdcall]<void*, void>)(*PPV)[base.VTableSize + 4])(PPV);
        }

        public int Value
        {
            get
            {
                int __result;
                __result = (int)((delegate* unmanaged[Stdcall]<void*, int>)(*PPV)[base.VTableSize + 5])(PPV);
                return __result;
            }
        }

        public void CustomStruct(Foo<int> foo)
        {
            ((delegate* unmanaged[Stdcall]<void*, Foo<int>, void>)(*PPV)[base.VTableSize + 6])(PPV, foo);
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(ISimpleInterface), new Guid("5b695a79-8357-498e-9fa0-e1ea3e47ab6b"), (p, owns) => new __MicroComISimpleInterfaceProxy(p, owns));
        }

        protected __MicroComISimpleInterfaceProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 7;
    }

    unsafe class __MicroComISimpleInterfaceVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int GetValueHrDelegate(void* @this, int* rv);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int GetValueHr(void* @this, int* rv)
        {
            ISimpleInterface __target = null;
            try
            {
                {
                    __target = (ISimpleInterface)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.ValueHr;
                        *rv = __result;
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
        delegate void GenericDelegate(void* @this, KeyValuePair<int, float> pair);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void Generic(void* @this, KeyValuePair<int, float> pair)
        {
            ISimpleInterface __target = null;
            try
            {
                {
                    __target = (ISimpleInterface)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.Generic(pair);
                }
            }
            catch (System.Exception __exception__)
            {
                global::MicroCom.Runtime.MicroComRuntime.UnhandledException(__target, __exception__);
                ;
            }
        }

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate KeyValuePair<int, float>* GetGenericValueDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static KeyValuePair<int, float>* GetGenericValue(void* @this)
        {
            ISimpleInterface __target = null;
            try
            {
                {
                    __target = (ISimpleInterface)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    {
                        var __result = __target.GenericValue;
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

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate int GetGenericValueHrDelegate(void* @this, KeyValuePair<int, float>* pair);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static int GetGenericValueHr(void* @this, KeyValuePair<int, float>* pair)
        {
            ISimpleInterface __target = null;
            try
            {
                {
                    __target = (ISimpleInterface)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.GetGenericValueHr(pair);
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

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate void CustomStructDelegate(void* @this, Foo<int> foo);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void CustomStruct(void* @this, Foo<int> foo)
        {
            ISimpleInterface __target = null;
            try
            {
                {
                    __target = (ISimpleInterface)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.CustomStruct(foo);
                }
            }
            catch (System.Exception __exception__)
            {
                global::MicroCom.Runtime.MicroComRuntime.UnhandledException(__target, __exception__);
                ;
            }
        }

        protected __MicroComISimpleInterfaceVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int*, int>)&GetValueHr); 
#else
            base.AddMethod((GetValueHrDelegate)GetValueHr); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, KeyValuePair<int, float>, void>)&Generic); 
#else
            base.AddMethod((GenericDelegate)Generic); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void*>)&GetGenericValue); 
#else
            base.AddMethod((GetGenericValueDelegate)GetGenericValue); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, KeyValuePair<int, float>*, int>)&GetGenericValueHr); 
#else
            base.AddMethod((GetGenericValueHrDelegate)GetGenericValueHr); 
#endif
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
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, Foo<int>, void>)&CustomStruct); 
#else
            base.AddMethod((CustomStructDelegate)CustomStruct); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(ISimpleInterface), new __MicroComISimpleInterfaceVTable().CreateVTable());
    }
}