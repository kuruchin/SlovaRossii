using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class BlockPushingWhilePaused : MonoBehaviourPunCallbacks
{

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_IS_SLOTS_OPEN))
        {
            this.GetComponent<Button>().interactable = (bool)propertiesThatChanged[Constants.HASH_ROOM_IS_SLOTS_OPEN];
        }
    }
}
