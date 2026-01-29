namespace SpinGameDemo.Context
{
    public interface IContextUnit
    {
        void Initialize();
        void Dispose();
    }

    public interface IContextBehaviour : IContextUnit
    {
        void Update();
    }
}