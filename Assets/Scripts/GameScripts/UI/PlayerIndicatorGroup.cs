using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerIndicatorGroup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerChooseIndicatorPrefab playerChooseIndicatorPrefab;
    
    public List<PlayerChooseIndicatorPrefab> PlayerChooseIndicators = new List<PlayerChooseIndicatorPrefab>();

    public override void OnEnable()
    {
        base.OnEnable();

        GetCurrentRoomPlayersID();
    }

    private void GetCurrentRoomPlayersID()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerIDToList(playerInfo.Value);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerIDToList(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = PlayerChooseIndicators.FindIndex(x => x.UserID == otherPlayer.UserId);
        if (index != -1)
        {
            PlayerChooseIndicators.RemoveAt(index);
        }
    }

    private void AddPlayerIDToList(Player newPlayer)
    {
        int index = PlayerChooseIndicators.FindIndex(x => x.UserID == newPlayer.UserId);
        if (index != -1)
        {
            Debug.Log("AddPlayerIDToList Fail: " + newPlayer.UserId);
            PlayerChooseIndicators[index].SetPlayerID(newPlayer.UserId);
        }
        else
        {
            PlayerChooseIndicatorPrefab _playerChooseIndicatorPrefab = Instantiate(playerChooseIndicatorPrefab, this.transform);

            if (_playerChooseIndicatorPrefab != null)
            {
                _playerChooseIndicatorPrefab.SetPlayerID(newPlayer.UserId);
                PlayerChooseIndicators.Add(_playerChooseIndicatorPrefab);
                _playerChooseIndicatorPrefab.gameObject.SetActive(false);
            }
        }
    }

    public void TurnIndicatorOn(string userID)
    {
        IndicatorSwitch(userID, true);
    }

    public void TurnIndicatorOff(string userID)
    {
        IndicatorSwitch(userID, false);
    }

    private void IndicatorSwitch(string userID, bool state)
    {
        int index = PlayerChooseIndicators.FindIndex(x => x.UserID == userID);
        if (index != -1)
        {
            //Debug.Log("TurnIndicatorSwitch Done: " + userID);
            PlayerChooseIndicators[index].gameObject.SetActive(state);
        }
        else
        {
            Debug.Log("//////////////TurnIndicatorSwitch fail: " + userID);
        }
    }

}
