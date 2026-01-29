namespace SpinGameDemo.Context
{
    public static class ApplicationContext
    {
        private static ApplicationContextController _controller;

        public static void SetController(ApplicationContextController controller)
        {
            _controller = controller;
        }

        private static ContextContainer _contextContainer = new ContextContainer();

        public static T Get<T>() where T : IContextUnit
        {
            return _contextContainer.Get<T>();
        }

        public static void AddContextUnit(IContextUnit unit)
        {
            _contextContainer.Add(unit);
        }

        public static void Initialize()
        {
            _contextContainer.Initialize();
        }

        public static void Update()
        {
            _contextContainer.Update();
        }
    }
}