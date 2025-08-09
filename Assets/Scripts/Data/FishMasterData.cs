using System.Collections.Generic;
using UnityEngine;

namespace FishingBlast.Data
{
    [CreateAssetMenu(fileName = "FishData_", menuName = "Game Data/Fish Master Data")]
    public class FishMasterData : ScriptableObject
    {
        // 데이터는 private으로 보호하고, [SerializeField]로 저장 대상으로 지정
        [SerializeField] private int _id;
        [SerializeField] private string _fishName;
        [SerializeField] private FishGrade _grade;
        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;
        [SerializeField] private FishSpawnRegion[] _spawnRegions;

        // 외부에서는 이 프로퍼티를 통해 '읽기'만 가능
        public int Id => _id;
        public string FishName => _fishName;
        public FishGrade Grade => _grade;
        public float MinSize => _minSize;
        public float MaxSize => _maxSize;
        public IReadOnlyList<FishSpawnRegion> SpawnRegions => _spawnRegions;

        // 에디터 스크립트가 값을 설정할 수 있도록 public 메소드 제공
        public void SetData(int id, string fishName, FishGrade grade, float minSize, float maxSize, FishSpawnRegion[] spawnRegions)
        {
            _id = id;
            _fishName = fishName;
            _grade = grade;
            _minSize = minSize;
            _maxSize = maxSize;
            _spawnRegions = spawnRegions;
        }
    }

    // 열거형(Enum)으로 데이터의 종류를 명확히 합니다.
    public enum FishGrade
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    public enum FishSpawnRegion
    {
        ShallowSea,
        DeepSea,
        CoralReef,
        ArcticOcean
    }
}