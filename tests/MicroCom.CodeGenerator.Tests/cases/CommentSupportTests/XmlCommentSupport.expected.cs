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
    /// <summary>
    /// Test enum with XML comments.
    /// </summary>
    internal enum XmlCommentEnum
    {
        /// <summary>
        /// First value.
        /// </summary>
        First = 0,
        /// <summary>
        /// Second value.
        /// </summary>
        Second = 1,
        /// <summary>
        /// Third value.
        /// </summary>
        Third = 2
    }

    /// <summary>
    /// Test struct with XML comments.
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal unsafe partial struct XmlCommentStruct
    {
        /// <summary>
        /// The integer value.
        /// </summary>
        public int Value;
        /// <summary>
        /// The string name.
        /// </summary>
        public string Name;
    }

    /// <summary>
    /// Test interface for XML comment support.
    /// </summary>
    internal unsafe partial interface IXmlCommentTest : global::MicroCom.Runtime.IUnknown
    {
        /// <summary>
        /// Does something important.
        /// </summary>
        void DoSomething();
        /// <summary>
        /// Gets a value.
        /// </summary>
        /// <returns>The value.</returns>
        int Value
        {
            get;
        }

        /// <summary>
        /// Processes a value.
        /// </summary>
        /// <param name="value">The value to process.</param>
        void ProcessValue(int value);
    }
}

namespace Test.Impl
{
    internal unsafe partial class __MicroComIXmlCommentTestProxy : global::MicroCom.Runtime.MicroComProxyBase, IXmlCommentTest
    {
        public void DoSomething()
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

        public void ProcessValue(int value)
        {
            ((delegate* unmanaged[Stdcall]<void*, int, void>)(*PPV)[base.VTableSize + 2])(PPV, value);
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit()
        {
            global::MicroCom.Runtime.MicroComRuntime.Register(typeof(IXmlCommentTest), new Guid("4b236ea0-ca9d-4588-a8a3-ba7d94f941a7"), (p, owns) => new __MicroComIXmlCommentTestProxy(p, owns));
        }

        protected __MicroComIXmlCommentTestProxy(IntPtr nativePointer, bool ownsHandle) : base(nativePointer, ownsHandle)
        {
        }

        protected override int VTableSize => base.VTableSize + 3;
    }

    unsafe class __MicroComIXmlCommentTestVTable : global::MicroCom.Runtime.MicroComVtblBase
    {
        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
        delegate void DoSomethingDelegate(void* @this);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void DoSomething(void* @this)
        {
            IXmlCommentTest __target = null;
            try
            {
                {
                    __target = (IXmlCommentTest)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.DoSomething();
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
            IXmlCommentTest __target = null;
            try
            {
                {
                    __target = (IXmlCommentTest)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
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
        delegate void ProcessValueDelegate(void* @this, int value);
#if NET5_0_OR_GREATER
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvStdcall) })] 
#endif
        static void ProcessValue(void* @this, int value)
        {
            IXmlCommentTest __target = null;
            try
            {
                {
                    __target = (IXmlCommentTest)global::MicroCom.Runtime.MicroComRuntime.GetObjectFromCcw(new IntPtr(@this));
                    __target.ProcessValue(value);
                }
            }
            catch (System.Exception __exception__)
            {
                global::MicroCom.Runtime.MicroComRuntime.UnhandledException(__target, __exception__);
                ;
            }
        }

        protected __MicroComIXmlCommentTestVTable()
        {
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, void>)&DoSomething); 
#else
            base.AddMethod((DoSomethingDelegate)DoSomething); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int>)&GetValue); 
#else
            base.AddMethod((GetValueDelegate)GetValue); 
#endif
#if NET5_0_OR_GREATER
            base.AddMethod((delegate* unmanaged[Stdcall]<void*, int, void>)&ProcessValue); 
#else
            base.AddMethod((ProcessValueDelegate)ProcessValue); 
#endif
        }

        [System.Runtime.CompilerServices.ModuleInitializer()]
        internal static void __MicroComModuleInit() => global::MicroCom.Runtime.MicroComRuntime.RegisterVTable(typeof(IXmlCommentTest), new __MicroComIXmlCommentTestVTable().CreateVTable());
    }
}