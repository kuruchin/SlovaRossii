using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestConnect : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text nickName;
    // Start is called before the first frame update
    void Start()
    {
        print("Connecting to server");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        nickName.text = PhotonNetwork.NickName;
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to server");
        print(PhotonNetwork.LocalPlayer.NickName);
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            Debug.LogFormat("1) Connected AppVersion={0}", PhotonNetwork.GameVersion);
            Debug.Log("1) Connected AppVersion=" + PhotonNetwork.AppVersion);
            Debug.Log("1) Connected AppVersion=" + MasterManager.GameSettings.GameVersion);
            Debug.LogFormat("2) Connected AppVersion={0}", PhotonNetwork.NetworkingClient.AppVersion);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server" + cause.ToString());
    }
}
