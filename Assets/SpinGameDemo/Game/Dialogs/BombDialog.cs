using System;
using SpinGameDemo.User;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game.Dialogs
{
    public class BombDialog : Dialog
    {
        [SerializeField] private Button giveUpButton;
        [SerializeField] private TextMeshProUGUI continueCostText;
        
        private GameStateManager gameStateManager;
        private SpinManager spinManager;

        private void OnValidate()
        {
            if (giveUpButton == null) giveUpButton = GetComponentInChildren<Button>();
        }

        private void Awake()
        {
            giveUpButton.onClick.AddListener(OnGiveUpClicked);
        }

        public override void OnOpen(Action callback = null)
        {
            gameStateManager = GameContext.Get<GameStateManager>();
            spinManager = GameContext.Get<SpinManager>();
            // ConfigureContinueButton();
            base.OnOpen(callback);
        }
        
        private void OnGiveUpClicked()
        {
            gameStateManager.SetState(GameState.GameEnd);
            Close();
        }
        
        private void OnContinueClicked()
        {
            gameStateManager.SetState(GameState.Idle);
            spinManager.ContinueAfterLose();
            Close();
        }
        
        // private void ConfigureContinueButton()
        // {
        //     const int baseCost = 50;
        //     const int costIncrement = 25;
        //     var currentZone = PersistentUserData.GetZone();
        //     var continueCost = baseCost + (currentZone * costIncrement);
        //     if (PersistentUserData.GetCash() < continueCost)
        //     {
        //         continueCostText.text = "Insufficient Funds";
        //         continueButton.interactable = false;
        //     }
        //     else
        //     {
        //         continueCostText.text = continueCost.ToString();
        //         continueButton.interactable = true;
        //     }
        // }
    }
}