using FishingBlast.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace FishingBlast.UI
{
    public class FishCollectionPopup : BasePopup
    {
        [SerializeField] FishDatabase _fishDatabase;

        [SerializeField] Transform _itemListRoot;
        [SerializeField] GameObject _fishCollectionItemPrefab;

        [SerializeField] FishDetailPopup _fishDetailPopup;

        [SerializeField] Button _closeButton;

        private List<FishCollectionItem> _fishCollectionItemList = new List<FishCollectionItem>();
        private List<PlayerFishRecord> _fishRecordList;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Close);
        }

        [Inject]
        public void Construct(PlayerData playerData)
        {
            // 도감 데이터 주입 받기
            _fishRecordList = playerData.CaughtFishes;
        }

        protected override void OnOpen()
        {
            _fishDetailPopup.Close();

            // 도감 목록 생성
            foreach (FishMasterData fishData in _fishDatabase.GetAllFish())
            {
                // 이미 만들어져 있으면 생략
                if (_fishCollectionItemList.Exists(item => item.FishId == fishData.Id))
                {
                    continue;
                }

                FishCollectionItem fishItem = Instantiate(_fishCollectionItemPrefab, _itemListRoot).GetComponent<FishCollectionItem>();
                fishItem.SetMasterData(fishData);
                fishItem.SetDetailOpenEvent(OpenFishDetail);
                _fishCollectionItemList.Add(fishItem);
            }

            // 도감 목록에 있는 물고기에 대응하는 데이터를 삽입
            foreach (PlayerFishRecord fishRecord in _fishRecordList)
            {
                FishCollectionItem fishItem = _fishCollectionItemList.Find(f => f.FishId == fishRecord.Id);
                if (fishItem != null)
                {
                    fishItem.SetRecord(fishRecord);
                }
            }
        }

        private void OpenFishDetail(FishMasterData fishData, PlayerFishRecord fishRecord)
        {
            _fishDetailPopup.SetData(fishData, fishRecord);
            _fishDetailPopup.Open();
        }

        public override void Back()
        {
            // 상세 팝업이 열려있으면 닫기
            if (_fishDetailPopup.IsOpen)
            {
                _fishDetailPopup.Back();
            }
            else
            {
                Close();
            }
        }
    }
}
