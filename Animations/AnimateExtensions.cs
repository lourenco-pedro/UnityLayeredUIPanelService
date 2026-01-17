using System;
using System.Collections;
using UnityEngine;

namespace ppl.ServiceManagement.LayeredUIService.Animations
{
    public static class AnimateExtensions
    {
        /// <summary>
        /// Animates a Transform's local scale from its current value to a target scale over the specified duration.
        /// </summary>
        /// <param name="transform">The transform to animate.</param>
        /// <param name="toScale">The target scale value.</param>
        /// <param name="duration">The duration of the animation in seconds.</param>
        /// <param name="onFinished">Optional callback invoked when the animation completes.</param>
        public static void AnimateLocalScale(this Transform transform, Vector3 toScale, float duration, Action onFinished = null)
        {
            transform.gameObject
                .GetComponent<MonoBehaviour>()
                .StartCoroutine(IEScale(transform, toScale, duration, onFinished));    
        }

        /// <summary>
        /// Animates a Transform's position from its current value to a target position over the specified duration.
        /// </summary>
        /// <param name="transform">The transform to animate.</param>
        /// <param name="toPosition">The target position value.</param>
        /// <param name="duration">The duration of the animation in seconds.</param>
        /// <param name="onFinished">Optional callback invoked when the animation completes.</param>
        public static void AnimatePosition(this Transform transform, Vector3 toPosition, float duration, Action onFinished = null)
        {
            transform.gameObject
                .GetComponent<MonoBehaviour>()
                .StartCoroutine(IEPosition(transform, toPosition, duration, isLocal: false, onFinished));
        }

        /// <summary>
        /// Animates a Transform's local position from its current value to a target position over the specified duration.
        /// </summary>
        /// <param name="transform">The transform to animate.</param>
        /// <param name="toPosition">The target local position value.</param>
        /// <param name="duration">The duration of the animation in seconds.</param>
        /// <param name="onFinished">Optional callback invoked when the animation completes.</param>
        public static void AnimateLocalPosition(this Transform transform, Vector3 toPosition, float duration, Action onFinished = null)
        {
            transform.gameObject
                .GetComponent<MonoBehaviour>()
                .StartCoroutine(IEPosition(transform, toPosition, duration, isLocal: true, onFinished));
        }

       /// <summary>
        /// Animates an integer value from its current state to a target value over the specified duration.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour to run the coroutine on.</param>
        /// <param name="target">Function that returns the current value.</param>
        /// <param name="step">Action called each frame with the interpolated value.</param>
        /// <param name="to">The target value.</param>
        /// <param name="duration">The duration of the animation in seconds.</param>
        /// <param name="onFinished">Optional callback invoked when the animation completes.</param>
        public static void To(this MonoBehaviour monoBehaviour, Func<int> target, Action<int> step, int to, float duration, Action onFinished = null)
        {
            monoBehaviour.StartCoroutine(IETo(target, step, to, duration, onFinished));
        }

        /// <summary>
        /// Animates a float value from its current state to a target value over the specified duration.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour to run the coroutine on.</param>
        /// <param name="target">Function that returns the current value.</param>
        /// <param name="step">Action called each frame with the interpolated value.</param>
        /// <param name="to">The target value.</param>
        /// <param name="duration">The duration of the animation in seconds.</param>
        /// <param name="onFinished">Optional callback invoked when the animation completes.</param>
        public static void To(this MonoBehaviour monoBehaviour, Func<float> target, Action<float> step, float to, float duration, Action onFinished = null)
        {
            monoBehaviour.StartCoroutine(IETo(target, step, to, duration, onFinished));
        }

        static IEnumerator IETo(Func<int> target, Action<int> step, int to, float duration, Action onFinished = null)
        {
            float elapsed = 0f;
            int from = target();
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                int value = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
                step(value);
                yield return null;
            }

            step(to);
            onFinished?.Invoke();
        }

        static IEnumerator IETo(Func<float> target, Action<float> step, float to, float duration, Action onFinished = null)
        {
            float elapsed = 0f;
            float from = target();
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float value = Mathf.Lerp(from, to, t);
                step(value);
                yield return null;
            }

            step(to);
            onFinished?.Invoke();
        }

         static IEnumerator IEPosition(Transform transform, Vector3 toPosition, float duration, bool isLocal, Action onFinished = null)
        {
            Vector3 fromPosition = isLocal ? transform.localPosition : transform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                if (isLocal)
                    transform.localPosition = Vector3.Lerp(fromPosition, toPosition, t);
                else
                    transform.position = Vector3.Lerp(fromPosition, toPosition, t);
                yield return null;
            }

            
            if (isLocal)
                transform.localPosition = toPosition;
            else
                transform.position = toPosition;
            
            onFinished?.Invoke();
        }

        static IEnumerator IEScale(Transform transform, Vector3 toScale, float duration, Action onFinished = null)
        {
            Vector3 fromScale = transform.localScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                transform.localScale = Vector3.Lerp(fromScale, toScale, t);
                yield return null;
            }

            transform.localScale = toScale;
            onFinished?.Invoke();
        }
    }
}
