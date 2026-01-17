using System;
using UnityEngine;

namespace ppl.ServiceManagement.LayeredUIService.Animations
{
    public class Slide : BaseAnimation
    {
        public override void Display(Action onFinished)
        {
            RectTransform rectTransform = transform as RectTransform;

            if (null == rectTransform)
                return;

            Vector2 startPosition = Vector2.right * rectTransform.rect.width;

            rectTransform.anchoredPosition = startPosition;
            rectTransform.AnimatePosition(Vector2.zero, .2f);
        }

        public override void Reverse(Action onFinished)
        {
            RectTransform rectTransform = transform as RectTransform;

            if (null == rectTransform)
                return;

            Vector2 toPosition = Vector2.right * rectTransform.rect.width;

            rectTransform.anchoredPosition = Vector2.zero;

            rectTransform.AnimatePosition(toPosition, 0.2f, () =>
                {
                    onFinished?.Invoke();
                    GameObject.Destroy(gameObject, 2);
                });
        }
    }
}
