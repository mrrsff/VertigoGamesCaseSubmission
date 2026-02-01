using System;
using System.Collections.Generic;
using DG.Tweening;
using SpinGameDemo.Context;
using SpinGameDemo.Game.Spin;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SpinGameDemo.Game.Rewards
{
    public class RewardManager : IContextUnit
    {
        private RewardsPanelController controller;
        private ObjectPool<Image> imagePool;
        public event Action OnRewardCollected;
        public void Initialize()
        {
        }


        public void Dispose()
        {
        }
        
        public void SetController(RewardsPanelController controller)
        {
            this.controller = controller;
            CreatePool();
        }
        private void CreatePool()
        {
            imagePool = new ObjectPool<Image>(() =>
                {
                    var go = new GameObject("ui_icon_reward");
                    var img = go.AddComponent<Image>();
                    img.raycastTarget = false;
                    img.maskable = false;
                    img.preserveAspect = true;
                    go.SetActive(false);
                    return img;
                },
                img => img.gameObject.SetActive(true),
                img => img.gameObject.SetActive(false));
        }

        public void CollectToPanel(SpinSlot slot, SpinOutcome outcome)
        {
            controller.AddReward(outcome);
            var rewardEntry = controller.GetRewardEntry(outcome.Icon.name);
            DOVirtual.DelayedCall(1 / 60f, // Delay 1 frame to ensure UI is updated
                () =>
                {
                    var animation = PlayCollectAnimation(slot.transform, rewardEntry.transform, outcome.Icon);
                    animation.OnComplete(() => OnRewardCollected?.Invoke());
                });
        }

        public List<RewardEntry> CollectRewards()
        {
            return controller.GetAllRewards();
        }

        private Tween PlayCollectAnimation(Transform from, Transform to, Sprite icon)
        {
            int iconCount = Random.Range(4, 8);

            var mainSequence = DOTween.Sequence();
            for (int i = 0; i < iconCount; i++)
            {
                var img = imagePool.Get();
                img.sprite = icon;

                img.transform.SetParent(from.transform.parent, false);
                img.transform.position = from.position;
                img.transform.rotation = Quaternion.identity;
                img.transform.localScale = Vector3.zero;

                float randomScale = Random.Range(0.4f, 0.6f);
                float randomRadius = Random.Range(0.15f, 0.35f);
                float angle = (i * (360f / iconCount) + Random.Range(-15f, 15f)) * Mathf.Deg2Rad;
                Vector3 burstTarget = from.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * randomRadius;

                float randomBurstDuration = Random.Range(0.25f, 0.4f);
                float randomTravelDuration = Random.Range(0.4f, 0.6f);
                float randomDelay = Random.Range(0f, 0.1f);

                var sequence = DOTween.Sequence();

                sequence.Append(img.transform.DOScale(Vector3.one * randomScale, 0.1f));
                sequence.Join(img.transform.DOMove(burstTarget, randomBurstDuration).SetEase(Ease.OutBack));

                sequence.AppendInterval(Random.Range(0.02f, 0.08f));

                sequence.Append(img.transform.DOMove(to.position, randomTravelDuration).SetEase(Ease.InBack)
                    .SetDelay(randomDelay));
                sequence.Join(img.transform.DOScale(Vector3.zero, randomTravelDuration * 0.5f)
                    .SetEase(Ease.InQuad)
                    .SetDelay(randomTravelDuration * 0.5f + randomDelay));

                sequence.OnComplete(() => imagePool.Release(img));
                
                mainSequence.Join(sequence);
            }
            return mainSequence;
        }
    }
}