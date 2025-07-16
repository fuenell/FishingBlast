using System.Collections.Generic;
using UnityEngine;

namespace Scene.Play
{
    /// <summary>
    /// 블럭 UI 생성 및 유저 입력 연결
    /// </summary>
    public class BlockQueuePresenter : MonoBehaviour
    {
        [SerializeField] private Transform[] _blockSlots; // 3개 슬롯 위치
        [SerializeField] private GameObject _blockViewPrefab;

        private List<BlockModel> _blocks; // 현재 화면에 표시된 블럭들
        private int placedCount = 0;

        public bool AreAllBlocksPlaced => _blocks.Count == 0;
        public List<BlockModel> RemainBlockList => _blocks;

        public void CreateAndShowBlocks(List<BlockModel> blocks)
        {
            _blocks = blocks;

            for (int i = 0; i < blocks.Count && i < _blockSlots.Length; i++)
            {
                var blockGO = Instantiate(_blockViewPrefab, _blockSlots[i]);
                var blockView = blockGO.GetComponent<BlockView>();
                blockView.SetModel(blocks[i]);
            }
        }

        public void RemoveBlock(BlockModel placedBlock)
        {
            if (_blocks.Contains(placedBlock))
            {
                // 블럭 제거
                _blocks.Remove(placedBlock);
            }
        }
    }
}