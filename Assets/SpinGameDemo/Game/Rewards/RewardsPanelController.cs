using System;
using System.Collections.Generic;
using SpinGameDemo.Game.Dialogs;
using SpinGameDemo.Game.Spin;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game.Rewards
{
    public class RewardsPanelController : MonoBehaviour
    {
        [SerializeField] private Button exitButton;
        [SerializeField] private RewardEntry rewardEntryPrefab;
        [SerializeField] private Transform contentTransform;

        private Dictionary<string, RewardEntry> rewardMap = new Dictionary<string, RewardEntry>();
        private RewardManager rewardManager;
        private DialogManager dialogManager;
        private GameStateManager gameStateManager;
        public List<RewardEntry> GetAllRewards() => new List<RewardEntry>(rewardMap.Values);

        private void OnValidate()
        {
            if (exitButton == null)
            {
                var buttons = GetComponentsInChildren<Button>();
                if (buttons.Length > 0)
                {
                    exitButton = buttons[0];
                }
            }
            if (rewardEntryPrefab == null)
            {
                rewardEntryPrefab = GetComponentInChildren<RewardEntry>();
            }
            if (contentTransform == null)
            {
                var content = GetComponentInChildren<ScrollRect>();
                if (content != null)
                {
                    contentTransform = content.content;
                }
            }
        }

        private void Start()
        {
            OnValidate();
            rewardManager = GameContext.Get<RewardManager>();
            rewardManager.SetController(this);
            
            dialogManager = GameContext.Get<DialogManager>();
            exitButton.onClick.AddListener(OnExitClicked);
            
            gameStateManager = GameContext.Get<GameStateManager>();
        }

        private void OnExitClicked()
        {
            if (gameStateManager.GetCurrentState() != GameState.Idle)
                return;
            gameStateManager.SetState(GameState.Exit);
        }

        public void AddReward(SpinOutcome reward)
        {
            string key = reward.Icon.name;
            if (rewardMap.TryGetValue(key, out var entry))
            {
                UpdateExistingUI(entry, reward);
            }
            else
            {
                AddToUI(reward);
            }
        }

        private void AddToUI(SpinOutcome reward)
        {
            var entry = Instantiate(rewardEntryPrefab, contentTransform);
            entry.SetData(reward);
            rewardMap.Add(reward.Icon.name, entry);
        }
        private void UpdateExistingUI(RewardEntry entry, SpinOutcome reward)
        {
            entry.AddAmount(reward.Amount);
        }

        public RewardEntry GetRewardEntry(string key)
        {
            rewardMap.TryGetValue(key, out var value);
            return value;
        }
    }
}