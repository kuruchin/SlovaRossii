using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class BackgroundColorSync : MonoBehaviourPunCallbacks
{
    private ColorTheme colorTheme;
    private RawImage background;
    private Color color;
    private int currentPlayerTeam;
    private MainManager.GamePhase currentPlayerPhase;
    private int currentTeam;
    private float alpha;

    void Start()
    {
        colorTheme = FindObjectOfType<ColorTheme>();
        background = this.gameObject.GetComponent<RawImage>();
        color = colorTheme.ThemeColor[0];
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer == PhotonNetwork.LocalPlayer)
        {
            if (changedProps.ContainsKey(Constants.HASH_PLAYER_TEAM_NUMBER))
            {
                currentPlayerTeam = (int)changedProps[Constants.HASH_PLAYER_TEAM_NUMBER];
            }

            if (changedProps.ContainsKey(Constants.HASH_PLAYER_IS_CAPTAIN))
            {
                if ((bool)changedProps[Constants.HASH_PLAYER_IS_CAPTAIN])
                {
                    currentPlayerPhase = MainManager.GamePhase.Writing;
                }
                else
                {
                    currentPlayerPhase = MainManager.GamePhase.Opening;
                }
            }
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_CURRENT_TEAM))
        {
            color = colorTheme.ThemeColor[(int)propertiesThatChanged[Constants.HASH_ROOM_CURRENT_TEAM]];
            currentTeam = (int)propertiesThatChanged[Constants.HASH_ROOM_CURRENT_TEAM];
        }

        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_GAME_PHASE))
        {
            if ((MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE] == MainManager.GamePhase.Ending ||
                (MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE] == MainManager.GamePhase.Setup ||
               ((MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE] == currentPlayerPhase &&
                currentTeam == currentPlayerTeam))
            {
                alpha = 1f;
            }
            else
            {
                alpha = 0.5f;
            }
        }
        
        color = new Color(color.r, color.g, color.b, alpha);
    }
    


    void Update()
    {
        background.color = color;
    }
}
