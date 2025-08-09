using FishingBlast.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using UnityEditor;
using UnityEngine;

namespace FishingBlast.Editor
{
    public class SheetLoader
    {
        private const string SPREADSHEET_ID = "1egUViu_lZwFuVJor2c80BGe7T65FliR3v7PrSs22Hh0";
        private const string SHEET_ID = "1070580520";
        private const string URL_FORMAT = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";

        private const string DATABASE_SAVE_PATH = "Assets/Resources/";
        private const string DATABASE_ASSET_NAME = "FishDatabase.asset";

        [MenuItem("Data/Update Fish Database From Google Sheet")]
        public static async void UpdateFishDatabase()
        {
            FishDatabase database = GetOrCreateDatabase();
            string csvData;
            try
            {
                string url = string.Format(URL_FORMAT, SPREADSHEET_ID, SHEET_ID);
                using (var client = new HttpClient())
                {
                    csvData = await client.GetStringAsync(url);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"데이터 다운로드 실패: {e.Message}");
                return;
            }

            ClearDatabase(database);

            string[] allLines = csvData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (allLines.Length <= 1) return;

            string[] header = allLines[0].Split(',');

            for (int i = 1; i < allLines.Length; i++)
            {
                string[] values = allLines[i].Split(',');
                if (values.Length < header.Length) continue;

                var data = new Dictionary<string, string>();
                for (int j = 0; j < header.Length; j++)
                {
                    data[header[j].Trim()] = values[j].Trim();
                }

                FishMasterData fishInstance = ScriptableObject.CreateInstance<FishMasterData>();
                fishInstance.name = $"Fish_{data["id"]}";

                // BindDataToInstance 로직을 SetData 메소드 호출로 변경
                BindData(fishInstance, data);

                AssetDatabase.AddObjectToAsset(fishInstance, database);
                database.AddFish(fishInstance);
            }

            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("FishDatabase 업데이트가 완료되었습니다.");
        }

        private static FishDatabase GetOrCreateDatabase()
        {
            string fullPath = Path.Combine(DATABASE_SAVE_PATH, DATABASE_ASSET_NAME);
            if (!Directory.Exists(DATABASE_SAVE_PATH))
            {
                Directory.CreateDirectory(DATABASE_SAVE_PATH);
            }
            FishDatabase database = AssetDatabase.LoadAssetAtPath<FishDatabase>(fullPath);
            if (database == null)
            {
                database = ScriptableObject.CreateInstance<FishDatabase>();
                AssetDatabase.CreateAsset(database, fullPath);
            }
            return database;
        }

        private static void ClearDatabase(FishDatabase database)
        {
            var subAssets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(database));
            foreach (var subAsset in subAssets)
            {
                if (!AssetDatabase.IsMainAsset(subAsset))
                {
                    AssetDatabase.RemoveObjectFromAsset(subAsset);
                }
            }
            database.ClearAll(); // 이제 데이터베이스의 Clear 메소드를 호출
        }

        private static void BindData(FishMasterData fishInstance, Dictionary<string, string> data)
        {
            // FishMasterData의 SetData 메소드를 통해 안전하게 데이터 주입
            fishInstance.SetData(
                int.Parse(data["id"]),
                data["fishName"],
                (FishGrade)Enum.Parse(typeof(FishGrade), data["grade"]),
                float.Parse(data["minSize"]),
                float.Parse(data["maxSize"]),
                ParseSpawnRegions(data["spawnRegions"])
            );
        }

        private static FishSpawnRegion[] ParseSpawnRegions(string regionData)
        {
            if (string.IsNullOrEmpty(regionData)) return new FishSpawnRegion[0];

            string[] regionStrings = regionData.Split(';');
            var regions = new List<FishSpawnRegion>();
            foreach (var str in regionStrings)
            {
                if (Enum.TryParse<FishSpawnRegion>(str, out var region))
                {
                    regions.Add(region);
                }
            }
            return regions.ToArray();
        }
    }
}