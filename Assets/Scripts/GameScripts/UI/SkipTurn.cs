using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkipTurn : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private PlayerIndicatorGroup playerIndicatorGroup;

    [SerializeField]
    private int WordNumber;
    [SerializeField]
    private int teamNumber;

    [SerializeField]
    private GameObject buttonSkip;

    private MainManager.GamePhase currentGamePhase;
    private int currentTeamNumber;

    public void Start()
    {
        WordNumber = Constants.PICKED_WORD_SKIP_TURN;
    }

    public void Update()
    {
        if (currentGamePhase == MainManager.GamePhase.Opening &&
            currentTeamNumber == teamNumber)
        {
            buttonSkip.SetActive(true);
        }
        else
        {
            buttonSkip.SetActive(false);
        }
    }

    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    public void OnClick_SkipTurn()
    {
        ExitGames.Client.Photon.Hashtable _localPlayerHash = PhotonNetwork.LocalPlayer.CustomProperties;
        ExitGames.Client.Photon.Hashtable _currentRoomHash = PhotonNetwork.CurrentRoom.CustomProperties;

        if ((bool)_localPlayerHash[Constants.HASH_PLAYER_IS_CAPTAIN] == false &&
            (int)_currentRoomHash[Constants.HASH_ROOM_CURRENT_TEAM] == (int)_localPlayerHash[Constants.HASH_PLAYER_TEAM_NUMBER] &&
            teamNumber == (int)_localPlayerHash[Constants.HASH_PLAYER_TEAM_NUMBER] &&
            (MainManager.GamePhase)_currentRoomHash[Constants.HASH_ROOM_GAME_PHASE] == MainManager.GamePhase.Opening)
            SetWordPressed();
    }

    private void SetWordPressed()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["WordPressed"] == null)
        {
            _myCustomProperties["WordPressed"] = WordNumber;
        }
        else
        {
            if ((int)PhotonNetwork.LocalPlayer.CustomProperties["WordPressed"] == WordNumber)
            {
                _myCustomProperties["WordPressed"] = Constants.PICKED_WORD_EMPTY;
            }
            else
            {
                _myCustomProperties["WordPressed"] = WordNumber;
            }
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(_myCustomProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        _myCustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        if (changedProps.ContainsKey("WordPressed") && (bool)_myCustomProperties["IsPaused"] == false)
        {
            ExitGames.Client.Photon.Hashtable _localPlayerHash = targetPlayer.CustomProperties;
            RefreshPlayerIndicatorsForPlayer(targetPlayer.UserId, _localPlayerHash);
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_GAME_PHASE))
        {
            currentGamePhase = (MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE];
        }

        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_CURRENT_TEAM))
        {
            currentTeamNumber = (int)propertiesThatChanged[Constants.HASH_ROOM_CURRENT_TEAM];
        }
    }

    private void RefreshPlayerIndicatorsForPlayer(string userID, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if ((int)changedProps[Constants.HASH_PLAYER_WORD_PRESSED] == Constants.PICKED_WORD_SKIP_TURN &&
            (int)changedProps[Constants.HASH_PLAYER_TEAM_NUMBER] == teamNumber)
        {
            playerIndicatorGroup.TurnIndicatorOn(userID);
        }
        else
        {
            playerIndicatorGroup.TurnIndicatorOff(userID);
        }
    }
}
