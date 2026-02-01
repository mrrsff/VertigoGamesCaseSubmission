using SpinGameDemo.Game.Zones;
using UnityEngine;

namespace SpinGameDemo.Game.Wheel
{
    [CreateAssetMenu(fileName = "WheelAssets", menuName = "Spin/WheelAssets", order = 1)]
    public class WheelAssets : ScriptableObject
    {
        public Sprite NormalWheelSprite;
        public Sprite SafeWheelSprite;
        public Sprite SuperWheelSprite;
        
        public Sprite NormalPointerSprite;
        public Sprite SafePointerSprite;
        public Sprite SuperPointerSprite;
        
        public Sprite GetWheelSprite(int zoneIndex)
        {
            return GetWheelSprite(ZoneManager.GetZoneType(zoneIndex + 1));
        }
        public Sprite GetWheelSprite(ZoneType zoneType)
        {
            return zoneType switch
            {
                ZoneType.Normal => NormalWheelSprite,
                ZoneType.Safe => SafeWheelSprite,
                ZoneType.Super => SuperWheelSprite,
                _ => NormalWheelSprite
            };
        }
        
        public Sprite GetPointerSprite(int zoneIndex)
        {
            return GetPointerSprite(ZoneManager.GetZoneType(zoneIndex + 1));
        }

        public Sprite GetPointerSprite(ZoneType zoneType)
        {
            return zoneType switch
            {
                ZoneType.Normal => NormalPointerSprite,
                ZoneType.Safe => SafePointerSprite,
                ZoneType.Super => SuperPointerSprite,
                _ => NormalPointerSprite
            };
        }
    }
}