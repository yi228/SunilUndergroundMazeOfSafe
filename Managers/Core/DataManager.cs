using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager
{
    // 1. edit 이전까지 주소 복사 후 {}/export?format=tsv -> {}안에 넣기
    // 2. 공유 대상을 링크 소유자로 바꿔주기
    const string URL = "https://docs.google.com/spreadsheets/d/1iyPnyAdIQW70icojP7_nSVHgqu7_PypMWDvwaCkSwzk/export?format=tsv";
    public List<int> PlayerStat = new List<int>();
    public IEnumerator CoDownloadDataSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(Managers.URL.PlayerStat);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        Debug.Log(data);
        Deserialization(data);
    }
    void Deserialization(string data)
    {
        string[] row = data.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;
        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split("\t");
            for (int j = 0; j < columnSize; j++)
            {
                Debug.Log(column[j]);
                int value;
                bool isInt = int.TryParse(column[j], out value);
                if (isInt)
                    PlayerStat.Add(value);
            }
        }
        Managers.Game.GetPlayerStat();
    }
}