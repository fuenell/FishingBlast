using FishingBlast.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer; // IObjectResolver를 위해 추가
using VContainer.Unity; // ITickable, IDisposable을 위해 추가

namespace FishingBlast.AppScope
{
    public class PopupManager : ITickable, IDisposable
    {
        private readonly IObjectResolver _container;
        private readonly InputService _inputService;

        // 프리팹 원본을 보관하는 딕셔너리 (키: 프리팹 이름, 값: 프리팹)
        private readonly Dictionary<string, IPopup> _popupPrefabs;

        // 생성된 팝업 인스턴스를 캐싱하는 딕셔너리 (키: 클래스 이름, 값: 인스턴스)
        private readonly Dictionary<string, IPopup> _cachedPopups;

        private Transform _popupRoot;
        private IPopup _currentOpenPopup;
        private IBackButtonHandler _currentBackButtonHandler;

        public bool IsAnyPopupOpen => _currentOpenPopup != null;

        public PopupManager(IObjectResolver container, InputService inputService)
        {
            _container = container;
            _inputService = inputService;

            _cachedPopups = new Dictionary<string, IPopup>();
            _popupPrefabs = new Dictionary<string, IPopup>();

            _popupRoot = _container.Instantiate(Resources.Load<GameObject>("Popups/PopupCanvas")).transform;

            // Resources/Popups 폴더에서 모든 IPopup 컴포넌트를 가진 프리팹을 로드
            var prefabs = Resources.LoadAll<GameObject>("Popups");
            foreach (var prefab in prefabs)
            {
                // PopupCanvas 자체는 팝업이 아니므로 건너뜁니다.
                if (prefab.name == "PopupCanvas")
                {
                    continue;
                }

                // 2. 로드한 GameObject가 IPopup 인터페이스를 가지고 있는지 확인합니다.
                if (prefab.TryGetComponent<IPopup>(out var popupComponent))
                {
                    // 3. IPopup을 가지고 있다면, 딕셔너리에 추가합니다.
                    _popupPrefabs[prefab.name] = popupComponent;
                }
            }
        }

        public void Dispose()
        {
            // 게임이 종료될 때, 생성된 모든 팝업의 이벤트 구독을 안전하게 해제
            foreach (var popup in _cachedPopups.Values)
            {
                if (popup != null)
                {
                    popup.OnClosed -= OnPopupClosedBySelf;
                }
            }
        }

        public void Tick()
        {
            // 매 프레임 뒤로가기 입력을 확인
            if (_inputService.IsPressEscape())
            {
                Back();
            }
        }

        // 외부에서 '뒤로가기 담당자'를 등록하거나 해제할 수 있는 public 메소드
        public void SetBackButtonHandler(IBackButtonHandler handler)
        {
            _currentBackButtonHandler = handler;
        }

        /// <summary>
        /// 뒤로가기 로직을 처리합니다. 열린 팝업이 있으면 닫고, 없으면 종료 팝업을 엽니다.
        /// </summary>
        public void Back()
        {
            if (_currentOpenPopup != null)
            {
                _currentOpenPopup.Back();
            }
            else if (_currentBackButtonHandler != null)
            {
                _currentBackButtonHandler.HandleBackButtonOnTop();
            }
        }

        /// <summary>
        /// 특정 타입의 팝업을 엽니다. 이미 열린 팝업이 있다면 먼저 닫습니다.
        /// </summary>
        /// <typeparam name="T">열고 싶은 팝업의 클래스 타입</typeparam>
        /// <returns>생성되거나 활성화된 팝업 인스턴스</returns>
        public void Show<T>() where T : Component, IPopup
        {
            // 다른 팝업이 열려있으면 먼저 닫아줌
            if (_currentOpenPopup != null)
            {
                _currentOpenPopup.Close();
                _currentOpenPopup = null;
            }

            string popupName = typeof(T).Name;

            // 이미 한번 생성해서 캐싱해둔 팝업이 있는지 확인
            if (_cachedPopups.TryGetValue(popupName, out IPopup cachedPopup))
            {
                cachedPopup.Open();
                _currentOpenPopup = cachedPopup;
                return;
            }

            // 캐시에 없다면, 프리팹 원본이 있는지 확인
            if (_popupPrefabs.TryGetValue(popupName, out IPopup prefab))
            {
                // 프리팹을 Instantiate 하고, VContainer에 의존성 주입을 요청 (핵심!)
                T newInstance = _container.Instantiate((T)prefab, _popupRoot) as T;

                // 생성된 팝업을 관리 목록(캐시)에 추가
                _cachedPopups[popupName] = newInstance;
                newInstance.OnClosed += OnPopupClosedBySelf;

                // 팝업 열기
                newInstance.Open();
                _currentOpenPopup = newInstance;
            }
            else
            {
                Debug.LogError($"'{popupName}' 팝업이 리소스 폴더에 없습니다");
            }
        }

        /// <summary>
        /// 특정 타입의 팝업을 닫습니다.
        /// </summary>
        /// <typeparam name="T">닫고 싶은 팝업의 클래스 타입</typeparam>
        public void Hide<T>() where T : IPopup
        {
            string popupName = typeof(T).Name;
            if (_cachedPopups.TryGetValue(popupName, out var popup))
            {
                popup.Close();
            }
        }

        // 팝업이 스스로 닫혔을 때(예: Cancel 버튼 클릭) 호출될 콜백 메소드
        private void OnPopupClosedBySelf(IPopup closedPopup)
        {
            // 닫힌 팝업이 현재 열려있던 팝업과 같다면, 현재 열린 팝업 상태를 null로 동기화
            if (_currentOpenPopup == closedPopup)
            {
                _currentOpenPopup = null;
            }
        }
    }
}