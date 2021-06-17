using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CreateOrJoinRoomCanvas : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private CreateRoomMenu _createRoomMenu;

    [SerializeField]
    private RoomListingsMenu _roomListingsMenu;

    private RoomsCanvases _roomsCanvases;

    [SerializeField]
    private Text nickName;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
        _createRoomMenu.FirstInitialize(canvases);
        _roomListingsMenu.FirstInitialize(canvases);
    }

    public override void OnEnable()
    {
        nickName.text = PhotonNetwork.NickName;
    }
}
