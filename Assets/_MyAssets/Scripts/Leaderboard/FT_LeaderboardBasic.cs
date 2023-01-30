using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySteamLeaderboard;

public class FT_LeaderboardBasic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HideAfterSeconds(10));
    }

    IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        EasySteamLeaderboards.Instance.GetLeaderboard("ArchVizStylePointsLeaderboard_1", (result) =>
        {
            if (result.resultCode == ESL_ResultCode.Success)
            {
                Debug.Log("global" + result.GlobalEntries.Count);
            }
        }, 1, 20);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
