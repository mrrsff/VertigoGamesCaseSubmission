using System;
using SpinGameDemo.Context;
using SpinGameDemo.Game.Rewards;
using SpinGameDemo.User;

namespace SpinGameDemo.Game.Zones
{
    public enum ZoneType { Normal, Safe, Super }
    public class ZoneManager : IContextUnit
    {
        private SpinManager spinManager;
        private RewardManager _rewardManager;
        private ZonePanelController controller;
        public int CurrentZone => PersistentUserData.GetZone();
        public event Action<int> OnZoneChanged;
        public void Initialize()
        {
            spinManager = GameContext.Get<SpinManager>();
            _rewardManager = GameContext.Get<RewardManager>();
            _rewardManager.OnRewardCollected += LoadNextZone;
        }
        
        public void SetController(ZonePanelController controller)
        {
            this.controller = controller;
            controller.PopulateNumbers(spinManager.GetMaxZone());
            LoadCurrentZone();
        }

        public void Dispose()
        {
            
        }
        
        private void LoadNextZone()
        {
            int nextZone = PersistentUserData.GetZone() + 1;
            if (nextZone > spinManager.GetMaxZone() - 1) return;
            
            PersistentUserData.SetZone(nextZone);
            OnZoneChanged?.Invoke(nextZone);
        }
        
        private void LoadCurrentZone()
        {
            int currentZone = PersistentUserData.GetZone();
            OnZoneChanged?.Invoke(currentZone);
        }
        
        public static ZoneType GetZoneType(int zone)
        {
            if (zone == 0) return ZoneType.Normal;
            if (zone % 30 == 0) return ZoneType.Super;
            if (zone % 5 == 0) return ZoneType.Safe;
            return ZoneType.Normal;
        }
    }
}