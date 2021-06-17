using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class QuitButton : MonoBehaviourPunCallbacks
{
    public void OnClick_Escape()
    {
        Application.Quit();
    }

    public void OnClick_QuitToRoomList()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }
}
