using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game.Dialogs
{
    public class Dialog : MonoBehaviour
    {
        protected virtual DialogAnimation openAnimation => DialogAnimation.DefaultOpen;
        protected virtual DialogAnimation closeAnimation => DialogAnimation.DefaultClose;
        public Image background;
        public RectTransform content;
        private Tween currentTween;
        
        public DialogManager dialogManager;

        public virtual void OnOpen(Action callback = null)
        {
            if (currentTween != null && currentTween.IsActive())
                currentTween.Kill();

            currentTween = openAnimation?.CreateTween(this);
            currentTween?.Play();
            currentTween?.OnComplete(() => { callback?.Invoke(); });
        }

        public virtual void OnClose(Action callback = null)
        {
            if (currentTween != null && currentTween.IsActive())
                currentTween.Kill();

            currentTween = closeAnimation?.CreateTween(this);
            currentTween?.Play();
            currentTween?.OnComplete(() => { callback?.Invoke(); });
        }

        protected void Close()
        {
            dialogManager.CloseDialog(this);
        }
    }
}