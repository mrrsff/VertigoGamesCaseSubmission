using System;
using UnityEngine;

namespace SpinGameDemo.Spin
{
    [Serializable]
    public class BombOutcome : SpinOutcome
    {
        public override void Apply()
        {
            Debug.Log("BombOutcome triggered! Additional bomb logic can be implemented here.");
        }
    }
}