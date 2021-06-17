using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FieldSettings : MonoBehaviourPunCallbacks
{
    private bool isShowingAll = true;
    private bool isShowingRestartWindow = true;

    [SerializeField]
    private GameObject buttonSettings;
    [SerializeField]
    private GameObject buttonRestart;
    [SerializeField]
    private GameObject windowRestart;
    [SerializeField]
    private GameObject fieldSettings;

    private MainManager mainManager;

    public void Start()
    {
        mainManager = FindObjectOfType<MainManager>();

        if (!PhotonNetwork.IsMasterClient)
        {
            isShowingAll = false;
            isShowingRestartWindow = false;
        }
        else
        {
            isShowingAll = true;
            isShowingRestartWindow = true;
        }

        buttonSettings.SetActive(isShowingAll);
        fieldSettings.SetActive(isShowingAll);
        windowRestart.SetActive(isShowingRestartWindow);
        buttonRestart.SetActive(!isShowingRestartWindow);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick_HideOrShow();
        }
    }

    public void OnClick_HideOrShow()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        isShowingAll = !isShowingAll;
        fieldSettings.SetActive(isShowingAll);
    }

    public void OnClick_Restart()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        RestartGameWindowSwitch();

        mainManager.ResetGame();
    }

    public void OnClick_StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        RestartGameWindowSwitch();

        mainManager.StartGame();
    }

    private void RestartGameWindowSwitch()
    {
        isShowingRestartWindow = !isShowingRestartWindow;

        windowRestart.SetActive(isShowingRestartWindow);
        buttonRestart.SetActive(!isShowingRestartWindow);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_IS_PAUSED))
        {
            if((bool)propertiesThatChanged[Constants.HASH_ROOM_IS_PAUSED] == false)
            {
                fieldSettings.SetActive(false);
                isShowingAll = !isShowingAll;
            }
        }
    }
}
