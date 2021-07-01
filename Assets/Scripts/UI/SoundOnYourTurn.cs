using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Audio;

public class SoundOnYourTurn : MonoBehaviourPunCallbacks
{
    private MainManager.GamePhase currentGamePhase;
    private int currentTeamNumber;

    private MainManager.GamePhase neededGamePhase;
    private int neededTeamNumber;

    [SerializeField]
    private AudioSource startTurnSound;

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

        if (currentGamePhase == neededGamePhase && currentTeamNumber == neededTeamNumber)
            PlayStartTurnSound();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey(Constants.HASH_PLAYER_IS_CAPTAIN) && targetPlayer == PhotonNetwork.LocalPlayer)
        {
            if ((bool)changedProps[Constants.HASH_PLAYER_IS_CAPTAIN] == true)
                neededGamePhase = MainManager.GamePhase.Writing;
            else
                neededGamePhase = MainManager.GamePhase.Opening;
        }

        if (changedProps.ContainsKey(Constants.HASH_PLAYER_TEAM_NUMBER) && targetPlayer == PhotonNetwork.LocalPlayer)
        {
            neededTeamNumber = (int)changedProps[Constants.HASH_PLAYER_TEAM_NUMBER];
        }
    }

    public void PlayStartTurnSound()
    {
        startTurnSound.Play();
        Debug.Log("SoundEffect 1");
    }
}
