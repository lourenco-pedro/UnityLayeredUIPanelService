using System;
using R3;
using UnityEngine;

namespace Services.LayeredUIService
{
    public class CanvasInactivator : MonoBehaviour
    {
        
        IDisposable _inactivatorDisposable;
        
        void OnEnable()
        {
            _inactivatorDisposable = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Subscribe((_)=> DisableInactiveCanvas());
        }

        private void OnDisable()
        {
            _inactivatorDisposable?.Dispose();
        }

        void DisableInactiveCanvas()
        {
            if(transform.childCount == 0)
                gameObject.SetActive(false);
        }
    }
}