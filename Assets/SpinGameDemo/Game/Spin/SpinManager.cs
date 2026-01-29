using System;
using SpinGameDemo.Context;
using SpinGameDemo.Game;
using SpinGameDemo.Rewards;
using UnityEngine;

namespace SpinGameDemo.Spin
{
    public class SpinManager : IContextUnit
    {
        private RewardsManager rewardsManager;
        private ZoneManager zoneManager;
        
        private SpinPresetLibrary _presetLibrary;
        private SpinPreset currentPreset;
        private WheelPanelController _controller;
        
        private int zone;
        private bool setupOnControllerSet = false;
        private bool spinned = false;
        
        public event Action OnSpinCompleted; 
        public event Action OnSpinFailed; 
        
        public bool CanSpin() => !spinned;
        public void Initialize()
        {
            rewardsManager = GameContext.Get<RewardsManager>();
            zoneManager = GameContext.Get<ZoneManager>();
            zoneManager.OnZoneChanged += () => SetZone(zone + 1);
            
            _presetLibrary = Resources.Load<SpinPresetLibrary>("SpinPresetLibrary");
            _presetLibrary.Init();
        }
        
        public void SetController(WheelPanelController controller)
        {
            _controller = controller;
            if (setupOnControllerSet)
            {
                SetZone(zone);
                setupOnControllerSet = false;
            }
        }

        public void Dispose()
        {
        }
        
        public int GetSpinResultSliceIndex()
        {
            if (currentPreset != null) return currentPreset.GetRandomOutcomeIndex();
            
            Debug.LogError("Current preset is null in SpinManager");
            return -1;
        }
        
        public void SetZone(int newZone)
        {
            zone = newZone;
            currentPreset = _presetLibrary.GetPreset(zone);
            if (_controller) _controller.SetupPreset(currentPreset);
            else setupOnControllerSet = true;
            spinned = false;
        }
        
        public void ApplySpinResult(int sliceIndex)
        {
            if (currentPreset == null)
            {
                Debug.LogError("SpinPreset is null in SpinManager");
                return;
            }

            SpinOutcome outcome = currentPreset.GetOutcome(sliceIndex);
            var slot = _controller.GetSlotAt(sliceIndex);
            if (outcome.Apply(rewardsManager, slot))
            {
                OnSpinCompleted?.Invoke();
            }
            else
            {
                OnSpinFailed?.Invoke();
            }
        }
    }
}