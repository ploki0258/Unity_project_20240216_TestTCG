using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AYE
{
    public class RankManager
    {
        //單例模式Singleton
        public static RankManager instance
        {
            // 當有要取用這個實體
            get
            {
                // 當我不存在的時候
                if (_instance == null)
                {
                    // 分配一個新的記憶體給自己的真實位置
                    _instance = new RankManager();
                }
                return _instance;
            }
        }
        // 他的真實位置
        static RankManager _instance = null;

        public void Creat()
        {

        }

        /// <summary>
        /// 添加紀錄
        /// </summary>
        /// <param name="name">名稱</param>
        /// <param name="score">排行值(大到小)</param>
        /// <param name="data">夾帶內容</param>
        /// <returns>true : 建立新紀錄 false : 更新舊紀錄</returns>
        public bool Update(string rankName, string name, float score, string data = "")
        {
            RankDataSave saveData = Load(rankName);

            for (int i = 0; i < saveData.rankData.Count; i++)
            {
                if (saveData.rankData[i].name == name)
                {
                    saveData.rankData[i] = new RankData(name, score, data);
                    saveData.rankData.Sort((x, y) => { return -x.score.CompareTo(y.score); });
                    Save(saveData);
                    return false;
                }
            }

            saveData.rankData.Add(new RankData(name, score, data));
            saveData.rankData.Sort((x, y) => { return -x.score.CompareTo(y.score); });
            Save(saveData);
            return true;
        }

        /// <summary>總共有幾筆資料</summary>
        public int GetCount(string rankName)
        {
            RankDataSave saveData = Load(rankName);
            return saveData.rankData.Count;
        }

        /// <summary>用名次取得名稱(1開始)</summary>
        public string GetNameByNumber(string rankName, int number)
        {
            RankDataSave saveData = Load(rankName);
            if (number == 0)
                number = 1;
            if (saveData.rankData.Count >= number)
            {
                return saveData.rankData[number - 1].name;
            }
            return "";
        }

        /// <summary>用名次取得資料(1開始)</summary>
        public string GetDataByNumber(string rankName, int number)
        {
            RankDataSave saveData = Load(rankName);
            if (number == 0)
                number = 1;
            if (saveData.rankData.Count >= number)
            {
                return saveData.rankData[number - 1].data;
            }
            return "";
        }

        /// <summary>用名次取得數值(1開始)</summary>
        public float GetValueByNumber(string rankName, int number)
        {
            RankDataSave saveData = Load(rankName);
            if (number == 0)
                number = 1;
            if (saveData.rankData.Count >= number)
            {
                return saveData.rankData[number - 1].score;
            }
            return 0f;
        }

        /// <summary>用名稱取得名次</summary>
        public int GetNumberByName(string rankName, string name)
        {
            RankDataSave saveData = Load(rankName);
            for (int i = 0; i < saveData.rankData.Count; i++)
            {
                if (saveData.rankData[i].name == name)
                    return i + 1;
            }
            return 0;
        }

        /// <summary>用名稱取得資料</summary>
        public string GetDataByName(string rankName, string name)
        {
            RankDataSave saveData = Load(rankName);
            for (int i = 0; i < saveData.rankData.Count; i++)
            {
                if (saveData.rankData[i].name == name)
                    return saveData.rankData[i].data;
            }
            return "";
        }

        /// <summary>用名稱取得數值</summary>
        public float GetValueByName(string rankName, string name)
        {
            RankDataSave saveData = Load(rankName);
            for (int i = 0; i < saveData.rankData.Count; i++)
            {
                if (saveData.rankData[i].name == name)
                    return saveData.rankData[i].score;
            }
            return 0f;
        }

        /// <summary>移除排行榜中的所有內容</summary>
        public void DeleteAll(string rankName)
        {
            RankDataSave saveData = Load(rankName);
            saveData.rankData.Clear();
            Save(saveData);
        }

        /// <summary>依照名次移除資料</summary>
        public void DeleteByNumber(string rankName, int number)
        {
            RankDataSave saveData = Load(rankName);

            if (number == 0)
                number = 1;
            for (int i = 0; i < saveData.rankData.Count; i++)
            {
                if (i == (number - 1))
                {
                    saveData.rankData.RemoveAt(i);
                }
            }

            Save(saveData);
        }

        /// <summary>依照名稱移除資料</summary>
        public void DeleteByName(string rankName, string name)
        {
            RankDataSave saveData = Load(rankName);

            for (int i = 0; i < saveData.rankData.Count; i++)
            {
                if (saveData.rankData[i].name == name)
                {
                    saveData.rankData.RemoveAt(i);
                }
            }

            Save(saveData);
        }

        /// <summary>測試內容</summary>
        public void Test(string name, bool showData)
        {
            RankDataSave saveData = Load(name);

            string t = "排行榜 " + name + " 總共 " + saveData.rankData.Count + " 筆資料\n";
            for (int o = 0; o < saveData.rankData.Count; o++)
            {
                if (saveData.rankData[o].data != "" && showData)
                    t += "" + (o + 1).ToString() + "   " + saveData.rankData[o].name + " : " + saveData.rankData[o].score + " : " + saveData.rankData[o].data + "\n";
                else
                    t += "" + (o + 1).ToString() + "   " + saveData.rankData[o].name + " : " + saveData.rankData[o].score + "\n";
            }
            t += "---------END---------";
            Debug.Log(t);

            Save(saveData);
        }

        // -------------------------------------------------------------

        List<RankDataSave> tempRankDataSaveList = new List<RankDataSave>();

        RankDataSave Load(string rankName)
        {
            // 試圖使用目前有的排行榜
            for (int i = 0; i < tempRankDataSaveList.Count; i++)
            {
                if (tempRankDataSaveList[i].name == rankName)
                {
                    return new RankDataSave(tempRankDataSaveList[i]);
                }
            }

            string loadData = PlayerPrefs.GetString("RankManagerSave_" + rankName, "");
            if (loadData == "")
            {
                // 找不到這個排行榜，創造新的排行榜並重找
                tempRankDataSaveList.Add(new RankDataSave(rankName, new List<RankData>()));
                return Load(rankName);
            }
            else
            {
                // 找到這個排行榜，將這筆資料放進暫存並重找
                tempRankDataSaveList.Add(JsonUtility.FromJson<RankDataSave>(loadData));
                return Load(rankName);
            }
        }

        void Save(RankDataSave data)
        {
            for (int i = 0; i < tempRankDataSaveList.Count; i++)
            {
                if (tempRankDataSaveList[i].name == data.name)
                {
                    tempRankDataSaveList[i] = new RankDataSave(data);
                    PlayerPrefs.SetString("RankManagerSave_" + tempRankDataSaveList[i].name, JsonUtility.ToJson(tempRankDataSaveList[i]));
                }
            }
        }

        [System.Serializable]
        struct RankDataSave
        {
            public string name;
            [SerializeField]
            public List<RankData> rankData;
            public RankDataSave(string name, List<RankData> rankData)
            {
                this.name = name;
                this.rankData = new List<RankData>(rankData);
            }
            public RankDataSave(RankDataSave data)
            {
                this.name = data.name;
                this.rankData = new List<RankData>(data.rankData);
            }
        }

        [System.Serializable]
        struct RankData
        {
            [SerializeField]
            public string name;
            [SerializeField]
            public float score;
            [SerializeField]
            public string data;
            public RankData(string name, float score, string data)
            {
                this.name = name;
                this.score = score;
                this.data = data;
            }
        }
    }
}

// 2020 by 阿葉