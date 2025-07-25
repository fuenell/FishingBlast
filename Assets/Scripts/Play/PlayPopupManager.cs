using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Scene.Play
{
    public class PlayPopupManager : ITickable
    {
        private readonly Dictionary<Type, BasePopup> _popups;

        private BasePopup _currentOpenPopup;

        public bool IsAnyPopupOpen => _currentOpenPopup != null;

        // IReadOnlyList<T>로 받는 것을 권장
        public PlayPopupManager(IReadOnlyList<BasePopup> popups)
        {
            _popups = popups.ToDictionary(p => p.GetType(), p => p);

            // 생성자에서 모든 팝업의 OnClosed 이벤트를 구독
            foreach (var popup in popups)
            {
                popup.OnClosed += OnPopupClosedBySelf;
            }
        }

        // Todo: 추후 인풋매니저로 분리 후 이벤트 연결
        public void Tick()
        {
            // 키보드의 Esc 키 또는 안드로이드의 뒤로 가기 버튼이 눌렸는지 확인
            if ((Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
                (Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame))
            {
                Back();
            }
        }

        public void Back()
        {
            // 닫힌 팝업이 현재 열려있던 팝업과 같다면, 상태를 동기화
            if (_currentOpenPopup != null)
            {
                _currentOpenPopup.Back();
            }
            else
            {
                Show<QuitPopup>();
            }
        }

        // 팝업이 스스로 닫혔을 때 호출될 메소드
        private void OnPopupClosedBySelf(BasePopup closedPopup)
        {
            // 닫힌 팝업이 현재 열려있던 팝업과 같다면, 상태를 동기화
            if (_currentOpenPopup == closedPopup)
            {
                _currentOpenPopup = null;
            }
        }

        public void Show<T>() where T : BasePopup
        {
            if (_currentOpenPopup != null)
            {
                _currentOpenPopup.Close();
                _currentOpenPopup = null;
            }

            if (_popups.TryGetValue(typeof(T), out var popup))
            {
                popup.Open();
                _currentOpenPopup = popup;
            }
            else
            {
                Debug.LogError($"해당 팝업이 없습니다: {typeof(T)}");
            }
        }

        public void Hide<T>() where T : BasePopup
        {
            if (_popups.TryGetValue(typeof(T), out var popup))
            {
                popup.Close();
            }
        }
    }
}
