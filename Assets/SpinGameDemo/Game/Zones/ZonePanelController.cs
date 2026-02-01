using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SpinGameDemo.Game.Zones
{
    public class ZonePanelController : MonoBehaviour
    {
        private ZoneManager zoneManager;
        [SerializeField] private ZoneNumber numberPrefab;
        [SerializeField] private float spacing = 16f;
        [SerializeField] private Transform numbersContentTransform;
        
        private ZoneNumberColors zoneNumberColors;

        private List<ZoneNumber> numbersTexts = new List<ZoneNumber>();
        private int currentZone = 0;
        private NumberColors currentColors;
        private float numberRectWidth;

        private void OnValidate()
        {
            numberRectWidth = numberPrefab.GetComponent<RectTransform>().rect.width;
        }

        // Grayish color for used zones
        private static Color GetUsedColor(Color color) 
        {
            return Color.Lerp(color, Color.gray, 0.9f);
        }

        private void Awake()
        { 
            zoneNumberColors = Resources.Load<ZoneNumberColors>("ZoneNumberColors");
        }

        private void Start()
        {
            zoneManager = GameContext.Get<ZoneManager>();
            zoneManager.SetController(this);
            zoneManager.OnZoneChanged += OnZoneChanged;
            
        }
        public void PopulateNumbers(int zoneCount)
        {
            currentZone = zoneManager.CurrentZone;
            numbersContentTransform.localPosition = new Vector3(-currentZone * (numberRectWidth + spacing), 0, 0);
            for (int i = 0; i < zoneCount; i++)
            {
                NumberColors colors = zoneNumberColors.GetColorsForZoneType(ZoneManager.GetZoneType(i + 1));
                ZoneNumber numberText = Instantiate(numberPrefab, numbersContentTransform);
                numberText.numberText.text = (i + 1).ToString();
                numberText.backgroundImage.sprite = colors.backgroundSprite;
                
                if (i == currentZone)
                {
                    currentColors = colors;
                    numberText.backgroundImage.color = new Color(colors.backgroundColor.r,
                        colors.backgroundColor.g, colors.backgroundColor.b, 1f);
                    numberText.numberText.color = currentColors.highlightTextColor;
                }
                else if (i < currentZone)
                {
                    numberText.backgroundImage.color = new Color(colors.backgroundColor.r,
                        colors.backgroundColor.g, colors.backgroundColor.b, 0f);
                    numberText.numberText.color = GetUsedColor(currentColors.textColor);
                }
                else
                {
                    numberText.backgroundImage.color = new Color(colors.backgroundColor.r,
                        colors.backgroundColor.g, colors.backgroundColor.b, 0f);
                    numberText.numberText.color = colors.textColor;
                }
                
                RectTransform rectTransform = numberText.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(i * (rectTransform.rect.width + spacing), 0);
                numbersTexts.Add(numberText);
            }
        }

        private void OnZoneChanged(int newZone)
        {
            const float transitionDuration = 0.25f;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(ShiftNumbers(newZone, transitionDuration));
            
            if (currentZone != newZone)
            {
                ZoneNumber oldZoneNumber = numbersTexts[currentZone];
                sequence.Join(SetBackgroundAlpha(oldZoneNumber, 0f, transitionDuration));
                sequence.Join(SetTextColor(oldZoneNumber, GetUsedColor(currentColors.textColor), transitionDuration));
            }
            ZoneNumber newZoneNumber = numbersTexts[newZone];
            currentColors = zoneNumberColors.GetColorsForZoneType(ZoneManager.GetZoneType(newZone + 1));
            
            sequence.Join(SetBackgroundAlpha(newZoneNumber, 1f, transitionDuration));
            sequence.Join(SetTextColor(newZoneNumber, currentColors.highlightTextColor, transitionDuration));
            currentZone = newZone;
        }
        
        private Tween ShiftNumbers(int newZone, float duration)
        {
            return numbersContentTransform.DOLocalMoveX( -newZone * (numberRectWidth + spacing), duration);
        }

        private static Tween SetBackgroundAlpha(ZoneNumber zoneNumber, float alpha, float duration)
        {
            return zoneNumber.backgroundImage.DOFade(alpha, duration);
        }
        
        private static Tween SetTextColor(ZoneNumber zoneNumber, Color targetColor, float duration)
        {
            return zoneNumber.numberText.DOColor(targetColor, duration);
        }
    }
    
    [Serializable]
    public struct NumberColors
    {
        public ZoneType zoneType;
        public Color textColor;
        public Sprite backgroundSprite;
        public Color backgroundColor;
        public Color highlightTextColor;
    }
}