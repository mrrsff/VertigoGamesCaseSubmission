using DG.Tweening;
using SpinGameDemo.Game;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Spin
{
    public partial class WheelPanelController : MonoBehaviour
    {
        [SerializeField] private SpinSlot slotPrefab;
        [SerializeField] private float radius;
        private SpinManager spinManager;
        
        private Button spinButton;
        private Transform wheelTransform;
        private SpinPreset spinPreset;

        private Tween currentSpinTween;
        private SpinSlot[] spinSlots = new SpinSlot[8];
        
        private int currentSliceIndex = 0;
        private float currentRotation = 0f;
        private void OnValidate() 
        {
            if (!spinButton) spinButton = GetComponentInChildren<Button>(true);
            if (!wheelTransform) wheelTransform = transform.GetChild(0);
        }
        
        private void Start()
        {
            spinManager = GameContext.Get<SpinManager>();
            spinManager.SetController(this);
            spinButton.onClick.AddListener(SpinWheel);
        }

        private void SpinWheel()
        {
            if (currentSpinTween != null && currentSpinTween.IsActive() && currentSpinTween.IsPlaying())
            {
                return;
            }
            int sliceIndex = spinManager.GetSpinResultSlice();
            currentSpinTween = SpinWheelToSlice(sliceIndex);
            currentSpinTween.OnComplete(() => 
            {
                // Update rotation state
                currentSliceIndex = sliceIndex;
                currentRotation = wheelTransform.localEulerAngles.z;
                wheelTransform.localEulerAngles = new Vector3(0, 0, currentRotation);
                
                spinManager.ApplySpinResult(sliceIndex);
            });
        }

        public void SetupPreset(SpinPreset preset)
        {
            spinPreset = preset;
            for (int i = 0; i < 8; i++)
            {
                var (position, rotation) = GetSlotTransform(i, radius);
                SpinSlot slotObj = Instantiate(slotPrefab, wheelTransform);
                slotObj.transform.localPosition = position;
                slotObj.transform.localEulerAngles = rotation;
                SpinSlot slot = slotObj.GetComponent<SpinSlot>();
                spinSlots[i] = slot;
                if (spinPreset == null) continue;
                SpinOutcome outcome = spinPreset.GetOutcome(i);
                slot.SetOutcome(outcome);
            }
        }

        private static (Vector3 position, Vector3 rotation) GetSlotTransform(int index, float radius)
        {
            float angle = index * Mathf.PI / 4;
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            Vector3 position = new Vector3(x, y, 0);
            Vector3 rotation = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90);
            return (position, rotation);
        }

    }
}