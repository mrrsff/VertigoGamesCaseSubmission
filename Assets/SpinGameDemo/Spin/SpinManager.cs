using SpinGameDemo.Context;
using UnityEngine;

namespace SpinGameDemo.Spin
{
    public class SpinManager : IContextUnit
    {
        private SpinPresetLibrary _presetLibrary;
        private SpinPreset currentPreset;
        private WheelPanelController _controller;
        
        private int zone = 0; // TODO: Make dynamic
        public void Initialize()
        {
            _presetLibrary = Resources.Load<SpinPresetLibrary>("SpinPresetLibrary");
            _presetLibrary.Init();
        }
        
        public void SetController(WheelPanelController controller)
        {
            _controller = controller;
            SetZone(1);
        }

        public void Dispose()
        {
        }
        
        public int GetSpinResultSlice()
        {
            if (currentPreset != null) return currentPreset.GetRandomOutcome();
            
            Debug.LogError("Current preset is null in SpinManager");
            return -1;

        }
        
        public void SetZone(int newZone)
        {
            zone = newZone;
            currentPreset = _presetLibrary.GetPreset(zone);
            _controller.SetupPreset(currentPreset);
        }
        
        public void ApplySpinResult(int sliceIndex)
        {
            if (currentPreset == null)
            {
                Debug.LogError("SpinPreset is null in SpinManager");
                return;
            }

            SpinOutcome outcome = currentPreset.GetOutcome(sliceIndex);
            Debug.Log($"Spin outcome: {outcome.Icon.name} x{outcome.Amount}, Index: {sliceIndex}");
            outcome.Apply();
        }
    }
}