using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System;

public class Leaderboard : MonoBehaviour
{
    public LeaderboardItem leaderboardItemPrefab;
    List<LeaderboardItem> leaderboardItemList = new List<LeaderboardItem>();
    public Transform contentObject;

    public TextMeshProUGUI UserNameTXT;
    public TextMeshProUGUI ScoreTXT;

    public static string playerName;

    private string leaderboardUrl = "http://localhost:8081/api/leaderboard/";
    private string playerScoreUrl = "http://localhost:8081/api/users/{0}/score/";

    public void UpdadeLeaderboard()
    {

        foreach (LeaderboardItem item in leaderboardItemList)
        {
            Destroy(item.gameObject);
        }
        leaderboardItemList.Clear();
        StartCoroutine(GetLeaderboard());
        StartCoroutine(OnePlayerLeaderboard());

    }

    private IEnumerator GetLeaderboard()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(leaderboardUrl))
        {
            www.url = leaderboardUrl;

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var text = www.downloadHandler.text;
                LeaderboardResultArray result = JsonUtility.FromJson<LeaderboardResultArray>("{\"result\":" + text + "}");
                for (int i = 0; i < result.result.Length; i++)
                {
                    LeaderboardResult info = result.result[i];
                    
                    LeaderboardItem newItem = Instantiate(leaderboardItemPrefab, contentObject);
                    newItem.SetPlayerName(info.pr_name);
                    newItem.SetScore(info.mh_points.ToString());
                    newItem.SetPosition((i+1).ToString());
                    leaderboardItemList.Add(newItem);
                    
                }
            }
        }
    }

    [System.Serializable]
    class LeaderboardResultArray
    {
        public LeaderboardResult[] result;
    }

    [System.Serializable]
    class LeaderboardResult
    {
        public int mh_points;
        public string pr_name;
    }

    [System.Serializable]
    class OnePlayerResult
    {
        public int id;
        public int points;
        public int playerId;
        public string playerName;
    }
    public static void SetPlayerName(string name)
    {
        playerName = name;
    }
    private IEnumerator OnePlayerLeaderboard()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(String.Format(playerScoreUrl, GameManager.playerId)))
        {
            www.url = String.Format(playerScoreUrl, GameManager.playerId);

            yield return www.SendWebRequest();

            UserNameTXT.text = "Username: " + playerName;
            if (www.result == UnityWebRequest.Result.Success)
            {
                var text = www.downloadHandler.text;
                OnePlayerResult result = JsonUtility.FromJson<OnePlayerResult>(text);
                if (result != null) { }
                ScoreTXT.text = "Score: " + result.points;

            }
            else
            {
                ScoreTXT.text = "Score: 0";
            }
        }
    }
}
