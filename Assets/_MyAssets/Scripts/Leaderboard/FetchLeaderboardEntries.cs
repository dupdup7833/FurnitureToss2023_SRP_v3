using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySteamLeaderboard;

public class FetchLeaderboardEntries : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EasySteamLeaderboards.Instance.GetLeaderboard("Public Style Points Leaderboard", (result) =>
        {
            if (result.resultCode == ESL_ResultCode.Success){
                for (int i=0; i<result.GlobalEntries.Count;i++){
                    Debug.Log("Entry "+(i+1)+":"+result.GlobalEntries[i].PlayerName+" - "+result.GlobalEntries[i].Score+"\n" );
                }
            } else {
                Debug.Log("failed because of "+result.resultCode);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
