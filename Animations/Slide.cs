using System;
using UnityEngine;
using DG.Tweening;


namespace ppl.ServiceManagement.LayeredUIService.Animations
{
    public class Slide : BaseAnimation
    {
        public override void Display(Action onFinished)
        {
            Vector2 startPosition = Vector2.right * Transform.rect.width;

            Transform.anchoredPosition = startPosition;

            Transform.DOAnchorPos(Vector2.zero, .2f);
        }
        
        public override void Reverse(Action onFinished)
        {
            Vector2 toPosition = Vector2.right * Transform.rect.width;

            Transform.anchoredPosition = Vector2.zero;
            
            Transform.DOAnchorPos(toPosition, 0.2f)
                .OnComplete(()=>
                {
                    onFinished?.Invoke();
                    GameObject.Destroy(gameObject, 2);
                });
        }
    }
}
