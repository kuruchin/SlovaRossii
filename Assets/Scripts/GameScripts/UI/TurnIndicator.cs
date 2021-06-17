using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnIndicator : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private MainManager.GamePhase neededGamePhase;

    [SerializeField]
    private int neededTeamNumber;

    [SerializeField]
    private GameObject indicator;

    private MainManager.GamePhase currentGamePhase;
    private int currentTeamNumber;

    public void Start()
    {
        indicator.SetActive(false);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_GAME_PHASE))
        {
            currentGamePhase = (MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE];
        }

        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_CURRENT_TEAM))
        {
            currentTeamNumber = (int)propertiesThatChanged[Constants.HASH_ROOM_CURRENT_TEAM];
        }
    }

    public void Update()
    {
        if(currentGamePhase == MainManager.GamePhase.Writing ||
           currentGamePhase == MainManager.GamePhase.Opening)
        {
            if(neededGamePhase == currentGamePhase &&
               neededTeamNumber == currentTeamNumber)
            {
                indicator.SetActive(true);
            }
            else
            {
                indicator.SetActive(false);
            }
        }
        else
        {
            indicator.SetActive(false);
        }
    }


}
