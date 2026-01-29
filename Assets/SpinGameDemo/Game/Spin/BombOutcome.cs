using System;
using SpinGameDemo.Rewards;
using UnityEngine;

namespace SpinGameDemo.Spin
{
    [Serializable]
    public class BombOutcome : SpinOutcome
    {
        // private DialogManager dialogManager;
        public override bool Apply(RewardsManager rewardsManager, SpinSlot slot)
        {
            Debug.Log("BombOutcome triggered! Additional bomb logic can be implemented here.");
            return false;
        }
    }
}