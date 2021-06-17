using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButton : MonoBehaviourPunCallbacks
{
    //PhotonNetwork.CurrentRoom.CustomProperties["GamePhase"]
    //PhotonNetwork.CurrentRoom.CustomProperties["CurrentTeam"]
    //PhotonNetwork.CurrentRoom.CustomProperties["FirstTeamWords"]
    //PhotonNetwork.CurrentRoom.CustomProperties["SecondTeamWords"]

    //PhotonNetwork.LocalPlayer.CustomProperties["TeamNumber"]
    //PhotonNetwork.LocalPlayer.CustomProperties["Captain"]
    //PhotonNetwork.LocalPlayer.CustomProperties["WordPressed"]

    public void CheckHashtables()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log(player.NickName + " Teamnumber: " + player.CustomProperties["TeamNumber"].ToString());
            Debug.Log(player.NickName + " Captain: " + player.CustomProperties["Captain"].ToString());
            if(player.CustomProperties["WordPressed"] == null)
            {
                Debug.Log(player.NickName + " WordPressed: NULL");
            }
            else
            {
                Debug.Log(player.NickName + " WordPressed: " + player.CustomProperties["WordPressed"].ToString());
            }
            
        }

        Debug.Log("FirstTeamWords: " + PhotonNetwork.CurrentRoom.CustomProperties["FirstTeamWords"]);
        Debug.Log("SecondTeamWords: " + PhotonNetwork.CurrentRoom.CustomProperties["SecondTeamWords"]);
        Debug.Log("FirstTeam: " + PhotonNetwork.CurrentRoom.CustomProperties["FirstTeam"]);
        if (PhotonNetwork.CurrentRoom.CustomProperties["SecondTeam"] != null)
        {
            Debug.Log("SecondTeam: " + PhotonNetwork.CurrentRoom.CustomProperties["SecondTeam"]);
        }

        
        if (PhotonNetwork.CurrentRoom.CustomProperties["GamePhase"] != null)
        {
            Debug.Log("Current GamePhase: " + (MainManager.GamePhase)PhotonNetwork.CurrentRoom.CustomProperties["GamePhase"]);
        }
        else
        {
            Debug.Log("Current GamePhase: " + "NULL");
        }
        
        if (PhotonNetwork.CurrentRoom.CustomProperties["CurrentTeam"] != null)
        {
            Debug.Log("CurrentTeam: " + PhotonNetwork.CurrentRoom.CustomProperties["CurrentTeam"]);
        }
        else
        {
            Debug.Log("CurrentTeam: " + "NULL");
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("Local player: " + player.NickName);
            }
            else
            {
                Debug.Log("Another player: " + player.NickName);
            }
        }

    }

}
