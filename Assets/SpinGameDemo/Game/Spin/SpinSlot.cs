using System;
using SpinGameDemo.Game.Spin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game
{
    public class SpinSlot : MonoBehaviour
    {
        [field: SerializeField] public Image Icon { get; private set; }
        [field: SerializeField] public TextMeshProUGUI AmountText { get; private set; }

        private void OnValidate()
        {
            if (Icon == null) Icon = GetComponentInChildren<Image>();
            if (AmountText == null) AmountText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Awake()
        {
            OnValidate();
        }

        public void SetOutcome(SpinOutcome outcome)
        {
            if (outcome == null)
            {
                Debug.LogError("SpinOutcome is null in SpinSlot.SetOutcome");
                return;
            }
            Icon.sprite = outcome.Icon;
            if (outcome.Amount < 0) // Bomb
            {
                AmountText.gameObject.SetActive(false);
            }
            else
            {
                AmountText.gameObject.SetActive(true);
                AmountText.text = outcome.Amount.ToString();
            }
        }
    }
}