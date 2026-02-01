using System;
using DG.Tweening;
using SpinGameDemo.Game.Spin;
using SpinGameDemo.Game.Zones;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game.Wheel
{
    public partial class WheelPanelController : MonoBehaviour
    {
        [SerializeField] private SpinSlot slotPrefab;
        [SerializeField] private float radius;
        private SpinManager spinManager;
        
        private Button spinButton;
        private Transform wheelTransform;
        private SpinPreset spinPreset;
        
        [SerializeField] private Image wheelImage;
        [SerializeField] private Image pointerImage;

        private SpinSlot[] spinSlots = new SpinSlot[8];
        private bool slotsCreated;
        private void OnValidate() 
        {
            if (!spinButton) spinButton = GetComponentInChildren<Button>(true);
            if (!wheelTransform) wheelTransform = transform.GetChild(0);
        }

        private void Awake()
        {
            OnValidate();
        }

        private void Start()
        {
            spinManager = GameContext.Get<SpinManager>();
            spinManager.SetController(this);
            spinButton.onClick.AddListener(SpinWheel);
        }

        public SpinSlot GetSlotAt(int index)
        {
            if (index < 0 || index >= spinSlots.Length)
            {
                Debug.LogError($"Index {index} is out of bounds for spinSlots");
                return null;
            }
            return spinSlots[index];
        }

        private void SpinWheel()
        {
            if (!spinManager.CanSpin()) return;
            spinManager.StartSpin();
            int sliceIndex = spinManager.GetSpinResultSliceIndex();
            SpinWheelToSlice(sliceIndex).OnComplete(() => 
            {
                spinManager.ApplySpinResult(sliceIndex);
            });
            
        }
        private void CreateSlots()
        {
            for (int i = 0; i < 8; i++)
            {
                var (position, rotation) = GetSlotTransform(i, radius);
                SpinSlot slotObj = Instantiate(slotPrefab, wheelTransform);
                slotObj.transform.localPosition = position;
                slotObj.transform.localEulerAngles = rotation;
                SpinSlot slot = slotObj.GetComponent<SpinSlot>();
                spinSlots[i] = slot;
            }
            slotsCreated = true;
        }
        public void SetupPreset(SpinPreset preset, Sprite wheelSprite, Sprite pointerSprite)
        {
            spinPreset = preset;
            if (!slotsCreated) CreateSlots();
            
            for (int i = 0; i < 8; i++)
            {
                SpinOutcome outcome = spinPreset.GetOutcome(i);
                spinSlots[i].SetOutcome(outcome);
            }
            
            wheelImage.sprite = wheelSprite;
            pointerImage.sprite = pointerSprite;
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