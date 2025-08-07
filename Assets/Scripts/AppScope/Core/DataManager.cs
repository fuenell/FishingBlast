using AppScope.Data;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AppScope.Core
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

        public void SaveData()
        {
            string json = JsonUtility.ToJson(_playerData, true);
            // JSON 문자열을 암호화
            string encryptedJson = Encrypt(json);
            File.WriteAllTextAsync(_savePath, encryptedJson);
        }

        public void LoadData()
        {
            if (File.Exists(_savePath))
            {
                string encryptedJson = File.ReadAllText(_savePath);
                // 암호화된 문자열을 복호화
                string json = Decrypt(encryptedJson);
                _playerData = JsonUtility.FromJson<PlayerData>(json);
            }
            else
            {
                _playerData = new PlayerData();
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