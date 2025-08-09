using FishingBlast.Data;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FishingBlast.AppScope
{
    public class DataManager
    {
        private PlayerData _playerData;
        private string _savePath;

        public PlayerData GetPlayerData()
        {
            return _playerData;
        }

        public bool ShouldLoadPlayScene()
        {
            // 실제 판단 로직은 PlayerData에 위임
            return _playerData.IsPlaying || _playerData.NeedTutorial;
        }

        public DataManager()
        {
            // 데이터 로드
            _savePath = Path.Combine(Application.persistentDataPath, "playerdata.json");
            LoadData();
        }

        public void ResetGame()
        {
            _playerData.ResetGame();
        }

        public void UpdateGame(int[,] grid, List<BlockModel> remainBlockList, int totalScore)
        {
            // 실제 데이터 처리 로직을 PlayerData에 위임
            _playerData.UpdateGameBoard(grid, remainBlockList);
            _playerData.UpdateScore(totalScore);
        }

        // 물고기를 잡았을 때 호출할 함수
        public void AddNewFish(int fishId, float size)
        {
            _playerData.AddOrUpdateFish(fishId, size);
        }

        public void LoadData()
        {
            if (File.Exists(_savePath))
            {
                try
                {
                    string encryptedJson = File.ReadAllText(_savePath);
                    // 암호화된 문자열을 복호화
                    string json = Decrypt(encryptedJson);
                    _playerData = JsonUtility.FromJson<PlayerData>(json);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to load player data: {e.Message}");
                    _playerData = new PlayerData();
                }
            }
            else
            {
                _playerData = new PlayerData();
            }
        }

        private bool _isSaving = false;
        private bool _saveRequested = false;

        public async UniTask SaveData()
        {
            // 이미 저장 작업이 진행 중이라면,
            if (_isSaving)
            {
                // "나중에 한 번 더 저장해달라"고 요청만 표시하고 즉시 종료합니다.
                _saveRequested = true;
                return;
            }

            _isSaving = true;
            _saveRequested = false;

            // while 루프를 사용하여 요청된 저장이 없을 때까지 반복합니다.
            try
            {
                string json = JsonUtility.ToJson(_playerData, true);
                string encryptedJson = Encrypt(json);

                // 실제 비동기 파일 쓰기 작업
                await File.WriteAllTextAsync(_savePath, encryptedJson);
            }
            finally
            {
                _isSaving = false; // 마스터 플래그를 내려 저장 루프를 종료합니다.
            }

            if (_saveRequested)
            {
                SaveData().Forget(); // 저장 요청이 있으면 재귀적으로 호출하여 저장을 시도합니다.
            }
        }

        // 간단한 암호화/복호화 예시 (실제로는 더 강력한 AES 등을 사용 권장)
        private string Encrypt(string data)
        {
            return data;
            //byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
            //return System.Convert.ToBase64String(dataBytes); // 간단한 인코딩으로도 내용을 숨길 수 있음
        }

        private string Decrypt(string data)
        {
            return data;
            //byte[] dataBytes = System.Convert.FromBase64String(data);
            //return System.Text.Encoding.UTF8.GetString(dataBytes);
        }
    }
}