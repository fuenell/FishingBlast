using UnityEngine;

namespace Scene.Play
{
    public class BlockModel
    {
        private Vector2Int[] _shape;

        public Vector2Int[] GetShape()
        {
            return _shape;
        }

        public BlockModel(Vector2Int[] shape)
        {
            this._shape = shape;
        }
    }
}
