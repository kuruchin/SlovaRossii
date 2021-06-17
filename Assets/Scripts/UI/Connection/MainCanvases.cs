using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainCanvases : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField nicknameField;

    public LoginScreen LoginScreen;
    public CreateOrJoinRoomCanvas CreateOrJoinRoom;

    public void Start()
    {
        if (Constants.IS_TEST_MODE)
        {
            OnClick_SetNickAndConnectToServer();
        }
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
            LoginScreen.gameObject.SetActive(false);
            CreateOrJoinRoom.gameObject.SetActive(true);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server" + cause.ToString());
    }

    public void OnClick_SetNickAndConnectToServer()
    {
        print("Connecting to server");
        PhotonNetwork.AutomaticallySyncScene = true;
        if(nicknameField.text == "")
        {
            PhotonNetwork.NickName = MasterManager.GameSettings.NickName + MasterManager.GameSettings.NickNameTag;
        }
        else
        {
            PhotonNetwork.NickName = nicknameField.text + MasterManager.GameSettings.NickNameTag;
        }
        
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
    }
}