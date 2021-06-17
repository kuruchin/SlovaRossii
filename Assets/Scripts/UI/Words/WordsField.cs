using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordsField : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TestDictionary testDictionary;

    private string[] tempDictionary;

    [SerializeField]
    private WordPrefab[] wordPrefabMassive;

    [SerializeField]
    private WordPrefab wordPrefab;

    [SerializeField]
    private MainManager mainManager;

    public ColorTheme ColorThemeMain;

    private GridLayoutGroup gridLayoutGroup;

    private int wordStateFirstTeam;
    private int wordStateSecondTeam;
    private int wordStateWhite;
    private int wordStateBlack;

    private int maxWords;
    /// <summary>
    /// Word Field Settings
    /// </summary>
    [SerializeField]
    private TMP_Dropdown initDropdownGameField;
    [SerializeField]
    private TMP_Dropdown initDropdownDictionary;
    [SerializeField]
    private Slider initSliderPlayersWords;
    [SerializeField]
    private Slider initsSiderBlackWords;
    [SerializeField]
    private Slider initsSiderJokerWords;
    [SerializeField]
    private TMP_Dropdown initDropdownJokerIndex;

    private int dictionaryIndex;
    private int gameFieldIndex;

    private List<string> myDictionary = new List<string>();

    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    void Start()
    {
        gridLayoutGroup = this.GetComponent<GridLayoutGroup>();
        mainManager = FindObjectOfType<MainManager>();
        ColorThemeMain = FindObjectOfType<ColorTheme>();
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            maxWords = (int)PhotonNetwork.CurrentRoom.CustomProperties["MaxWords"];

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            ResetGameField();
    }

    public void SetupGameConstant()
    {
        maxWords = SetupGameFieldCount();

        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPC_SetupVar", RpcTarget.AllBuffered, maxWords);

        wordStateFirstTeam = (int)initSliderPlayersWords.value;
        wordStateSecondTeam = wordStateFirstTeam - 1;

        int tempWordCountChecker = 0;
        tempWordCountChecker = wordStateFirstTeam + wordStateSecondTeam + (int)initsSiderBlackWords.value;

        if (maxWords >= tempWordCountChecker)
        {
            wordStateBlack = (int)initsSiderBlackWords.value;
            wordStateWhite = maxWords - tempWordCountChecker;
        }
        else
        {
            wordStateBlack = 0;
            wordStateWhite = 0;
        }
        
        tempDictionary = testDictionary.WordDictionary[initDropdownDictionary.value];

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //Loading to temp dictionary
            _myCustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

            _myCustomProperties["FirstTeamWords"] = wordStateFirstTeam;
            _myCustomProperties["SecondTeamWords"] = wordStateSecondTeam;

            PhotonNetwork.CurrentRoom.SetCustomProperties(_myCustomProperties);
        }
    }

    private int SetupGameFieldCount()
    {
        int returnedValue = 0;

        switch (initDropdownGameField.value)
        {
            // 5x5
            case 0:
                gridLayoutGroup.cellSize = new Vector2(100, 50);
                gridLayoutGroup.constraintCount = 5;
                returnedValue = 25;
                break;
            // 5x6
            case 1:
                gridLayoutGroup.cellSize = new Vector2(100, 41);
                gridLayoutGroup.constraintCount = 5;
                returnedValue = 30;
                break;
            // 6x6
            case 2:
                gridLayoutGroup.cellSize = new Vector2(83, 41);
                gridLayoutGroup.constraintCount = 6;
                returnedValue = 36;
                break;
            // 6x7
            case 3:
                gridLayoutGroup.cellSize = new Vector2(83, 35);
                gridLayoutGroup.constraintCount = 6;
                returnedValue = 42;
                break;
            default:
                gridLayoutGroup.cellSize = new Vector2(100, 50);
                gridLayoutGroup.constraintCount = 5;
                returnedValue = 25;
                break;
        }

        return returnedValue;
    }

    [PunRPC]
    private void RPC_SetupVar(int maxWordsLocal)
    {
        maxWords = maxWordsLocal;
    }

    public void ResetGameField()
    {
        SetupGameConstant();

        // Loading global dictionary to temp dictionary
        myDictionary.Clear();
        foreach (string _word in tempDictionary)
        {
            myDictionary.Add(_word);
        }
        // Write Words and word state on all clients

        int[] wordState = new int[(maxWords)];
        string[] word = new string[(maxWords)];

        for (int j = 0; j < (maxWords); j++)
        {
            //wordState[j] = RandomiseTeamNumberToWord();
            word[j] = RandomiseWordFromDictionary();
        }
        wordState = RandomiseWordsNumber();
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPC_SetupWordsField", RpcTarget.AllBuffered, wordState, word, maxWords);

    }

    [PunRPC]
    private void RPC_SetupWordsField(int[] wordState, string[] wordText, int maxWordsLocal)
    {
        // Clear temp massive
        wordPrefabMassive = new WordPrefab[maxWordsLocal];

        // Clear Word Field
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Recreate all Word Listings
        for (int j = 0; j < (maxWordsLocal); j++)
        {
            WordPrefab _wordPrefab = Instantiate(wordPrefab, transform);
            wordPrefabMassive[j] = _wordPrefab;
            wordPrefabMassive[j].WordNumber = j;
            wordPrefabMassive[j].SetupWords(wordState[j], wordText[j]);

            if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Captain"] == true)
            {
                wordPrefabMassive[j].ShowTrueColorOfWord();
            }
        }
    }

    private int[] RandomiseWordsNumber()
    {
        List<int> tempMassive = new List<int>();
        int[] returnedMassive = new int[maxWords];
        
        for (int i = 0; i < wordStateFirstTeam; i++)
        {
            tempMassive.Add(1);
        }

        for (int i = 0; i < wordStateSecondTeam; i++)
        {
            tempMassive.Add(2);
        }

        for (int i = 0; i < wordStateWhite; i++)
        {
            tempMassive.Add(Constants.WORD_COLOR_WHITE);
        }

        for (int i = 0; i < wordStateBlack; i++)
        {
            tempMassive.Add(Constants.WORD_COLOR_BLACK);
        }

        int tempPosition;
        for (int i = 0; i < maxWords; i++)
        {
            tempPosition = Random.Range(0, tempMassive.Count);
            returnedMassive[i] = tempMassive[tempPosition];
            tempMassive.RemoveAt(tempPosition);
        }

        return returnedMassive;
    }

    private string RandomiseWordFromDictionary()
    {
        int position = UnityEngine.Random.Range(0, myDictionary.Count);
        string result = myDictionary[position];
        myDictionary.RemoveAt(position);
        return result;
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        _myCustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        if (changedProps.ContainsKey("WordPressed") && (bool)_myCustomProperties["IsPaused"] == false)
        {
            Debug.Log("OnPlayerPropertiesUpdate: Word pressed");
            _myCustomProperties = changedProps;
            RefreshPlayerIndicatorsForPlayer(targetPlayer.UserId);
            CheckForAllVoted(targetPlayer);
        }
    }

    private void CheckForAllVoted(Player targetPlayer)
    {
        int _targetWord = (int)targetPlayer.CustomProperties["WordPressed"];
        int _playerTeam = (int)targetPlayer.CustomProperties["TeamNumber"];
        int _numberOfPlayersVoted = 0;
        int _maxNumberOfPlayersVoted = 0;

        if (_playerTeam == 1)
        {
            _maxNumberOfPlayersVoted = (int)PhotonNetwork.CurrentRoom.CustomProperties["FirstTeam"] - 1; // -1 because captain does not count
        }
        else if(_playerTeam == 2)
        {
            _maxNumberOfPlayersVoted = (int)PhotonNetwork.CurrentRoom.CustomProperties["SecondTeam"] - 1;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(player.CustomProperties["WordPressed"] != null &&
               (int)player.CustomProperties["WordPressed"] != Constants.PICKED_WORD_EMPTY)
            {
                if ((int)player.CustomProperties["WordPressed"] == _targetWord && (int)player.CustomProperties["TeamNumber"] == _playerTeam)
                {
                    _numberOfPlayersVoted++;
                }
            }
        }

        if(_numberOfPlayersVoted == _maxNumberOfPlayersVoted && _maxNumberOfPlayersVoted != 0)
        {
            if(_targetWord == Constants.PICKED_WORD_SKIP_TURN)
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    SkipTurn();
            }
            else
            {
                wordPrefabMassive[_targetWord].OpenWord();
            }
        }

    }

    private void RefreshPlayerIndicatorsForPlayer(string userID)
    {
        foreach (WordPrefab _wordPrefab in wordPrefabMassive)
        {
            if(_wordPrefab != null)
            {
                if ((int)_myCustomProperties["WordPressed"] == _wordPrefab.WordNumber)
                {
                    //_wordPrefab.PlayerPlacer.PlayerIndicator[targetPlayer].gameObject.SetActive(true);
                    _wordPrefab.TurnIndicatorOn(userID);
                }
                else
                {
                    // Rework Issue
                    //_wordPrefab.PlayerPlacer.PlayerIndicator[targetPlayer].gameObject.SetActive(false);
                    _wordPrefab.TurnIndicatorOff(userID);
                }
            }
        }
    }

    public void ShowColorOfWords()
    {
        Debug.Log("ShowColorOfWords");
        if((bool)PhotonNetwork.LocalPlayer.CustomProperties["Captain"] == true)
        {
            for (int i = 0; i < (maxWords); i++)
            {
                Debug.Log("color " + i);
                wordPrefabMassive[i].ShowTrueColorOfWord();
            }
        }
    }

    public void SkipTurn()
    {
        mainManager.StartTimer();
    }

}
