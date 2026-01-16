using System;
using ppl.ServiceManagement.LayeredUIService.Animations;
using UnityEngine;

namespace ppl.ServiceManagement.LayeredUIService
{
    [RequireComponent(typeof(Slide))]
    [RequireComponent(typeof(Instant))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseLayeredPanel : MonoBehaviour, ILayeredPanel
    {
        public enum EntranceType
        {
            Instant,
            Slide
        }
        
        public static EntranceType CurrentEntranceType = EntranceType.Instant;
        
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
        public BaseLayeredPanel Panel => this;
        public int PanelId => _panelId;
        
        [Space]
        [SerializeField]
        private Slide _slide;
        [SerializeField]
        private Instant _instant;

        private int _panelId;

        private EntranceType _lastEntranceUsed;
        
        internal virtual void Start()
        {
            _lastEntranceUsed = CurrentEntranceType;
            
            switch (_lastEntranceUsed)
            {
                case EntranceType.Slide:
                    _slide.Display(()=>{});
                    break;
                default:
                case EntranceType.Instant:
                    _instant.Display(()=>{});
                    break;
            }
        }

        internal void ValidateReference(params MonoBehaviour[] component)
        {
            foreach (var compo in component)
            {
                if(null == compo)
                    throw new ArgumentNullException("component is null");
            }
        }

        public virtual void Setup(int panelId)
        {
            _panelId = panelId;
        }

        public void Close(Action onClose = null)
        {
            switch (_lastEntranceUsed)
            {
                case EntranceType.Slide:
                    _slide.Reverse(()=>{Destroy(gameObject);});
                    break;
                default:
                case EntranceType.Instant:
                    _instant.Reverse(()=>{Destroy(gameObject);});
                    break;
            }
        }

        private void OnValidate()
        {
            if(null == _slide)
                _slide = GetComponent<Slide>();
            
            if(null == _instant)
                _instant = GetComponent<Instant>();
        }
    }
}