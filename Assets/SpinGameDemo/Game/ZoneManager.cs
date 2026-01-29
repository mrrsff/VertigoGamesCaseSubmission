using System;
using SpinGameDemo.Context;
using SpinGameDemo.Rewards;
using SpinGameDemo.Spin;
using SpinGameDemo.User;

namespace SpinGameDemo.Game
{
    public enum ZoneType { Normal, Safe, Super }
    public class ZoneManager : IContextUnit
    {
        private SpinManager spinManager;
        private RewardsManager rewardsManager;
        
        public event Action OnZoneChanged;
        public void Initialize()
        {
            spinManager = GameContext.Get<SpinManager>();
            spinManager.SetZone(PersistentUserData.GetZone());
            // spinManager.OnSpinCompleted += LoadNextZone;
            
            rewardsManager = GameContext.Get<RewardsManager>();
            rewardsManager.OnRewardCollected += LoadNextZone;
        }

        public void Dispose()
        {
            
        }
        
        private void LoadNextZone()
        {
            int nextZone = PersistentUserData.GetZone() + 1;
            PersistentUserData.SetZone(nextZone);
            spinManager.SetZone(nextZone);
        }
    }
}