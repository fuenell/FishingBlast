using FishingBlast.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FishingBlast.Play
{
    /// <summary>
    /// 배치 가능한 무작위 블럭 선택
    /// 새로운 블럭 모델 데이터 생성
    /// </summary>
    public class BlockGenerator
    {
        // 대표적인 3x3 블럭들 (원하는 만큼 추가 가능)
        private static readonly Vector2Int[][] Patterns = new Vector2Int[][]
        {
            // 1x1
            new Vector2Int[] { new(0,0) },
            
            // 2x2
            new Vector2Int[] { new(0,0), new(1,0), new(0,1), new(1,1) },
            new Vector2Int[] { new(0,0), new(1,0), new(0,1) },
            new Vector2Int[] { new(0,0), new(1,0), new(1,1) },
            new Vector2Int[] { new(0,0), new(0,1), new(1,1) },
            new Vector2Int[] { new(1,0), new(0,1), new(1,1) },
            new Vector2Int[] { new(0,0), new(1,1) },
            new Vector2Int[] { new(1,0), new(0,1) },
            
            // 2x3
            new Vector2Int[] { new(0,0), new(0,1), new(1,0), new(1,1), new(2,0), new(2,1) },
            new Vector2Int[] { new(0,1), new(1,0), new(1,1), new(2,0) },
            new Vector2Int[] { new(0,0), new(1,0), new(1,1), new(2,1) },
            new Vector2Int[] { new(0,0), new(1,0), new(1,1), new(2,0) },
            new Vector2Int[] { new(0,1), new(1,0), new(1,1), new(2,1) },
            new Vector2Int[] { new(0,0), new(1,0), new(2,0), new(2,1) },
            new Vector2Int[] { new(0,1), new(1,1), new(2,0), new(2,1) },
            new Vector2Int[] { new(0,0), new(0,1), new(1,0), new(2,0) },
            new Vector2Int[] { new(0,0), new(0,1), new(1,1), new(2,1) },
            
            // 3x2
            new Vector2Int[] { new(0,0), new(1,0), new(0,1), new(1,1), new(0,2), new(1,2) },
            new Vector2Int[] { new(1,0), new(0,1), new(1,1), new(0,2) },
            new Vector2Int[] { new(0,0), new(0,1), new(1,1), new(1,2) },
            new Vector2Int[] { new(0,0), new(0,1), new(1,1), new(0,2) },
            new Vector2Int[] { new(1,0), new(0,1), new(1,1), new(1,2) },
            new Vector2Int[] { new(0,0), new(0,1), new(0,2), new(1,2) },
            new Vector2Int[] { new(1,0), new(1,1), new(0,2), new(1,2) },
            new Vector2Int[] { new(0,0), new(1,0), new(0,1), new(0,2) },
            new Vector2Int[] { new(0,0), new(1,0), new(1,1), new(1,2) },

            // 3x3
            new Vector2Int[] { new(0,0), new(1,0), new(2,0), new(0,1), new(1,1), new(2,1), new(0,2), new(1,2), new(2,2) },
            new Vector2Int[] { new(0,0), new(1,0), new(2,0), new(0,1), new(0,2) },
            new Vector2Int[] { new(0,0), new(1,0), new(2,0), new(2,1), new(2,2) },
            new Vector2Int[] { new(0,0), new(0,1), new(0,2), new(1,2), new(2,2) },
            new Vector2Int[] { new(2,0), new(2,1), new(0,2), new(1,2), new(2,2) },
            new Vector2Int[] { new(0,0), new(1,1), new(2,2) },
            new Vector2Int[] { new(2,0), new(1,1), new(0,2) },

            // 일자 블럭
            new Vector2Int[] { new(0,0), new(1,0) },
            new Vector2Int[] { new(0,0), new(1,0), new(2,0) },
            new Vector2Int[] { new(0,0), new(1,0), new(2,0), new(3,0) },
            new Vector2Int[] { new(0,0), new(1,0), new(2,0), new(3,0), new(4,0) },
            new Vector2Int[] { new(0,0), new(0,1) },
            new Vector2Int[] { new(0,0), new(0,1), new(0,2) },
            new Vector2Int[] { new(0,0), new(0,1), new(0,2), new(0,3) },
            new Vector2Int[] { new(0,0), new(0,1), new(0,2), new(0,3), new(0,4) },
        };

        private BlockBoard _blockBoard;

        public BlockGenerator(BlockBoard blockBoard)
        {
            _blockBoard = blockBoard;
        }

        public List<BlockModel> GenerateNextBlocks()
        {
            // _blockBoard 에 배치할 수 있는 블럭 최소 3,2,1개를 보장해서 랜덤으로 생성
            int[,] grid = _blockBoard.GetDeepCopyGrid();

            var result = new List<BlockModel>();

            for (int i = 0; i < 3; i++)
            {
                BlockModel newBlock = GetPlaceableBlock(grid);
                result.Add(newBlock);
            }

            return result;
        }

        private BlockModel GetPlaceableBlock(int[,] grid)
        {
            List<Vector2Int[]> remainPatterns = Patterns.ToList();

            while (0 < remainPatterns.Count)
            {
                int randomIndex = Random.Range(0, remainPatterns.Count);
                BlockModel blockModel = new BlockModel(remainPatterns[randomIndex], Random.Range(0, BoardConfig.ColorCount));     // 추후 블럭별 가중치를 넣어서 가중치가 높은 블럭이 뽑히도록
                remainPatterns.RemoveAt(randomIndex);
                bool canBatch = BlockBoard.CanPlaceBlockAnyWhere(blockModel, ref grid);
                //Todo: BlockBoard.PlaceBlockAnyWhere(shape, ref grid, x, y); 블럭을 임시로 배치해서 배치된 상태로 비교 가능하게
                if (canBatch)
                {
                    return blockModel;
                }
            }

            // 놓을 수 있는 블럭이 없으면 0번(1x1) 블럭 생성
            return new BlockModel(Patterns[0], Random.Range(0, BoardConfig.ColorCount));
        }
    }
}