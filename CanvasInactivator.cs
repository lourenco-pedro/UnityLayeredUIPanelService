using System;
using System.Collections;
using UnityEngine;

namespace ppl.ServiceManagement.LayeredUIService
{
    public class CanvasInactivator : MonoBehaviour
    {
        Coroutine _inactivatorDisposable;
        WaitForSeconds _waitForSeconds = new WaitForSeconds(1f);

        void OnEnable()
        {
            _inactivatorDisposable = StartCoroutine(IEDisableCheck());
        }

        private IEnumerator IEDisableCheck()
        {
            yield return _waitForSeconds;
            if (transform.childCount == 0)
                gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (_inactivatorDisposable != null)
                StopCoroutine(_inactivatorDisposable);
        }
    }
}