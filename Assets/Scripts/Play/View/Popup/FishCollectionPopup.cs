using FishingBlast.AppScope;
using FishingBlast.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace FishingBlast.Play
{
    public class FishCollectionPopup : BasePopup
    {
        [SerializeField] GameObject _root;
        [SerializeField] GameObject _test;

        [SerializeField] Button _closeButton;

        private DataManager _dataManager;
        private List<FishData> _fishDataList;

        private void Awake()
        {
            _root.SetActive(false);
            _closeButton.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Close);
        }

        [Inject]
        public void Construct(DataManager dataManager)
        {
            // 도감 데이터 주입 받기
            _dataManager = dataManager;
            _fishDataList = _dataManager.GetPlayerData().CaughtFishes;
        }

        public override void Open()
        {
            _root.SetActive(true);
            _test.SetActive(true);

            // 도감 목록 생성
            // 이미 만들어져 있으면 생략

            // 도감 목록에 있는 물고기에 대응하는 데이터를 삽입
            for (int i = 0; i < _fishDataList.Count; i++)
            {

            }


        }

        protected override void OnClose()
        {
            _root.SetActive(false);
        }

        public override void Back()
        {
            if (_test.activeSelf)
            {
                _test.SetActive(false);
            }
            else
            {
                Close();
            }
        }
    }
}
