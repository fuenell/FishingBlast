using FishingBlast.Data;
using System.Collections.Generic;
using UnityEngine;

namespace FishingBlast.Play
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
        public const int EmptyNum = -1;

        private int[,] _grid;
        public int[,] Grid { get => _grid; }

        public BlockBoard()
        {
            _grid = new int[BoardConfig.Width, BoardConfig.Height];

            // _grid를 emptynum으로 초기화
            for (int i = 0; i < BoardConfig.Width; i++)
            {
                for (int j = 0; j < BoardConfig.Height; j++)
                {
                    _grid[i, j] = EmptyNum;
                }
            }
        }

        public int[,] GetDeepCopyGrid()
        {
            return _grid.DeepCopy();
        }

        public void SetBoard(int[] gameBoard)
        {
            for (int i = 0; i < gameBoard.Length; i++)
            {
                // gameBoard를 BoardConfig.Width, BoardConfig.Height 크기에 맞춰서 _grid에 넣는다
                _grid[i % BoardConfig.Width, i / BoardConfig.Width] = gameBoard[i];
            }
        }

        public bool CanPlaceBlock(BlockModel block, Vector2Int gridPosition)
        {
            return CanPlaceBlock(block.Shape, ref _grid, gridPosition.x, gridPosition.y);
        }

        public void PlaceBlock(BlockModel block, Vector2Int gridPosition)
        {
            PlaceBlock(block.Shape, ref _grid, gridPosition.x, gridPosition.y, block.ColorIndex);
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
                    if (_grid[x, y] == EmptyNum)
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
                    if (_grid[x, y] == EmptyNum)
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
                    _grid[x, y] = EmptyNum;
                }
            }

            // 세로 줄 제거
            foreach (int x in matches.Columns)
            {
                for (int y = 0; y < height; y++)
                {
                    _grid[x, y] = EmptyNum;
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

        public MatchedResult GetMatchedLinesIfPlaced(BlockModel model, Vector2Int gridPosition)
        {
            int width = _grid.GetLength(0);
            int height = _grid.GetLength(1);
            var result = new MatchedResult();

            HashSet<Vector2Int> virtualFilledCells = new HashSet<Vector2Int>();

            foreach (var shape in model.Shape)
            {
                Vector2Int pos = gridPosition + shape;
                virtualFilledCells.Add(pos);
            }

            // 가로 줄 검사
            for (int y = 0; y < height; y++)
            {
                bool full = true;
                for (int x = 0; x < width; x++)
                {
                    var pos = new Vector2Int(x, y);
                    bool occupied = _grid[x, y] != EmptyNum || virtualFilledCells.Contains(pos);

                    if (!occupied)
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

            // 세로 줄 검사
            for (int x = 0; x < width; x++)
            {
                bool full = true;
                for (int y = 0; y < height; y++)
                {
                    var pos = new Vector2Int(x, y);
                    bool occupied = _grid[x, y] != EmptyNum || virtualFilledCells.Contains(pos);

                    if (!occupied)
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

        #region 로직
        private static void PlaceBlock(Vector2Int[] shape, ref int[,] grid, int x, int y, int colorIndex)
        {
            for (int i = 0; i < shape.Length; i++)
            {
                bool result = CanPlaceCell(grid, shape[i].x + x, shape[i].y + y);

                if (result)
                {
                    // 배치 가능한곳에 1 배치
                    grid[shape[i].x + x, shape[i].y + y] = colorIndex;
                }
            }
        }

        public static bool CanPlaceBlockAnyWhere(BlockModel blockModel, ref int[,] grid)
        {
            Vector2Int[] shape = blockModel.Shape;

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

        private static bool CanPlaceBlock(Vector2Int[] shape, ref int[,] grid, int x, int y)
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

        private static bool CanPlaceCell(int[,] grid, int x, int y)
        {
            // grid 밖이면 불가
            if (x < 0 || grid.GetLength(0) <= x)
            {
                return false;
            }
            if (y < 0 || grid.GetLength(1) <= y)
            {
                return false;
            }

            // grid 속 다른 블럭과 겹치면 불가
            if (grid[x, y] != EmptyNum)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}