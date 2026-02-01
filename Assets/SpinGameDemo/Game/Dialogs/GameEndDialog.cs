using System;
using System.Collections.Generic;
using SpinGameDemo.Game.Rewards;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game.Dialogs
{
    public class GameEndDialog : Dialog
    {
        [SerializeField] private Transform WinContent;
        [SerializeField] private Transform GameOverContent;
        
        [SerializeField] private Button restartButton;
        
        [SerializeField] private Transform rewardsParent;
        [SerializeField] private DialogRewardEntry rewardEntry;

        private GameStateManager gameStateManager;
        private void OnValidate()
        {
            if (restartButton == null)
                restartButton = GetComponentInChildren<Button>();
        }

        public override void OnOpen(Action callback = null)
        {
            gameStateManager = GameContext.Get<GameStateManager>();
            restartButton.onClick.AddListener(RestartGame);
            base.OnOpen(callback);
        }

        public override void OnClose(Action callback = null)
        {
            
        }

        private void RestartGame()
        {
            gameStateManager.SetState(GameState.Restart);
            OnClose();
            // Close();
        }

        public void SetRewards(List<RewardEntry> rewards)
        {
            foreach (Transform child in rewardsParent)
            {
                Destroy(child.gameObject);
            }

            foreach (var reward in rewards)
            {
                var entry = Instantiate(rewardEntry, rewardsParent);
                entry.SetData(reward.GetIcon(), reward.GetAmount());
            }
        }

        public void SetAsWin()
        {
            WinContent.gameObject.SetActive(true);
            GameOverContent.gameObject.SetActive(false);
        }
        
        public void SetAsGameOver()
        {
            WinContent.gameObject.SetActive(false);
            GameOverContent.gameObject.SetActive(true);
        }
    }
}