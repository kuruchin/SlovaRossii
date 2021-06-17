using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TeamManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int teamNumber = 0;

    [SerializeField]
    private bool isPaused;

    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    public void OnClick_ChooseSoldier()
    {
        _myCustomProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        _myCustomProperties["Captain"] = false;
        _myCustomProperties["TeamNumber"] = teamNumber;

        PhotonNetwork.LocalPlayer.SetCustomProperties(_myCustomProperties);
    }

    public void OnClick_ChooseCaptain()
    {
        if(teamNumber == 0)
        {
            return;
        }

        bool thereIsCaptain = false;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            _myCustomProperties = player.CustomProperties;

            if((bool)_myCustomProperties["Captain"] == true &&
                (int)_myCustomProperties["TeamNumber"] == teamNumber)
            {
                thereIsCaptain = true;
            }
        }

        if (thereIsCaptain == false)
        {
            _myCustomProperties = PhotonNetwork.LocalPlayer.CustomProperties;

            _myCustomProperties["Captain"] = true;
            _myCustomProperties["TeamNumber"] = teamNumber;

            PhotonNetwork.LocalPlayer.SetCustomProperties(_myCustomProperties);
        }
    }

}
