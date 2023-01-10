using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FT_LeaderboardUI : MonoBehaviour
{
    [SerializeField] Transform holder;
    [SerializeField] GameObject highscorePrefab;
    List<GameObject> highscorePrefabs = new List<GameObject>();

    public void BtnBeginFillLeaderboardLocal()
    {
        FindObjectOfType<FT_LeadManager>().GetLeaderBoardData(Steamworks.ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, 14);
    }

    public void BtnBeginFillLeaderboardGlobal()
    {
        FindObjectOfType<FT_LeadManager>().GetLeaderBoardData(Steamworks.ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 14);
    }

    public void BtnBeginFillLeaderboardFriends()
    {
        FindObjectOfType<FT_LeadManager>().GetLeaderBoardData(Steamworks.ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends, 14);
    }

    public void FillLeaderboard(List<FT_LeadManager.LeaderboardData> lDataset)
    {
        Debug.Log("filling leaderboard");
        foreach(GameObject g in highscorePrefabs)
        {
            Destroy(g);
        }

        foreach (FT_LeadManager.LeaderboardData lD in lDataset)
        {
            GameObject g = Instantiate(highscorePrefab, holder);
            highscorePrefabs.Add(g);
            FillHighscorePrefab(g, lD);
        }
    }

    void FillHighscorePrefab(GameObject _prefab, FT_LeadManager.LeaderboardData _lData)
    {
        _prefab.transform.Find("username").GetComponent<Text>().text = _lData.username;
        _prefab.transform.Find("score").GetComponent<Text>().text = _lData.score.ToString();
        _prefab.transform.Find("rank").GetComponent<Text>().text = _lData.rank.ToString();
    }
}