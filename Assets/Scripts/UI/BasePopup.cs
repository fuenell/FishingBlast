using System;
using UnityEngine;

namespace FishingBlast.UI
{
    public abstract class BasePopup : MonoBehaviour
    {
        public event Action<BasePopup> OnClosed;

        public bool IsOpen => this.gameObject.activeSelf;

        public void Initialize()
        {
            this.gameObject.SetActive(false);
        }

        public void Open()
        {
            this.gameObject.SetActive(true);
            OnOpen();
        }

        public void Close()
        {
            this.gameObject.SetActive(false);
            OnClose();
            OnClosed?.Invoke(this);
        }

        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }

        public virtual void Back()
        {
            Close();
        }
    }
}