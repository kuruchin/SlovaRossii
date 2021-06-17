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

        ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

        _myCustomProperties.Add("Captain", false);
        _myCustomProperties.Add("TeamNumber", 0);
        _myCustomProperties.Add("WordPressed", Constants.PICKED_WORD_EMPTY);

        PhotonNetwork.LocalPlayer.SetCustomProperties(_myCustomProperties);
    }

}
