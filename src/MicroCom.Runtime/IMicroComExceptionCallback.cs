using System;

namespace MicroCom.Runtime
{
    public interface IMicroComExceptionCallback
    {
        void RaiseException(Exception e);
    }
}
