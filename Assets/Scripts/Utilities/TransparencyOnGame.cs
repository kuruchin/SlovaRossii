using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TransparencyOnGame : MonoBehaviourPunCallbacks
{
    private RawImage background;
    private Color color;

    public void Start()
    {
        background = this.gameObject.GetComponent<RawImage>();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_GAME_PHASE))
        {
            if((MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE] == MainManager.GamePhase.Ending ||
               (MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE] == MainManager.GamePhase.Setup)
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
    }
}
