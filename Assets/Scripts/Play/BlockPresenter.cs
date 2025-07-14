using System.Collections.Generic;
using UnityEngine;

namespace Scene.Play
{
    /// <summary>
    /// 블럭 UI 생성 및 유저 입력 연결
    /// </summary>
    public class BlockPresenter : MonoBehaviour
    {
        [SerializeField] private Transform[] _blockSlots; // 3개 슬롯 위치
        [SerializeField] private GameObject _blockViewPrefab;

        private int placedCount = 0;

        public bool AreAllBlocksPlaced => placedCount >= 3;

        public void CreateAndShowBlocks(List<BlockModel> blocks)
        {
            placedCount = 0;

            for (int i = 0; i < blocks.Count && i < _blockSlots.Length; i++)
            {
                var blockGO = Instantiate(_blockViewPrefab, _blockSlots[i]);
                var blockView = blockGO.GetComponent<BlockView>();
                blockView.SetModel(blocks[i]);
                blockView.OnPlaced += OnBlockPlaced;
            }
        }

        private void OnBlockPlaced()
        {
            placedCount++;
        }
    }
}