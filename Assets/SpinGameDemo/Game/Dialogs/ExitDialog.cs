using System;
using SpinGameDemo.Game.Zones;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game.Dialogs
{
    public class ExitDialog : Dialog
    {
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;

        [SerializeField] private TextMeshProUGUI infoText;
        
        [SerializeField] private string infoSafeToExit = "Are you sure? You can win even more.";
        [SerializeField] private string infoNotSafeToExit = "Are you sure? You will lose your rewards.";
        private GameStateManager gameStateManager;
        private ZoneManager zoneManager;
        private bool isSafeToExit;
        private void OnValidate()
        {
            if (yesButton == null || noButton == null)
            {
                var buttons = GetComponentsInChildren<Button>();
                if (buttons.Length >= 2)
                {
                    noButton = buttons[1];
                    yesButton = buttons[0];
                }
            }
        }

        private void Awake()
        {
            OnValidate();
            yesButton.onClick.AddListener(OnYesClicked);
            noButton.onClick.AddListener(OnNoClicked);
        }

        public override void OnOpen(Action callback = null)
        {
            gameStateManager = GameContext.Get<GameStateManager>();
            zoneManager = GameContext.Get<ZoneManager>();
            isSafeToExit = IsSafeToExit();
            infoText.text = isSafeToExit ? infoSafeToExit : infoNotSafeToExit;
            base.OnOpen(callback);
        }
        private void OnYesClicked()
        {
            gameStateManager.SetState(isSafeToExit ? GameState.GameWon : GameState.GameEnd);
            Close();
        }

        private void OnNoClicked()
        {
            gameStateManager.SetState(GameState.Idle);
            Close();
        }

        private bool IsSafeToExit()
        {
            return ZoneManager.GetZoneType(zoneManager.CurrentZone + 1) != ZoneType.Normal;
        }
    }
}