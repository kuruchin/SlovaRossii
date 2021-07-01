using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _text.text = roomInfo.Name + " [" + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers + "]";
    }

    public void OnClick_Button()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            {Constants.HASH_PLAYER_IS_CAPTAIN, false},
            {Constants.HASH_PLAYER_TEAM_NUMBER, 0},
            {Constants.HASH_PLAYER_WORD_PRESSED, Constants.PICKED_WORD_EMPTY},
            {Constants.HASH_PLAYER_CAPTAIN_COUNTER, 0}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

}
