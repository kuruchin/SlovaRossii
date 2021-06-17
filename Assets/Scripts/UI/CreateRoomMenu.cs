using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _roomName;

    private RoomsCanvases _roomsCanvases;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        options.MaxPlayers = Constants.MAX_PLAYER_IN_ROOM;
        options.PublishUserId = true;
        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully", this);
        //_roomsCanvases.CurrentRoomCanvas.Show();
        SetupRoomHashtable();
        SetupHostHashtable();
        SceneManager.LoadScene(1);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message, this);
    }

    private void SetupRoomHashtable()
    {
        ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

        _myCustomProperties.Add("GamePhase", MainManager.GamePhase.Setup);
        _myCustomProperties.Add("CurrentTeam", 1);
        _myCustomProperties.Add("IsPaused", true);
        _myCustomProperties.Add("FirstTeam", 0);
        _myCustomProperties.Add("SecondTeam", 0);
        _myCustomProperties.Add("FirstTeamWords", 0);
        _myCustomProperties.Add("SecondTeamWords", 0);
        _myCustomProperties.Add("MaxWords", 0);
        _myCustomProperties.Add(Constants.HASH_ROOM_IS_SLOTS_OPEN, true);

        PhotonNetwork.CurrentRoom.SetCustomProperties(_myCustomProperties);
    }

    private void SetupHostHashtable()
    {
        //ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();
        //
        //_myCustomProperties.Add("Captain", false);
        //_myCustomProperties.Add("TeamNumber", 0);
        //_myCustomProperties.Add("WordPressed", Constants.PICKED_WORD_EMPTY);
        //
        //PhotonNetwork.LocalPlayer.SetCustomProperties(_myCustomProperties);

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            {Constants.HASH_PLAYER_IS_CAPTAIN, false},
            {Constants.HASH_PLAYER_TEAM_NUMBER, 0},
            {Constants.HASH_PLAYER_WORD_PRESSED, Constants.PICKED_WORD_EMPTY}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
}
