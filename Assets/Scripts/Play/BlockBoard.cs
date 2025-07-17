using System.Collections.Generic;
using UnityEngine;

namespace Scene.Play
{
    public static class ArrayExtensions
    {
        public static T[,] DeepCopy<T>(this T[,] source)
        {
            int rows = source.GetLength(0);
            int cols = source.GetLength(1);
            var result = new T[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = source[i, j];

            return result;
        }
    }

    /// <summary>
    /// 매치된 줄 정보를 보관합니다.
    /// Rows에는 가로 줄 인덱스, Columns에는 세로 줄 인덱스가 담깁니다.
    /// </summary>
    public class MatchedResult
    {
        public List<int> Rows { get; } = new List<int>();
        public List<int> Columns { get; } = new List<int>();
        public bool IsEmpty => Rows.Count == 0 && Columns.Count == 0;
    }

    /// <summary>
    /// 게임 로직 담당 (배치, 매칭, 게임오버 판단)
    /// </summary>
    public class BlockBoard
    {
        private bool[,] _grid = new bool[BoardConfig.Width, BoardConfig.Height];

        public bool[,] GetDeepCopyGrid()
        {
            return _grid.DeepCopy();
        }

        public bool CanPlaceBlock(BlockModel block, Vector2Int position)
        {
            return CanPlaceBlock(block.GetShape(), ref _grid, position.x, position.y);
        }

        public void PlaceBlock(BlockModel block, Vector2Int position)
        {
            PlaceBlock(block.GetShape(), ref _grid, position.x, position.y);
        }

        /// <summary>
        /// 매치된 가로/세로 줄 인덱스를 찾습니다.
        /// </summary>
        public MatchedResult GetMatchedLines()
        {
            int width = _grid.GetLength(0);
            int height = _grid.GetLength(1);
            var result = new MatchedResult();

            // 가로 매치 검사
            for (int y = 0; y < height; y++)
            {
                bool full = true;
                for (int x = 0; x < width; x++)
                {
                    if (!_grid[x, y])
                    {
                        full = false;
                        break;
                    }
                }
                if (full)
                {
                    result.Rows.Add(y);
                }
            }

            // 세로 매치 검사
            for (int x = 0; x < width; x++)
            {
                bool full = true;
                for (int y = 0; y < height; y++)
                {
                    if (!_grid[x, y])
                    {
                        full = false;
                        break;
                    }
                }
                if (full)
                {
                    result.Columns.Add(x);
                }
            }

            return result;
        }

        /// <summary>
        /// 지정된 MatchedResult에 따라 격자에서 해당 줄을 제거(비움)합니다.
        /// </summary>
        public void ClearMatches(MatchedResult matches)
        {
            int width = _grid.GetLength(0);
            int height = _grid.GetLength(1);

            // 가로 줄 제거
            foreach (int y in matches.Rows)
            {
                for (int x = 0; x < width; x++)
                {
                    _grid[x, y] = false;
                }
            }

            // 세로 줄 제거
            foreach (int x in matches.Columns)
            {
                for (int y = 0; y < height; y++)
                {
                    _grid[x, y] = false;
                }
            }
        }

        public bool IsGameOver(List<BlockModel> nextBlocks)
        {
            for (int i = 0; i < nextBlocks.Count; i++)
            {
                if (CanPlaceBlockAnyWhere(nextBlocks[i], ref _grid))
                {
                    return false; // 배치 가능한 블럭이 하나라도 있으면 게임 오버 아님
                }
            }

            return true;
        }

        #region 로직
        private static void PlaceBlock(Vector2Int[] shape, ref bool[,] grid, int x, int y)
        {
            for (int i = 0; i < shape.Length; i++)
            {
                bool result = CanPlaceCell(grid, shape[i].x + x, shape[i].y + y);

                if (result)
                {
                    // 배치 가능한곳에 1 배치
                    grid[shape[i].x + x, shape[i].y + y] = true;
                }
            }
        }

        public static bool CanPlaceBlockAnyWhere(BlockModel blockModel, ref bool[,] grid)
        {
            Vector2Int[] shape = blockModel.GetShape();

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    bool result = CanPlaceBlock(shape, ref grid, x, y);
                    if (result)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool CanPlaceBlock(Vector2Int[] shape, ref bool[,] grid, int x, int y)
        {
            for (int i = 0; i < shape.Length; i++)
            {
                bool result = CanPlaceCell(grid, shape[i].x + x, shape[i].y + y);

                if (result == false)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CanPlaceCell(bool[,] grid, int x, int y)
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
            if (grid[x, y] == true)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}