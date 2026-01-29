using SpinGameDemo.Context;
using SpinGameDemo.Rewards;
using SpinGameDemo.Spin;
using UnityEngine;

namespace SpinGameDemo.Game
{
    public class GameContext : IContextUnit
    {
        public ContextContainer Container { get; } = new ContextContainer();
        private GameContextController _controller;
        
        public void SetController(GameContextController controller)
        {
            _controller = controller;
        }

        public GameContext()
        {
            _instance = this;
            Container.Add(new SpinManager());
            Container.Add(new RewardsManager());
            Container.Add(new ZoneManager());
        }
        
        public void Initialize()
        {
            Container.Initialize();
        }

        public void Dispose()
        {
            
        }

        private static GameContext _instance;
        public static T Get<T>() where T : IContextUnit
        {
            Debug.Assert(_instance != null, "GameContext instance is not set.");
            return _instance.Container.Get<T>();
        }
    }
}