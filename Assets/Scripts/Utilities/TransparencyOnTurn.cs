using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class TransparencyOnTurn : MonoBehaviourPunCallbacks
{
    private RawImage background;
    private Color color;
    private int currentPlayerTeam;
    private int currentTeam;

    public void Start()
    {
        background = this.gameObject.GetComponent<RawImage>();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(targetPlayer == PhotonNetwork.LocalPlayer &&
           changedProps.ContainsKey(Constants.HASH_PLAYER_TEAM_NUMBER))
        {
            currentPlayerTeam = (int)changedProps[Constants.HASH_PLAYER_TEAM_NUMBER];
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {

        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_GAME_PHASE))
        {
            if ((MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE] == MainManager.GamePhase.Ending ||
                (MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE] == MainManager.GamePhase.Setup ||
               ((MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE] == MainManager.GamePhase.Opening &&
                currentTeam == currentPlayerTeam))
            {
                color = new Color32(0, 0, 0, 255);
                if (background) background.color = new Color(background.color.r, background.color.g, background.color.b, color.a);
            }
            else
            {
                color = new Color32(0, 0, 0, 130);
                if (background) background.color = new Color(background.color.r, background.color.g, background.color.b, color.a);
            }
        }

        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_CURRENT_TEAM))
        {
            currentTeam = (int)propertiesThatChanged[Constants.HASH_ROOM_CURRENT_TEAM];
        }
    }
}
