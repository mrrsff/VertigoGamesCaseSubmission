using System;
using System.Collections.Generic;
using System.Linq;
using SpinGameDemo.Game.Spin;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpinGameDemo.Game
{
    [CreateAssetMenu(fileName = "SpinPreset", menuName = "Spin/Spin Preset", order = 1)]
    public class SpinPreset : ScriptableObject
    {
        public List<OutcomeEntry> outcomes = new List<OutcomeEntry>() { OutcomeEntry.CreateDefault() };

        public void OnValidate()
        {
            if (outcomes.Count != 8)
            {
                outcomes = outcomes.Take(8).ToList();
                while (outcomes.Count < 8)
                {
                    outcomes.Add(OutcomeEntry.CreateDefault());
                }
            }
        }

        public int GetRandomOutcomeIndex()
        {
            float totalWeight = outcomes.Sum(entry => entry.weight);

            float randomValue = Random.Range(0f, totalWeight);
            float cumulativeWeight = 0f;

            for (int i = 0; i < outcomes.Count; i++)
            {
                cumulativeWeight += outcomes[i].weight;
                if (randomValue <= cumulativeWeight)
                {
                    return i;
                }
            }
            
            Debug.LogError("Random outcome selection fell through; this should not happen.");
            return outcomes.Count - 1; // Fallback
        }
        public SpinOutcome GetOutcome(int index)
        {
            if (index < 0 || index >= outcomes.Count)
            {
                Debug.LogError($"Index {index} is out of bounds for possible outcomes.");
                index = Mathf.Clamp(index, 0, outcomes.Count - 1);
            }
            return outcomes[index].outcome;
        }
        
        public void Scale(float scale)
        {
            foreach (var t in outcomes)
            {
                t.outcome.Scale(scale);
            }
        }
    }

    [Serializable]
    public class OutcomeEntry
    {
        public SpinOutcome outcome;
        public float weight;
        
        public static OutcomeEntry CreateDefault()
        {
            return new OutcomeEntry
            {
                outcome = null,
                weight = 1f
            };
        }
    }
}