using System;
using UnityEngine;
using DG.Tweening;

namespace ppl.ServiceManagement.LayeredUIService.Animations
{
    public class Slide : BaseAnimation
    {
        public override void Display(Action onFinished)
        {
            RectTransform rectTransform = Transform;
            Vector2 startPosition = Vector2.right * rectTransform.rect.width;

            rectTransform.anchoredPosition = startPosition;

            rectTransform.DOAnchorPos(Vector2.zero, .2f);
        }
        
        public override void Reverse(Action onFinished)
        {
            RectTransform rectTransform = Transform;
            Vector2 toPosition = Vector2.right * rectTransform.rect.width;

            rectTransform.anchoredPosition = Vector2.zero;
            
            rectTransform.DOAnchorPos(toPosition, 0.2f)
                .OnComplete(()=>
                {
                    onFinished?.Invoke();
                    GameObject.Destroy(gameObject, 2);
                });
        }
    }
}
