using System;

namespace MicroCom.Runtime
{
    internal unsafe static class DebugHelpers
    {
        public static string PrettyPrintGuid(Guid guid)
        {
            if (MicroComRuntime.TryGetTypeForGuid(guid, out var type))
                return type.FullName;
            return guid.ToString();
        }
#if LOG_CCW_CALLS
        public static string PrettyPrintCcw(Ccw* ccw)
        {
            try
            {
                var target = ccw->GetShadow();

                return
                    $"{(((IntPtr)ccw).ToString("X16"))} - {PrettyPrintGuid(ccw->Guid)} {target.Target.GetType().FullName}";
            }
            catch
            {
                return ((IntPtr)ccw).ToString("X16");
            }
            
        }
#endif
    }
}