using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Services.LayeredUIService.Implementation
{
    public class LayeredUIService : ILayeredUIService
    {
        const int REFERENCE_WIDTH = 2430;
        const int REFERENCE_HEIGHT = 1080;
        
        static List<Canvas> _canvasPool = new List<Canvas>();
        static Dictionary<int, BaseLayeredPanel> _panels = new Dictionary<int, BaseLayeredPanel>();
        
        public string Name => nameof(LayeredUIService);
        
        public Task AsyncSetup()
        {
            return Task.CompletedTask;
        }
        
        public int UseCanvas(ILayeredPanel ipanel, BaseLayeredPanel.EntranceType entranceType = BaseLayeredPanel.EntranceType.Instant)
        {
            Canvas higherCanvas = _canvasPool[HigherActiveCanvas()];
            
            BaseLayeredPanel.CurrentEntranceType = entranceType;
            
            var instantiatedPanel = GameObject.Instantiate(ipanel.Panel, higherCanvas.transform);
            
            instantiatedPanel.transform.localScale = Vector3.one;
            instantiatedPanel.transform.localPosition = Vector3.zero;
            UniTask.SwitchToMainThread();
            
            int hashCode = instantiatedPanel.GetHashCode();
            
            ipanel.Setup(hashCode);
            _panels.Add(hashCode, instantiatedPanel);
            
            return hashCode;
        }

        public int UseHigherCanvas(ILayeredPanel ipanel)
        {
            Canvas pooledCanvas =_canvasPool[PoolCanvas()];
            pooledCanvas.gameObject.SetActive(true);
            pooledCanvas.transform.SetAsLastSibling();
            var instantiatedPanel = GameObject.Instantiate(ipanel.Panel, pooledCanvas.transform);
            instantiatedPanel.gameObject.SetActive(true);
            
            ipanel.Transform.localScale = Vector3.one;
            ipanel.Transform.localPosition = Vector3.zero;
            UniTask.SwitchToMainThread();
            
            int hashCode = instantiatedPanel.GetHashCode();
            
            ipanel.Setup(hashCode);
            _panels.Add(hashCode, instantiatedPanel);
            
            return hashCode;
        }

        public BaseLayeredPanel GetPanel(int panelId)
        {
            if (_panels.TryGetValue(panelId, out var panel))
                return panel;

            return null;
        }

        public void Close(int panelId)
        {
            _panels[panelId].Close(() =>
            {
                _panels.Remove(panelId);
            });
        }

        void HideAllCanvas()
        {
            foreach (Canvas canvas in _canvasPool)
            {
                canvas.gameObject.SetActive(false);
            }
        }
        
        int HigherActiveCanvas()
        {
            if (_canvasPool.Count == 0)
            {
                PoolCanvas();
                return 0;
            }
            
            if (!_canvasPool.Any(canvas => canvas.gameObject.activeSelf))
            {
                _canvasPool[0].gameObject.SetActive(true);
                return 0;
            }
            
            int higherSiblingIndex = _canvasPool
                .Where(canvas => canvas.gameObject.activeSelf)
                .Max(canvas => canvas.transform.GetSiblingIndex());
            
            int canvasIndex = _canvasPool.FindIndex(canvas => canvas.transform.GetSiblingIndex() == higherSiblingIndex);
            return canvasIndex;
        }

        int PoolCanvas(bool startInactive = false)
        {
            if (_canvasPool.Count > 0)
            {
                var possibleCanvas = _canvasPool.FirstOrDefault(canvas => !canvas.gameObject.activeSelf);
                int index = possibleCanvas != default ? _canvasPool.IndexOf(possibleCanvas) : -1;
                if (index >= 0)
                {
                    return index;
                }
            }
            
            Debug.Log("Pooling canvas");
            
            string name = "[LAYER]";
            Canvas newCanvas = new GameObject(name).AddComponent<Canvas>();
            CanvasScaler canvasScaler = newCanvas.AddComponent<CanvasScaler>();
            CanvasInactivator canvasInactivator = newCanvas.AddComponent<CanvasInactivator>();
            GraphicRaycaster graphicRaycaster = newCanvas.AddComponent<GraphicRaycaster>();
            
            newCanvas.gameObject.SetActive(startInactive == false);
            newCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(REFERENCE_WIDTH, REFERENCE_HEIGHT);
            
            _canvasPool.Add(newCanvas);
            
            return _canvasPool.Count - 1;
        }

#if UNITY_EDITOR
        public void DebugService()
        {
            EditorGUILayout.LabelField(nameof(LayeredUIService));
        }
#endif
    }    
}
