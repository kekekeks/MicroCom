namespace MicroCom.Runtime
{
    public interface IMicroComShadowContainer
    {
        MicroComShadow Shadow { get; set; }
        void OnReferencedFromNative();
        void OnUnreferencedFromNative();
    }
}
