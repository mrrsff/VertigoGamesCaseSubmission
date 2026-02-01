using DG.Tweening;
using SpinGameDemo.Game.Spin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game.Rewards
{
    public class RewardEntry : MonoBehaviour
    {
        private Image IconImage;
        private TextMeshProUGUI AmountText;
        
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private float scaleMultiplier = 1.5f;

        private int currentAmount;
        private Tween _textTween;
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
            AmountText.text = "0";
            AddAmount(amount);
        }
        
        public void SetData(SpinOutcome outcome)
        {
            SetData(outcome.Icon, outcome.Amount);
        }
        
        public void AddAmount(int amount)
        {
            int targetAmount = currentAmount + amount;

            _textTween?.Kill();
            
            _textTween = DOTween.To(() => currentAmount, x => {
                currentAmount = x;
                AmountText.text = currentAmount.ToString();
            }, targetAmount, duration).SetDelay(0.75f);

            // AmountText.transform.DOPunchScale(Vector3.one * (scaleMultiplier - 1f), duration, 0, 0);
        }
        
        public Sprite GetIcon()
        {
            return IconImage.sprite;
        }
        
        public int GetAmount()
        {
            return currentAmount;
        }
    }
}