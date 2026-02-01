using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game.Dialogs
{
    public class DialogRewardEntry : MonoBehaviour
    {
        public Image iconImage;
        public TextMeshProUGUI amountText;

        private void OnValidate()
        {
            if (iconImage == null) iconImage = GetComponentInChildren<Image>();
            if (amountText == null) amountText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetData(Sprite icon, int amount)
        {
            if (iconImage != null)
                iconImage.sprite = icon;
            if (amountText != null)
                amountText.text = amount.ToString();
        }
    }
}