using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpinGameDemo.Game.Zones
{
    [CreateAssetMenu(fileName = "ZoneNumberColors", menuName = "Spin/ZoneNumberColors")]
    public class ZoneNumberColors : ScriptableObject
    {
        [SerializeField] private List<NumberColors> numberColors = new List<NumberColors>();
        private Dictionary<ZoneType, NumberColors> numberColorsDict = new Dictionary<ZoneType, NumberColors>();

        private void OnValidate()
        {
            numberColorsDict.Clear();
            foreach (var nc in numberColors)
            {
                numberColorsDict[nc.zoneType] = nc;
            }
        }

        private void Awake()
        {
            OnValidate();
        }

        public NumberColors GetColorsForZoneType(ZoneType zoneType)
        {
            if (numberColorsDict.TryGetValue(zoneType, out var colors))
            {
                return colors;
            }
            throw new Exception($"No NumberColors defined for ZoneType {zoneType}");
        }
    }
}