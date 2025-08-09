using System;

namespace FishingBlast.Interfaces
{
    public interface IPopup
    {
        // PopupManager가 알아야 할 최소한의 기능
        void Open();
        void Close();
        void Back();

        // PopupManager가 팝업의 닫힘을 감지할 수 있어야 함
        event Action<IPopup> OnClosed;
    }
}
