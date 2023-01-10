using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class FT_Steamworks_Integration : MonoBehaviour
{
    // Start is called before the first frame update
    public void RecordStylePoints(int stylePoints)
    {
        if (SteamManager.Initialized)
        {

        }
    }

    public static void LongDistanceThrow100Points()
    {
        if (SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetAchievement("LONG_DISTANCE_THROW_100", out bool achievementCompleted);

            if (!achievementCompleted)
            {
                Debug.Log("Steamworks Info ADDED THE ACHIEVENT LONG_DISTANCE_THROW_100");
                SteamUserStats.SetAchievement("LONG_DISTANCE_THROW_100");
                SteamUserStats.StoreStats();
                SteamAPI.RunCallbacks();

            } else {
                Debug.Log("Steamworks ACHIEVENT LONG_DISTANCE_THROW_100 was ALREADY ACHIEVED");
            }
        }

    }
 



}
