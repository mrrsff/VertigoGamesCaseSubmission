using DG.Tweening;
using SpinGameDemo.Context;
using SpinGameDemo.Game.Dialogs;
using SpinGameDemo.Game.Rewards;
using SpinGameDemo.Game.Zones;
using SpinGameDemo.User;
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
            Container.Add(new RewardManager());
            Container.Add(new ZoneManager());
            Container.Add(new DialogManager());
            Container.Add(new GameStateManager());
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