using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FishingBlast.Data
{
    [CreateAssetMenu(fileName = "FishDatabase", menuName = "Game Data/Fish Database")]
    public class FishDatabase : ScriptableObject
    {
        [SerializeField]
        private List<FishMasterData> _allFish = new List<FishMasterData>();

        private Dictionary<int, FishMasterData> _fishDictionary;

        private void OnEnable()
        {
            // 딕셔너리 초기화 로직은 그대로 유지
            _fishDictionary = _allFish?.ToDictionary(fish => fish.Id, fish => fish);
        }

        public FishMasterData GetFishByID(int id)
        {
            if (_fishDictionary == null) OnEnable(); // 혹시 모를 초기화 누락 방지
            _fishDictionary.TryGetValue(id, out FishMasterData fish);
            return fish;
        }

        // 외부에는 IReadOnlyList로 안전하게 공개
        public IReadOnlyList<FishMasterData> GetAllFish()
        {
            return _allFish;
        }

        public void AddFish(FishMasterData fishInstance)
        {
            _allFish.Add(fishInstance);
        }

        public void ClearAll()
        {
            _allFish.Clear();
            if (_fishDictionary != null)
            {
                _fishDictionary.Clear();
            }
        }
    }
}