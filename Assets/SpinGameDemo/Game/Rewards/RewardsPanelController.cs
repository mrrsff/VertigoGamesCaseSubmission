using System.Collections.Generic;
using SpinGameDemo.Game;
using SpinGameDemo.Spin;
using UnityEngine;

namespace SpinGameDemo.Rewards
{
    public class RewardsPanelController : MonoBehaviour
    {
        [SerializeField] private RewardEntry rewardEntryPrefab;
        [SerializeField] private Transform contentTransform;

        private Dictionary<string, RewardEntry> rewardMap = new Dictionary<string, RewardEntry>();
        private RewardsManager rewardsManager;

        private void Start()
        {
            rewardsManager = GameContext.Get<RewardsManager>();
            rewardsManager.SetController(this);
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