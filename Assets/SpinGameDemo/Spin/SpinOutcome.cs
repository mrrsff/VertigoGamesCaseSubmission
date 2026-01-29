using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Spin
{
    [Serializable]
    public class SpinOutcome
    {
        public Sprite Icon;
        public int Amount;
        public virtual void Apply()
        {
            // Debug.Log($"Applied outcome: {Icon.name} x{Amount}");
        }
        public virtual void Scale(float scale)
        {
            Amount = Mathf.RoundToInt(Amount * scale);
        }

        public override int GetHashCode()
        {
            return Icon ? Icon.GetHashCode() : base.GetHashCode();
        }
    }
}