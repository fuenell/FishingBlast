using FishingBlast.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FishingBlast.UI
{
    public class FishCollectionItem : MonoBehaviour
    {
        private FishMasterData _fishData;
        private PlayerFishRecord _fishRecord;

        [SerializeField]
        private TextMeshProUGUI _fishNameText;

        // 상세 보기창 열기 버튼
        [SerializeField]
        private Button _detailButton;

        public int FishId => _fishData.Id;

        public void SetDetailOpenEvent(UnityAction<FishMasterData, PlayerFishRecord> openFishDetail)
        {
            _detailButton.onClick.AddListener(() => openFishDetail.Invoke(_fishData, _fishRecord));
        }

        private void OnDestroy()
        {
            _detailButton.onClick.RemoveAllListeners();
        }

        public void SetMasterData(FishMasterData fishData)
        {
            _fishData = fishData;
            _fishNameText.text = _fishData.FishName;
        }

        public void SetRecord(PlayerFishRecord fishRecord)
        {
            _fishRecord = fishRecord;
        }
    }
}