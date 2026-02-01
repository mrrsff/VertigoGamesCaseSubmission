using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpinGameDemo.Game.Dialogs
{
    [CreateAssetMenu(fileName = "DialogCollection", menuName = "Spin/DialogCollection")]
    public class DialogCollection : ScriptableObject
    {
        public Dialog[] dialogs;

        public Dictionary<Type, Dialog> GetDialogDictionary()
        {
            if (cachedDictionary != null) return cachedDictionary;
            cachedDictionary = ToDictionary();
            return cachedDictionary;
        }

        private Dictionary<Type, Dialog> cachedDictionary;

        private Dictionary<Type, Dialog> ToDictionary()
        {
            var dict = new Dictionary<Type, Dialog>();
            foreach (var dialog in dialogs)
            {
                dict[dialog.GetType()] = dialog;
            }

            return dict;
        }
    }
}