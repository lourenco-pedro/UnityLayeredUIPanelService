using System;
using UnityEngine;

namespace ppl.ServiceManagement.LayeredUIService.Animations
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class BaseAnimation : MonoBehaviour
    {
        public RectTransform Transform;
        public abstract void Display(Action onFinished);
        public abstract void Reverse(Action onFinished);

        private void OnValidate()
        {
            if(null == Transform)
                Transform = GetComponent<RectTransform>();
        }
    }
}