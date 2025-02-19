using System;
using UnityEngine;

namespace Services.LayeredUIService
{
    public interface ILayeredPanel
    {
        Transform Transform { get; }
        GameObject GameObject { get; }
        BaseLayeredPanel Panel { get; }
        int PanelId { get; }

        void Setup(int panelId);
        void Close(Action onClose);
    }
}