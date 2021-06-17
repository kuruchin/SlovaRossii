using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChooseIndicatorPrefab : MonoBehaviour
{
    public string UserID { get; private set; }

    public void SetPlayerID(string userID)
    {
        UserID = userID;
    }
}
