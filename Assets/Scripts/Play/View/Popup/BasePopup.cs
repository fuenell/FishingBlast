using System;
using UnityEngine;

namespace FishingBlast.Play
{
    public abstract class BasePopup : MonoBehaviour
    {
        // 자기가 닫혔을 때 외부에 알리기 위한 이벤트
        // Action<BasePopup>으로 자신을 넘겨주면 매니저가 어떤 팝업이 닫혔는지 알 수 있음
        public event Action<BasePopup> OnClosed;

        public abstract void Open();

        public void Close()
        {
            OnClose();
            OnClosed?.Invoke(this);
        }

        protected abstract void OnClose();

        public virtual void Back()
        {
            Close();
        }
    }
}