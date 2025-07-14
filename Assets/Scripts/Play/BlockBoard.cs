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

        public bool CanBatch(BlockModel blockModel, ref int[,] grid)
        {
            Vector2Int[] shape = blockModel.GetShape();

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    bool result = CanBatch(shape, ref grid, x, y);
                    if (result)
                    {
                        SetBatch(shape, ref grid, x, y);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CanBatch(Vector2Int[] shape, ref int[,] grid, int x, int y)
        {
            for (int i = 0; i < shape.Length; i++)
            {
                bool result = CanBatchCell(grid, shape[i].x + x, shape[i].y + y);

                if (result == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CanBatchCell(int[,] grid, int x, int y)
        {
            // grid안에 있고
            if (x < 0 || grid.GetLength(0) <= x)
            {
                return false;
            }
            if (y < 0 || grid.GetLength(1) <= y)
            {
                return false;
            }

            // grid 속 다른 블럭과 겹치지 않는지
            if (grid[x, y] != 0)
            {
                return false;
            }

            return true;
        }

        private void SetBatch(Vector2Int[] shape, ref int[,] grid, int x, int y)
        {
            for (int i = 0; i < shape.Length; i++)
            {
                bool result = CanBatchCell(grid, shape[i].x + x, shape[i].y + y);

                if (result)
                {
                    // 배치 가능한곳에 1 배치
                    grid[shape[i].x + x, shape[i].y + y] = 1;
                }
            }
        }

        public int[,] GetGrid()
        {
            return grid;
        }
    }
}