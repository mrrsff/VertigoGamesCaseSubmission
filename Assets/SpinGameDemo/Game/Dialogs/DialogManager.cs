using System;
using SpinGameDemo.Context;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SpinGameDemo.Game.Dialogs
{
    public class DialogManager : IContextUnit
    {
        private DialogCollection collection;
        private DialogController dialogController;

        private Dialog currentDialog;
        private RectTransform dialogParent;
        public void Initialize()
        {
            collection = Resources.Load<DialogCollection>("DialogCollection");
        }

        public void Dispose()
        {
        }
        
        public void SetController(DialogController controller)
        {
            dialogController = controller;
            dialogParent = dialogController.dialogPanelTransform;
        }
        
        public T ShowDialog<T>() where T : Dialog
        {
            var dict = collection.GetDialogDictionary();
            if (!dict.TryGetValue(typeof(T), out var dialogPrefab)) return null;

            if (currentDialog != null)
            {
                Object.Destroy(currentDialog.gameObject);
            }

            currentDialog = Object.Instantiate(dialogPrefab, dialogParent);
            currentDialog.OnOpen();
            currentDialog.dialogManager = this;
            return currentDialog as T;
        }

        public void CloseCurrentDialog(Action callback = null)
        {
            if (currentDialog == null) return;
            var dialog = currentDialog;
            currentDialog.OnClose(() => { Object.Destroy(dialog.gameObject); });
            currentDialog = null;
        }

        public void CloseDialog(Dialog dialog, Action callback = null)
        {
            if (currentDialog != dialog) return;
            CloseCurrentDialog(callback);
        }
    }
}