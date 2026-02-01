using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game.Zones
{
    public class ZoneNumber : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI numberText { get; private set; }
        [field: SerializeField] public Image backgroundImage { get; private set; }

        private void OnValidate()
        {
            if (numberText == null)
                numberText = GetComponentInChildren<TextMeshProUGUI>();
            if (backgroundImage == null)
                backgroundImage = GetComponentInChildren<Image>();
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}