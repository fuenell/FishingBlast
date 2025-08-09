using System;
using UnityEngine;

namespace FishingBlast.Data
{
    [Serializable]
    public class FishData
    {
        [SerializeField] private int _id;
        [SerializeField] private int _count;
        [SerializeField] private float _maxSize;

        public int Id => _id;
        public int Count => _count;
        public float MaxSize => _maxSize;

        public FishData() { }

        public FishData(int id, int count, float size)
        {
            _id = id;
            _count = count;
            _maxSize = size;
        }

        // 물고기를 더 잡았을 때 호출되는 메소드
        public void AddNewRecord(float newSize)
        {
            _count++;
            if (newSize > _maxSize)
            {
                _maxSize = newSize;
            }
        }
    }
}
