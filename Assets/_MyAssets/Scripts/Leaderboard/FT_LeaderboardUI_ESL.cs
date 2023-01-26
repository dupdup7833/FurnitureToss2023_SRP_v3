using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using EasySteamLeaderboard;

public class FT_LeaderboardUI_ESL : MonoBehaviour
{

    //ase
    public GameObject EntriesContainer;
    public GameObject LBEntryPrefab;
    //public InputField Fetch_IDField;
    //public InputField Upload_IDField;
    //public InputField Upload_ScoreField;
    public ESL_LeaderboardEntryUI yourEntryUI;

    public Text LeaderboardTitleTextComponent;

    public string LeaderboardName;
    public string LeaderboardTitle;


    //enum
    public enum LeaderboardFilter
    {
        Global,
        Friends
    }

    //vars
    LeaderboardFilter currentFilter;
    List<GameObject> entriesObjs = new List<GameObject>();
    ESL_Leaderboard lbCache;
    private void Start()
    {
		LeaderboardTitleTextComponent.text = LeaderboardTitle;
        
    }

    // void OnEnable()
    // {
    // 	ESL_LeaderboardFilterSelector.onFilterSelected += ESL_LeaderboardFilterSelector_onFilterSelected;
    // }

    // void OnDisable()
    // {
    // 	ESL_LeaderboardFilterSelector.onFilterSelected -= ESL_LeaderboardFilterSelector_onFilterSelected;
    // }

    // void ESL_LeaderboardFilterSelector_onFilterSelected(LeaderboardFilter filter)
    // {
    // 	currentFilter = filter;

    // 	//if cached then repopulate
    // 	if (lbCache != null)
    // 		PopulateEntriedBasedOnFilter();

    // }

    void PopulateEntriedBasedOnFilter()
    {
        StopAllCoroutines();
        //if (currentFilter == LeaderboardFilter.Global)
        StartCoroutine(PopulateEntries(lbCache.GlobalEntries));
        //else if (currentFilter == LeaderboardFilter.Friends)
        //	StartCoroutine(PopulateEntries(lbCache.FriendsEntries));
    }

    IEnumerator PopulateEntries(List<ESL_LeaderboardEntry> entries)
    {
        //reset current ui
        ResetUI();

        //populate your entry if it exists
        yourEntryUI.Initialize(lbCache.SteamUserEntry);
        Debug.Log("entries count:" + entries.Count);
        for (int i = 0; i < entries.Count; i++)
        {
            //instantiate prefab
            GameObject entry = Instantiate(LBEntryPrefab) as GameObject;

            //if gloabl entries show global rank
            if (currentFilter == LeaderboardFilter.Global)
                entry.GetComponent<ESL_LeaderboardEntryUI>().Initialize(entries[i]);
            else if (currentFilter == LeaderboardFilter.Friends) //if friends, then show local rank among friends
                entry.GetComponent<ESL_LeaderboardEntryUI>().Initialize(entries[i].PlayerName, (i + 1), entries[i].Score);

            //set transform to container
            entry.transform.SetParent(EntriesContainer.transform);
            entry.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            entry.transform.localScale = Vector3.one;
            // 

            //local obj cache
            entriesObjs.Add(entry);

            yield return null;
        }
    }

    void ResetUI()
    {
        for (int i = 0; i < entriesObjs.Count; i++)
        {
            Destroy(entriesObjs[i]);
        }

        entriesObjs.Clear();

        //reset your entry ui
        yourEntryUI.Reset();
    }

    void FetchLeaderboardWithID(string lbid, int startRange, int endRange)
    {
        EasySteamLeaderboards.Instance.GetLeaderboard(lbid, (result) =>
            {
                //check if leaderboard successfully fetched
                if (result.resultCode == ESL_ResultCode.Success)
                {
                    lbCache = result;
                    PopulateEntriedBasedOnFilter();
                }
                else
                {
                    Debug.Log("Failed Fetching: " + result.resultCode.ToString());
                    StopAllCoroutines();
                    ResetUI();
                }
            }, startRange, endRange); //fetch top 20 entries
    }

    //ID fetched from input field directly
    public void FetchLeaderboard()
    {
        //string lbid = Fetch_IDField.text; //get id from input field from user

        FetchLeaderboardWithID(LeaderboardName, 1, 20);
    }

     
    public void UploadScoreToLeaderboard(int score)
    {

    	EasySteamLeaderboards.Instance.UploadScoreToLeaderboard(LeaderboardName, score, (result) =>
    		{
    			//check if leaderboard successfully fetched
    			if (result.resultCode == ESL_ResultCode.Success)
    			{
    				Debug.Log("Succesfully Uploaded!");

    				//refresh lbid
    				FetchLeaderboardWithID(LeaderboardName, 1, 20);
    			}
    			else
    			{
    				Debug.Log("Failed Uploading: " + result.resultCode.ToString());
    				StopAllCoroutines();
    				ResetUI();
    			}
    		});
    }
}
