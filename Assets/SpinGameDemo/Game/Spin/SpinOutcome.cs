using System;
using SpinGameDemo.Game.Rewards;
using UnityEngine;

namespace SpinGameDemo.Game.Spin
{
    [Serializable]
    public class SpinOutcome
    {
        public Sprite Icon;
        public int Amount = 0;
        public bool IsBomb;
        public bool AllowScaling = true;
        public void Apply(RewardManager rewardManager, SpinSlot slot)
        {
            rewardManager.CollectToPanel(slot, this);
        }
        public void Scale(float scale)
        {
            if (AllowScaling) Amount = Mathf.RoundToInt(Amount * scale);
        }
    }
}