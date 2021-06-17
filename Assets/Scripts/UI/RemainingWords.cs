using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;

public class RemainingWords : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int teamNumber;

    [SerializeField]
    private TextMeshProUGUI textCounter;

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_FIRST_TEAM_WORDS) &&
             teamNumber == 1)
        {
            textCounter.text = ((int)propertiesThatChanged[Constants.HASH_ROOM_FIRST_TEAM_WORDS]).ToString();
        }
        else if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_SECOND_TEAM_WORDS) &&
             teamNumber == 2)
        {
            textCounter.text = ((int)propertiesThatChanged[Constants.HASH_ROOM_SECOND_TEAM_WORDS]).ToString();
        }
    }

    //private void Update()
    //{
    //    teamNumber
    //}

}
