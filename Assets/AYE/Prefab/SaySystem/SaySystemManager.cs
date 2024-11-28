using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaySystemManager : SampleSingleton<SaySystemManager>
{
    public TextAsset mainData = null;
    public List<LanguageData> languageDataList = new List<LanguageData>();
    public LogType logType = LogType.disabled;
    public enum LogType
    {
        disabled = 0,
        enabled = 1,
    }

    public override void OnLoad()
    {
        base.OnLoad();

        mainData = Resources.Load<TextAsset>("SaySystemData/MainData");
        if (logType == LogType.enabled)
            Debug.Log("解析對話資料");
        // 讀取譯本成陣列
        languageDataList = new List<LanguageData>();
        LanguageData temp = new LanguageData();
        temp.data = new List<string>();
        bool haveData = false;
        string[] ogdata = mainData.text.Split('\n');
        if (logType == LogType.enabled)
            Debug.Log("資料長度 : " + ogdata.Length);
        for (int i = 0; i < ogdata.Length; i++)
        {
            // 如果發現這是一個標籤就檢查是否要新增語言
            string ogdataTrim = ogdata[i].Trim();
            if (ogdataTrim[0] == '<' && ogdataTrim[ogdataTrim.Length - 1] == '>')
            {
                for (int j = 0; j < 28; j++)
                {
                    string languageName = ((LanguageISO639_1)j).ToString();
                    // 這邊要去掉前後的< >
                    string ogdataTrimInfo = ogdataTrim.Substring(1, ogdataTrim.Length - 2);
                    // 發現語言
                    if (languageName.Trim() == ogdataTrimInfo.Trim())
                    {
                        // 有資料的話先記錄
                        if (haveData)
                        {
                            // 加入到列表
                            if (logType == LogType.enabled)
                                Debug.Log("整合出 : " + temp.language.ToString());
                            languageDataList.Add(temp);
                            temp = new LanguageData();
                            temp.data = new List<string>();
                        }
                        temp.language = (LanguageISO639_1)j;
                        temp.data.Add("---------對齊用---------");
                        haveData = true;
                    }
                }
            }
            if (haveData)
            {
                if (logType == LogType.enabled)
                    Debug.Log(ogdata[i]);
                temp.data.Add(ogdata[i]);
            }
        }
        if (haveData)
        {
            if (logType == LogType.enabled)
                Debug.Log("整合出 : " + temp.language.ToString());
            languageDataList.Add(temp);
        }
        if (logType == LogType.enabled)
            Debug.Log("解析完畢");

        // 檢查每個languageDataList中資料數量是否一致
        int count = languageDataList[0].data.Count;
        for (int i = 0; i < languageDataList.Count; i++)
        {
            if (count != languageDataList[i].data.Count)
            {
                Debug.LogError("語言 : " + languageDataList[i].language.ToString() + " 資料數量 : " + languageDataList[i].data.Count + " 與其他語言不一致，請確認是否有缺漏翻譯。");
            }
        }
    }

    /// <summary>取得資料</summary>
    public string GetText(LanguageISO639_1 language, string indexName)
    {
        // 將indexName轉換為數字
        int index = 0;
        if (int.TryParse(indexName.Trim(), out index))
        {
            return GetText(language, index);
        }
        else
        {
            return "Error 請給整數，參考MainData文件。";
        }
    }
    /// <summary>取得資料</summary>
    public string GetText(LanguageISO639_1 language, int index)
    {
        if (languageDataList.Count == 0)
            return "";
        for(int i = 0; i < languageDataList.Count; i++)
        {
            if (languageDataList[i].language == language)
            {
                if (languageDataList[i].data.Count <= index)
                    return "";
                return languageDataList[i].data[index].Trim();
            }
        }
        return "";
    }
}
[System.Serializable]
public struct LanguageData
{
    public LanguageISO639_1 language;
    public List<string> data;
}
public enum LanguageCN : int
{
    繁體中文 = 0,
    簡體中文 = 1,
    捷克文 = 2,
    丹麥文 = 3,
    荷蘭文 = 4,
    英文 = 5,
    芬蘭文 = 6,
    法文 = 7,
    德文 = 8,
    匈牙利文 = 9,
    義大利文 = 10,
    日文 = 11,
    韓文 = 12,
    挪威文 = 13,
    波蘭文 = 14,
    葡萄牙文_葡萄牙 = 15,
    葡萄牙文_巴西 = 16,
    羅馬尼亞文 = 17,
    俄文 = 18,
    西班牙文_西班牙 = 19,
    瑞典文 = 20,
    泰文 = 21,
    土耳其文 = 22,
    保加利亞文 = 23,
    烏克蘭文 = 24,
    希臘文 = 25,
    西班牙文_拉丁美洲 = 26,
    越南文 = 27,
}
public enum LanguageISO639_1 : int
{
    zh_TW = 0,
    zh_CN = 1,
    cs = 2,
    da = 3,
    nl = 4,
    en = 5,
    fi = 6,
    fr = 7,
    de = 8,
    hu = 9,
    it = 10,
    ja = 11,
    ko = 12,
    nb = 13,
    pl = 14,
    pt_PT = 15,
    pt_BR = 16,
    ro = 17,
    ru = 18,
    es_ES = 19,
    sv = 20,
    th = 21,
    tr = 22,
    bg = 23,
    uk = 24,
    el = 25,
    es_LA = 26,
    vi = 27,
}