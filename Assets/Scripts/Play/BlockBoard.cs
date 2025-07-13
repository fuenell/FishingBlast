using System.Collections.Generic;
using UnityEngine;

namespace Scene.Play
{
    /// <summary>
    /// 게임 로직 담당 (배치, 매칭, 게임오버 판단)
    /// </summary>
    public class BlockBoard
    {
        private int[,] grid = new int[8, 8];

        public void PlaceBlock(BlockModel model, Vector2Int position)
        {
            //foreach (var offset in model._shape)
            //{
            //    var gridPos = position + offset;
            //    grid[gridPos.x, gridPos.y] = 1;
            //}
        }

        public void CheckMatches()
        {
            // 가로/세로 한 줄 체크
        }

        public bool IsGameOver(List<BlockModel> nextBlocks)
        {
            // 배치 가능한 자리가 하나도 없으면 true
            return false;
        }
    }
}