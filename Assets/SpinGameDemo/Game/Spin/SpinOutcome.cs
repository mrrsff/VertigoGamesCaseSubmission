using System;
using SpinGameDemo.Rewards;
using UnityEngine;

namespace SpinGameDemo.Spin
{
    [Serializable]
    public class SpinOutcome
    {
        public Sprite Icon;
        public int Amount = 0;
        public bool AllowScaling = true;
        public virtual bool Apply(RewardsManager rewardsManager, SpinSlot slot)
        {
            rewardsManager.CollectToPanel(slot, this);
            return true;
        }
        public void Scale(float scale)
        {
            if (AllowScaling) Amount = Mathf.RoundToInt(Amount * scale);
        }
    }
}