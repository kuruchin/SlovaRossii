using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviourPunCallbacks
{
    public void Start()
    {
        if (!Constants.IS_TEST_MODE)
        {
            gameObject.SetActive(false);
        }
    }


    public void OnClick_ShowInfo()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log("///////////////////////////////////////////////////////////////////////////");
            Debug.Log(player.NickName + " userID: " + player.UserId);
            Debug.Log(player.NickName + " actorname: " + player.ActorNumber);
            Debug.Log(player.NickName + " Teamnumber: " + player.CustomProperties["TeamNumber"].ToString());
            Debug.Log(player.NickName + " Captain: " + player.CustomProperties["Captain"].ToString());
            if (player.CustomProperties["WordPressed"] == null)
            {
                Debug.Log(player.NickName + " WordPressed: NULL");
            }
            else
            {
                Debug.Log(player.NickName + " WordPressed: " + player.CustomProperties["WordPressed"].ToString());
            }

        }
        Debug.Log("///////////////////////////////////////////////////////////////////////////");
        Debug.Log("FirstTeamWords: " + PhotonNetwork.CurrentRoom.CustomProperties["FirstTeamWords"]);
        Debug.Log("SecondTeamWords: " + PhotonNetwork.CurrentRoom.CustomProperties["SecondTeamWords"]);
        Debug.Log("FirstTeam: " + PhotonNetwork.CurrentRoom.CustomProperties["FirstTeam"]);
        if (PhotonNetwork.CurrentRoom.CustomProperties["SecondTeam"] != null)
        {
            Debug.Log("SecondTeam: " + PhotonNetwork.CurrentRoom.CustomProperties["SecondTeam"]);
        }
        Debug.Log("GAME_PHASE: " + PhotonNetwork.CurrentRoom.CustomProperties[Constants.HASH_ROOM_GAME_PHASE]);
        Debug.Log("currentteam: " + PhotonNetwork.CurrentRoom.CustomProperties[Constants.HASH_ROOM_CURRENT_TEAM]);
        Debug.Log("///////////////////////////////////////////////////////////////////////////");
    }
}
