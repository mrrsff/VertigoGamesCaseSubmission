using System;
using SpinGameDemo.Spin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Rewards
{
    public class RewardEntry : MonoBehaviour
    {
        private Image IconImage;
        private TextMeshProUGUI AmountText;

        private int currentAmount;
        private void OnValidate()
        {
            if (IconImage == null || AmountText == null)
            {
                IconImage = GetComponentInChildren<Image>();
                AmountText = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        private void Awake()
        {
            OnValidate();
        }

        public void SetData(Sprite icon, int amount)
        {
            IconImage.sprite = icon;
            AddAmount(amount);
        }
        
        public void SetData(SpinOutcome outcome)
        {
            SetData(outcome.Icon, outcome.Amount);
        }
        
        public void AddAmount(int amount)
        {
            currentAmount += amount;
            AmountText.text = currentAmount.ToString();
        }
        
    }
}