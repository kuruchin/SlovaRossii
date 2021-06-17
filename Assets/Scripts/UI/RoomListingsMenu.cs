using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing _roomListing;

    private List<RoomListing> _listings = new List<RoomListing>();
    private RoomsCanvases _roomsCanvases;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }


    public override void OnJoinedRoom()
    {
        //_roomsCanvases.CurrentRoomCanvas.Show();
        //PhotonNetwork.LoadLevel("MainScene");
            //LoadLevel(1);
        _content.DestroyChildren();
        _listings.Clear();
        SceneManager.LoadScene(1);

        ExitGames.Client.Photon.Hashtable tempCustomProperties = new ExitGames.Client.Photon.Hashtable();
        tempCustomProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        tempCustomProperties.Add("Captain", false);
        tempCustomProperties.Add("TeamNumber", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(tempCustomProperties);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo info in roomList)
        {
            //Removed from rooms list
            if (info.RemovedFromList)
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            //Added to rooms list
            else
            {
                int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index == -1)
                {
                    RoomListing listing = Instantiate(_roomListing, _content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _listings.Add(listing);
                    }
                }
                else
                {
                    //Modify listing here
                    //_listings[index].dowhatever.
                }
            }
        }
    }
}
