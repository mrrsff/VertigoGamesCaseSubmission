using System;
using UnityEngine;

namespace SpinGameDemo.Game.Dialogs
{
    public class DialogController : MonoBehaviour
    {
        public RectTransform dialogPanelTransform;

        private DialogManager dialogManager;

        private void OnValidate()
        {
            dialogPanelTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            dialogManager = GameContext.Get<DialogManager>();
            dialogManager.SetController(this);
        }
    }
}