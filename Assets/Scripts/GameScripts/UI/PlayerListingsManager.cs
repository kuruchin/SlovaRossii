using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListingsManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private Transform[] _team;
    [SerializeField]
    private Transform[] captainTeam;
    [SerializeField]
    private Transform[] soldierTeam;
    [SerializeField]
    private PlayerListing _playerListing;

    [SerializeField]
    private List<PlayerListing> _listings = new List<PlayerListing>();
    private RoomsCanvases _roomCanvases;

    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    public override void OnEnable()
    {
        base.OnEnable();

        //SetReadyUp(false);
        for (int i = 1; i < 3; i++)
        {
            captainTeam[i].DestroyChildren();
            soldierTeam[i].DestroyChildren();
        }
        GetCurrentRoomPlayers();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < _listings.Count; i++)
        {
            Destroy(_listings[i].gameObject);
        }

        _listings.Clear();
    }

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;
        
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player player)
    {
        int index = _listings.FindIndex(x => x.Player == player);
        if(index != -1)
        {
            Debug.Log("AddPlayerListing1: " + index);
            _listings[index].transform.SetParent(GetPlayerTransform(player));
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListing, GetPlayerTransform(player));

            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _listings.Add(listing);
            }
        }
    }
    // rework
    private Transform GetPlayerTransform(Player player)
    {
        int playerTeamNumber = (int)player.CustomProperties["TeamNumber"];
        bool isCaptain = (bool)player.CustomProperties["Captain"];
        Transform currentTransform;
        if (isCaptain)
        {
            currentTransform = captainTeam[playerTeamNumber];
        }
        else
        {
            currentTransform = soldierTeam[playerTeamNumber];
        }

        return currentTransform;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemovePlayerListing(otherPlayer);
    }

    private void RemovePlayerListing(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    private void SetupPlayersCountInTeams()
    {
        int firstTeam = 0;
        int secondTeam = 0;
        ExitGames.Client.Photon.Hashtable counters = new ExitGames.Client.Photon.Hashtable();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if ((int)player.CustomProperties["TeamNumber"] == 1)
            {
                firstTeam++;
            }
            else if ((int)player.CustomProperties["TeamNumber"] == 2)
            {
                secondTeam++;
            }
        }
        //PhotonNetwork.CurrentRoom.CustomProperties["FirstTeam"];
        counters.Add("FirstTeam", firstTeam);
        counters.Add("SecondTeam", secondTeam);

        PhotonNetwork.CurrentRoom.SetCustomProperties(counters);
    }

    private void OnPlayerChangedTeam(Player targetPlayer, int teamTransform, bool isCaptain)
    {
        int index = _listings.FindIndex(x => x.Player == targetPlayer);
        if (index != -1)
        {
            Debug.Log("OnPlayerChangedTeam: " + index + ", " + teamTransform + ", " + isCaptain);
            if (isCaptain)
            {
                _listings[index].gameObject.transform.SetParent(captainTeam[teamTransform], false);
            }
            else
            {
                _listings[index].gameObject.transform.SetParent(soldierTeam[teamTransform], false);
            }
        }
    }
    
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("TeamNumber") || changedProps.ContainsKey("Captain"))
        {
            int _teamNumber = (int)changedProps["TeamNumber"];
            bool _isCaptain = (bool)changedProps["Captain"];
            OnPlayerChangedTeam(targetPlayer, _teamNumber, _isCaptain);
        }
    }

    //public void ResetListings()
    //{
    //    foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
    //    {
    //        RemovePlayerListing(playerInfo.Value);
    //    }
    //
    //    GetCurrentRoomPlayers();
    //}
}
