using DG.Tweening;
using UnityEngine;

namespace SpinGameDemo.Game.Wheel
{
    public partial class WheelPanelController
    {
        private Tween SpinWheelToSlice(int sliceIndex)
        {
            // Wheel has 8 slices, each slice is 45 degrees
            float sliceAngle = sliceIndex * 45f;

            const float pointerOffset = 90f; 
            float desiredRotation = pointerOffset - sliceAngle;

            float currentZ = wheelTransform.localEulerAngles.z;
            float shortDelta = Mathf.DeltaAngle(currentZ, desiredRotation);
            while (shortDelta > 0) shortDelta -= 360f;

            int numSpins = Random.Range(4, 7);
            float totalRotation = shortDelta - (numSpins * 360f);
    
            float targetAngle = currentZ + totalRotation;
            
            const float maxOvershoot = 10f;
            const float minOvershoot = 5f;
            float overshoot = Random.Range(minOvershoot, maxOvershoot);

            float spinDuration = numSpins * .5f; 
            Sequence spinSequence = DOTween.Sequence();
            
            float currentRotation = currentZ;
            
            spinSequence.Append(DOTween.To(
                () => currentRotation,
                x => {
                    currentRotation = x;
                    wheelTransform.localEulerAngles = new Vector3(0, 0, x);
                },
                targetAngle - overshoot,
                spinDuration)
                .SetEase(Ease.OutQuad));
            
            // Oscillating settle animation
            const int overshootSteps = 4;
            const float baseStepDuration = 0.25f;
            for(int i = 0; i < overshootSteps; i++)
            {
                float stepOvershoot = overshoot * (1f - ((float)i / overshootSteps));
                float stepDuration = baseStepDuration / (i + 1);
                
                // Alternate between undershooting and overshooting
                float oscillationAngle = targetAngle + (i % 2 != 0 ? -stepOvershoot : stepOvershoot);
                
                spinSequence.Append(DOTween.To(
                    () => currentRotation,
                    x => {
                        currentRotation = x;
                        wheelTransform.localEulerAngles = new Vector3(0, 0, x);
                    },
                    oscillationAngle,
                    stepDuration)
                    .SetEase(Ease.OutQuad));
            }
            
            spinSequence.Append(DOTween.To(
                () => currentRotation,
                x => {
                    currentRotation = x;
                    wheelTransform.localEulerAngles = new Vector3(0, 0, x);
                },
                targetAngle,
                baseStepDuration)
                .SetEase(Ease.OutQuad));
            
            return spinSequence;
        }
    }
}