using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace ppl.ServiceManagement.LayeredUIService.Animations
{
    public class Instant : BaseAnimation
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        public override void Display(Action onFinished)
        {
            if (null == _canvasGroup)
                _canvasGroup = GetComponent<CanvasGroup>();

            if (null == _canvasGroup)
                return;

            StartCoroutine(IEFadeIn(onFinished));
        }

        public override void Reverse(Action onFinished)
        {
            if (null == _canvasGroup)
                _canvasGroup = GetComponent<CanvasGroup>();

            if (null == _canvasGroup)
                return;

            StartCoroutine(IEFadeOut(onFinished));
        }

        IEnumerator IEFadeIn(Action onFinished)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.transform.localScale = Vector2.one * 1.5f;
            
            yield return new WaitForEndOfFrame();

            _canvasGroup.transform.DOScale(Vector3.one, 0.2f);
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1, .2f).OnComplete(()=> onFinished?.Invoke());
        }
        
        IEnumerator IEFadeOut(Action onFinished)
        {
            _canvasGroup.alpha = 1;
            yield return new WaitForEndOfFrame();
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0, .2f).OnComplete(()=> onFinished?.Invoke());
        }
    }
}