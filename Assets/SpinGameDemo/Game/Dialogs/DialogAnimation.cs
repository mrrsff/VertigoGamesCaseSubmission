using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SpinGameDemo.Game.Dialogs
{
    public enum DialogAnimationKind
    {
        None,
        Slide,
        Scale,
        Fade
    }

    public enum DialogSlideDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    [System.Serializable]
    public struct SlideParams
    {
        public DialogSlideDirection direction;
        public float distance; // pixels or normalized units depending on useNormalizedDistance
        public float duration;
        public bool useNormalizedDistance; // if true, distance is 0-1 relative to screen size
    }

    [System.Serializable]
    public struct ScaleParams
    {
        public Vector2 from;
        public Vector2 to;
        public float duration;
    }

    [System.Serializable]
    public struct FadeParams
    {
        public float from;
        public float to;
        public float duration;
        public Ease ease;
        public bool absolute;
    }

    [System.Serializable]
    public class DialogAnimation
    {
        public static DialogAnimation DefaultOpen => new()
        {
            kind = DialogAnimationKind.Scale,
            scale = new ScaleParams
            {
                from = new Vector2(0.8f, 0.8f),
                to = Vector2.one,
                duration = 0.1f
            },
            fade = new FadeParams
            {
                from = 0f,
                to = 1f,
                duration = 0.1f,
                ease = Ease.OutQuad
            },
            ease = Ease.OutBack
        };

        public static DialogAnimation DefaultClose => new()
        {
            kind = DialogAnimationKind.Scale,
            scale = new ScaleParams
            {
                from = Vector2.one,
                to = new Vector2(0.8f, 0.8f),
                duration = 0.1f
            },
            fade = new FadeParams
            {
                from = 1f,
                to = 0f,
                duration = 0.1f,
                ease = Ease.InQuad
            },
            ease = Ease.InBack
        };

        public DialogAnimationKind kind;

        public SlideParams slide;
        public ScaleParams scale;
        public FadeParams fade;

        public Ease ease = Ease.OutQuad;

        public Tween CreateTween(Dialog dialog)
        {
            Sequence seq = DOTween.Sequence();

            if (dialog.background != null)
            {
                Tween bg = CreateFadeTween(dialog.background);
                if (bg != null)
                    seq.Join(bg);
            }

            // Content animation
            Tween contentTween = null;
            switch (kind)
            {
                case DialogAnimationKind.Slide:
                    contentTween = CreateSlideTween(dialog);
                    break;
                case DialogAnimationKind.Scale:
                    contentTween = CreateScaleTween(dialog);
                    break;
                case DialogAnimationKind.Fade:
                    contentTween = CreateContentFadeTween(dialog);
                    break;
            }

            if (contentTween != null)
                seq.Join(contentTween);

            return seq;
        }

        private Tween CreateSlideTween(Dialog dialog)
        {
            RectTransform rectTransform = dialog.content;
            if (rectTransform == null)
            {
                Debug.LogError("Dialog content must be a RectTransform for slide animations");
                return null;
            }

            Vector2 originalAnchoredPos = rectTransform.anchoredPosition;
            Vector2 startPos = originalAnchoredPos;

            float distance = slide.distance;

            if (slide.useNormalizedDistance)
            {
                Canvas canvas = dialog.GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    RectTransform canvasRect = canvas.transform as RectTransform;
                    if (canvasRect != null)
                    {
                        switch (slide.direction)
                        {
                            case DialogSlideDirection.Up:
                            case DialogSlideDirection.Down:
                                distance = canvasRect.rect.height * slide.distance;
                                break;
                            case DialogSlideDirection.Left:
                            case DialogSlideDirection.Right:
                                distance = canvasRect.rect.width * slide.distance;
                                break;
                        }
                    }
                }
            }

            // Calculate start position based on direction
            switch (slide.direction)
            {
                case DialogSlideDirection.Up:
                    startPos.y -= distance;
                    break;
                case DialogSlideDirection.Down:
                    startPos.y += distance;
                    break;
                case DialogSlideDirection.Left:
                    startPos.x += distance;
                    break;
                case DialogSlideDirection.Right:
                    startPos.x -= distance;
                    break;
            }

            rectTransform.anchoredPosition = startPos;

            return rectTransform.DOAnchorPos(originalAnchoredPos, slide.duration).SetEase(ease);
        }

        private Tween CreateScaleTween(Dialog dialog)
        {
            RectTransform rectTransform = dialog.content;
            if (rectTransform == null)
            {
                Debug.LogError("Dialog content must be a RectTransform for scale animations");
                return null;
            }

            rectTransform.localScale = new Vector3(scale.from.x, scale.from.y, 1f);

            return rectTransform.DOScale(new Vector3(scale.to.x, scale.to.y, 1f), scale.duration).SetEase(ease);
        }

        private Tween CreateFadeTween(Image bg)
        {
            if (bg == null) return null;

            Color c = bg.color;
            var alphaStart = fade.absolute ? fade.from : c.a * fade.from;
            var alphaEnd = fade.absolute ? fade.to : c.a * fade.to;

            c.a = alphaStart;
            bg.color = c;

            return bg.DOFade(alphaEnd, fade.duration).SetEase(fade.ease);
        }

        private Tween CreateContentFadeTween(Dialog dialog)
        {
            CanvasGroup canvasGroup = dialog.content.GetComponent<CanvasGroup>();
            
            if (canvasGroup == null)
            {
                canvasGroup = dialog.content.gameObject.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = fade.from;

            return canvasGroup.DOFade(fade.to, fade.duration).SetEase(fade.ease);
        }
    }
}