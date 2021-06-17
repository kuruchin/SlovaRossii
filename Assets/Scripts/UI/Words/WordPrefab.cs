using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordPrefab : MonoBehaviour
{
    [SerializeField]
    private MainManager mainManager;

    private Photon.Pun.UtilityScripts.TestTimer testTimer;

    public int WordNumber;
    [SerializeField]
    private int WordState;
    [SerializeField]
    private Text WordText;
    [SerializeField]
    private ColorTheme colorTheme;
    [SerializeField]
    private bool isOpen = false;

    [SerializeField]
    private PlayerIndicatorGroup playerIndicatorGroup;

    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    private void Start()
    {
        colorTheme = FindObjectOfType<ColorTheme>();
        mainManager = FindObjectOfType<MainManager>();
        testTimer = FindObjectOfType<Photon.Pun.UtilityScripts.TestTimer>();
    }

    public void SetupWords(int wordState, string wordText)
    {
        WordState = wordState;
        WordText.text = wordText;
        //ChangeColor();
    }

    //public void ChangeColor()
    //{
    //    if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Captain"] == true)
    //    {
    //        ShowTrueColorOfWord();
    //    }
    //}

    public void ShowTrueColorOfWord()
    {
        this.GetComponent<Image>().color = colorTheme.ThemeColor[WordState];
    }

    private void MakeWordTransparent()
    {
        //Color of back
        this.GetComponent<Image>().color = Transparenting(this.GetComponent<Image>().color);
        //Color of text
        WordText.color = Transparenting(WordText.color);
    }

    private Color Transparenting(Color inputColor)
    {
        Color _color = inputColor;
        _color.a = 0.4f;
        return _color;
    }

    public void OpenWord()
    {
        if((bool)PhotonNetwork.LocalPlayer.CustomProperties["Captain"] == true)
        {
            MakeWordTransparent();
        }
        else
        {
            ShowTrueColorOfWord();
        }

        isOpen = true;

        RecalculateRemainingWords();
        CheckForGameEndOrNewPhase();
    }

    private void CheckForGameEndOrNewPhase()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            return;

        ExitGames.Client.Photon.Hashtable _hashtable = PhotonNetwork.CurrentRoom.CustomProperties;

        if ((int)_hashtable["FirstTeamWords"] == 0 ||
           (int)_hashtable["SecondTeamWords"] == 0)
        {
            mainManager.EndGame();
        }
        else
        {
            CheckOpenedWordOnNewPhase();
        }
    }

    private void RecalculateRemainingWords()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            return;

        ExitGames.Client.Photon.Hashtable _newHashtable = PhotonNetwork.CurrentRoom.CustomProperties;
        int _newCounter;

        if(WordState == 1)
        {
            _newCounter = (int)_newHashtable["FirstTeamWords"] - 1;
            _newHashtable.Remove("FirstTeamWords");
            _newHashtable.Add("FirstTeamWords", _newCounter);

            PhotonNetwork.CurrentRoom.SetCustomProperties(_newHashtable);
        }else if (WordState == 2)
        {
            _newCounter = (int)_newHashtable["SecondTeamWords"] - 1;
            _newHashtable.Remove("SecondTeamWords");
            _newHashtable.Add("SecondTeamWords", _newCounter);

            PhotonNetwork.CurrentRoom.SetCustomProperties(_newHashtable);
        }
        else if (WordState == Constants.WORD_COLOR_BLACK)
        {
            int currentTeam = (int)PhotonNetwork.CurrentRoom.CustomProperties[Constants.HASH_ROOM_CURRENT_TEAM];
            if (currentTeam == 2)
            {
                _newHashtable.Remove("FirstTeamWords");
                _newHashtable.Add("FirstTeamWords", 0);

                PhotonNetwork.CurrentRoom.SetCustomProperties(_newHashtable);
            }
            else if (currentTeam == 1)
            {
                _newHashtable.Remove("SecondTeamWords");
                _newHashtable.Add("SecondTeamWords", 0);

                PhotonNetwork.CurrentRoom.SetCustomProperties(_newHashtable);
            }
        }
    }

    private void CheckOpenedWordOnNewPhase()
    {
        //if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        //    return;

        if (WordState != (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentTeam"])
        {
            // End turn of current team and start new turn
            mainManager.StartTimer();
        }
        else
        {
            // Players Hashtables [WordPressed] reset
            mainManager.ResetWordPressed();
            // Add Time to timer
            Debug.Log("TEST1");
            testTimer.AddTimeToTimer(15);
        }
    }

    public void SkipRound()
    {
        mainManager.StartTimer();
    }

    public void OnClick_Button()
    {
        ExitGames.Client.Photon.Hashtable _localPlayerHash = PhotonNetwork.LocalPlayer.CustomProperties;
        ExitGames.Client.Photon.Hashtable _currentRoomHash = PhotonNetwork.CurrentRoom.CustomProperties;

        if (!isOpen && 
            (bool)_localPlayerHash["Captain"] == false &&
            (int)_currentRoomHash["CurrentTeam"] == (int)_localPlayerHash["TeamNumber"] &&
            (MainManager.GamePhase)_currentRoomHash["GamePhase"] == MainManager.GamePhase.Opening &&
            !(bool)_currentRoomHash[Constants.HASH_ROOM_IS_SLOTS_OPEN] &&
            !(bool)_currentRoomHash[Constants.HASH_ROOM_IS_PAUSED])
            SetWordPressed();
    }

    private void SetWordPressed()
    {
        if(PhotonNetwork.LocalPlayer.CustomProperties["WordPressed"] == null)
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

    public void TurnIndicatorOn(string userID)
    {
        playerIndicatorGroup.TurnIndicatorOn(userID);
    }

    public void TurnIndicatorOff(string userID)
    {
        playerIndicatorGroup.TurnIndicatorOff(userID);
    }

}
