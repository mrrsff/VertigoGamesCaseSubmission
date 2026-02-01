using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpinGameDemo.Game
{
    [CreateAssetMenu(fileName = "SpinOutcomeLibrary", menuName = "Spin/Spin Outcome Library", order = 2)]
    public class SpinPresetLibrary : ScriptableObject
    {
        [SerializeField] private AnimationCurve outcomeScaleCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private int zoneCount;
        [SerializeField] private List<SpinPreset> presets = new List<SpinPreset>();
        
        private Dictionary<int, SpinPreset> _zonePresets;
        private void OnValidate()
        {
            if (presets.Count != zoneCount)
            {
                presets = presets.GetRange(0, Math.Min(presets.Count, zoneCount));
                while (presets.Count < zoneCount)
                {
                    presets.Add(null);
                }
            }
            
            _zonePresets = new Dictionary<int, SpinPreset>();
            for (int i = 0; i < presets.Count; i++)
            {
                _zonePresets[i] = presets[i];
            }
        }
        
        public void SetPresets(List<SpinPreset> newPresets)
        {
            presets = newPresets;
            zoneCount = newPresets.Count;
            OnValidate();
        }

        public void Init()
        {
            
        }
        public int GetZoneCount() => zoneCount;
        public SpinPreset GetPreset(int zone)
        {
            if (_zonePresets.TryGetValue(zone, out var preset)) // Zone is 1-based
            {
                float scale = 1 + outcomeScaleCurve.Evaluate(zone);
                return ApplyScaling(preset, scale);
            }

            Debug.LogError($"No preset found for zone {zone + 1}");
            return null;
        }

        private static SpinPreset ApplyScaling(in SpinPreset preset, in float scale)
        {
            var copy = Instantiate(preset);
            copy.Scale(scale);
            return copy;
        }
    }
}