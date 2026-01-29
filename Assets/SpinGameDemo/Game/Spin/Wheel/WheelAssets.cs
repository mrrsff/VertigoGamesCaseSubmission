using SpinGameDemo.Game;
using UnityEngine;

namespace SpinGameDemo.Spin
{
    public class WheelAssets : ScriptableObject
    {
        public Sprite NormalWheelSprite;
        public Sprite SafeWheelSprite;
        public Sprite SuperWheelSprite;
        
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
    }
}