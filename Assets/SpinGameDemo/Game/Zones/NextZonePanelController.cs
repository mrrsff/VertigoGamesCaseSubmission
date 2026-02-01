using System;
using TMPro;
using UnityEngine;

namespace SpinGameDemo.Game.Zones
{
    public class NextZonePanelController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI safeZoneNumberText;
        [SerializeField] private TextMeshProUGUI superZoneNumberText;
        private void Start()
        {
            var zoneManager = GameContext.Get<ZoneManager>();
            zoneManager.OnZoneChanged += OnZoneChanged;
        }
        
        private void OnZoneChanged(int newZone)
        {
            if (newZone % 30 == 0)
            {
                superZoneNumberText.text = (newZone + 30).ToString();
            }
            else if (newZone % 5 == 0)
            {
                var nextSafeZone = ((newZone / 5) + 1) * 5;
                if (nextSafeZone % 30 == 0)
                {
                    nextSafeZone += 5;
                }
                safeZoneNumberText.text = nextSafeZone.ToString();
            }
        }
    }
}