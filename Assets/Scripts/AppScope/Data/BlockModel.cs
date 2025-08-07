using System;
using UnityEngine;

namespace AppScope.Data
{
    [Serializable]
    public class BlockModel
    {
        [SerializeField] private int _colorIndex;
        [SerializeField] private Vector2Int[] _shape;

        public int ColorIndex => _colorIndex;
        public Vector2Int[] Shape => _shape;

        public BlockModel() { }

        public BlockModel(Vector2Int[] shape, int colorIndex)
        {
            _shape = shape;
            _colorIndex = colorIndex;
        }
    }
}
