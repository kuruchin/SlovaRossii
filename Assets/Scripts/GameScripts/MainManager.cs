using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Realtime;


public class MainManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField firstTeamWordInput;
    [SerializeField]
    private TextMeshProUGUI firstTeamWordList;
    [SerializeField]
    private CaptainField firstCaptainField;

    [SerializeField]
    private TMP_InputField secondTeamWordInput;
    [SerializeField]
    private TextMeshProUGUI secondTeamWordList;
    [SerializeField]
    private CaptainField secondCaptainField;


    [SerializeField]
    private Transform _content;
    [SerializeField]
    private Transform[] _team;
    [SerializeField]
    private PlayerListing _playerListing;

    private List<PlayerListing> _listings = new List<PlayerListing>();
    private RoomsCanvases _roomCanvases;

    [SerializeField]
    private WordsField wordsField;
    private TestTimer turnTimer;

    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private GameObject loseScreen;

    //private GamePhase gamePhase = GamePhase.Start;

    public override void OnEnable()
    {
        base.OnEnable();
        //SetReadyUp(false);
        //GetCurrentRoomPlayers();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < _listings.Count; i++)
        {
            Destroy(_listings[i].gameObject);
        }

        _listings.Clear();
    }

    public enum GamePhase
    {
        Setup = 0,
        Writing = 1,
        Opening = 2,
        Ending = 3,
        Start = 4,
        Pause = 5
    }

    private void SetupNextGamePhase()
    {
        ExitGames.Client.Photon.Hashtable newHashtable = PhotonNetwork.CurrentRoom.CustomProperties;

        GamePhase currentGamePhase = (GamePhase)newHashtable[Constants.HASH_ROOM_GAME_PHASE];
        int currentTeam = (int)newHashtable[Constants.HASH_ROOM_CURRENT_TEAM];
        GamePhase _gamePhase;
        int _currentTeam = currentTeam;

        Debug.Log("SetupNextGamePhase");
        switch (currentGamePhase)
        {
            //case GamePhase.Start:
            //    _gamePhase = GamePhase.Opening;
            //    break;
            case GamePhase.Opening:
                _gamePhase = GamePhase.Writing;
                break;
            case GamePhase.Writing:
                _gamePhase = GamePhase.Opening;
                break;
            default:
                _gamePhase = GamePhase.Pause;
                break;
        }
        // Change team
        if (_gamePhase == GamePhase.Writing)
        {
            if(currentTeam == 2)
            {
                _currentTeam = 1;
            }
            else
            {
                _currentTeam = 2;
            }
        }

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                {Constants.HASH_ROOM_GAME_PHASE, _gamePhase},
                {Constants.HASH_ROOM_CURRENT_TEAM, _currentTeam}
            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        SetNextCaptain(_gamePhase, _currentTeam);
    }

    private void Start()
    {

        wordsField = FindObjectOfType<WordsField>();
        turnTimer = this.GetComponent<TestTimer>();

        firstCaptainField.gameObject.SetActive(false);
        secondCaptainField.gameObject.SetActive(false);
    }

    public void ResetTimer()
    {
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
            StartTimer();
    }

    public void StartTimer()
    {
        if ((GamePhase)PhotonNetwork.CurrentRoom.CustomProperties["GamePhase"] != GamePhase.Ending)
        {
            SetupNextGamePhase();
            //PhotonView photonView = PhotonView.Get(this);
            turnTimer.RunTimer();
            ResetWordPressed();
        }
    }

    public void ResetWordPressed()
    {
        ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            _myCustomProperties["WordPressed"] = Constants.PICKED_WORD_EMPTY;
            player.SetCustomProperties(_myCustomProperties);
        }
    }

    private void SetNextCaptain(GamePhase gamePhase, int currentTeam)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPC_SetNextCaptain", RpcTarget.AllBuffered, gamePhase, currentTeam);
    }

    [PunRPC]
    private void RPC_SetNextCaptain(GamePhase _gamePhase, int _currentTeam)
    {
        //GamePhase _gamePhase = (GamePhase)PhotonNetwork.CurrentRoom.CustomProperties["GamePhase"];
        bool _isCaptain = (bool)PhotonNetwork.LocalPlayer.CustomProperties["Captain"];
        //int _currentTeam = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentTeam"];
        int _teamNumber = (int)PhotonNetwork.LocalPlayer.CustomProperties["TeamNumber"];

        if (_gamePhase == GamePhase.Writing && _isCaptain && _teamNumber == _currentTeam)
        {
            if(_teamNumber == 1)
            {
                firstCaptainField.gameObject.SetActive(true);
            }
            else if(_teamNumber == 2)
            {
                secondCaptainField.gameObject.SetActive(true);
            }

        }
        else
        {
            firstCaptainField.gameObject.SetActive(false);
            secondCaptainField.gameObject.SetActive(false);
        }
    }

    // Before Start Game
    /*
    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player player)
    {
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _listings[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListing, _content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _listings.Add(listing);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }
    */

    // rework two to one function
    public void OnClick_FirstTeamSendWord()
    {
        if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Captain"] == false ||
            firstTeamWordInput.text == "")
            return;

        FirstTeamSendWord(firstTeamWordInput.text);
        // Clean Input Field
        firstTeamWordInput.text = string.Empty;
        firstCaptainField.gameObject.SetActive(false);

        //StartNewGamePhase();
        StartTimer();
    }

    private void FirstTeamSendWord(string word)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPC_FirstTeamSendWord", RpcTarget.AllBuffered, word);
    }

    [PunRPC]
    private void RPC_FirstTeamSendWord(string word)
    {
        //SetGamePhase(_gamePhase);
        firstTeamWordList.text = firstTeamWordList.text + word + "\n";
    }

    public void OnClick_SecondTeamSendWord()
    {
        if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Captain"] == false ||
            secondTeamWordInput.text == "")
            return;

        SecondTeamSendWord(secondTeamWordInput.text);
        // Clean Input Field
        secondTeamWordInput.text = string.Empty;
        secondCaptainField.gameObject.SetActive(false);

        //StartNewGamePhase();
        StartTimer();
    }

    private void SecondTeamSendWord(string word)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPC_SecondTeamSendWord", RpcTarget.AllBuffered, word);
    }

    [PunRPC]
    private void RPC_SecondTeamSendWord(string word)
    {
        //SetGamePhase(_gamePhase);
        secondTeamWordList.text = secondTeamWordList.text + word + "\n";
    }


    public void EndGame()
    {
        ExitGames.Client.Photon.Hashtable newHashtable = PhotonNetwork.CurrentRoom.CustomProperties;

        //newHashtable.Remove("GamePhase");
        //newHashtable.Add("GamePhase", GamePhase.Ending);
        newHashtable["GamePhase"] = GamePhase.Ending;
        newHashtable["IsPaused"] = true;
        PhotonNetwork.CurrentRoom.SetCustomProperties(newHashtable);

        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPC_EndGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_EndGame()
    {
        ExitGames.Client.Photon.Hashtable newHashtable = PhotonNetwork.CurrentRoom.CustomProperties;

        // REWORK
        if ((int)newHashtable["FirstTeamWords"] == 0)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player == PhotonNetwork.LocalPlayer)
                {
                    if ((int)player.CustomProperties["TeamNumber"] == 1)
                    {
                        winScreen.SetActive(true);
                    }
                    else
                    {
                        loseScreen.SetActive(true);
                    }
                }
            }
        }

        if ((int)newHashtable["SecondTeamWords"] == 0)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player == PhotonNetwork.LocalPlayer)
                {
                    if ((int)player.CustomProperties["TeamNumber"] == 2)
                    {
                        winScreen.SetActive(true);
                    }
                    else
                    {
                        loseScreen.SetActive(true);
                    }
                }
            }
        }
    }


    public void OnClick_SkipWait()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            return;

        StartTimer();
    }

    public void StartGame()
    {
        GamePhase _gamePhase = (GamePhase)PhotonNetwork.CurrentRoom.CustomProperties["GamePhase"];
        bool _isPaused = (bool)PhotonNetwork.CurrentRoom.CustomProperties["IsPaused"];

        if(_gamePhase == GamePhase.Setup && _isPaused == true)
        {
            wordsField.ResetGameField();

            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RPC_StartRoundUISetup", RpcTarget.AllBuffered);
            SetupPlayersCountInTeams();

            ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();
            _myCustomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

            _myCustomProperties["CurrentTeam"] = 1;
            _myCustomProperties["GamePhase"] = GamePhase.Writing;
            _myCustomProperties["IsPaused"] = false;
            _myCustomProperties[Constants.HASH_ROOM_IS_SLOTS_OPEN] = false;

            PhotonNetwork.CurrentRoom.SetCustomProperties(_myCustomProperties);
        }
    }

    // rework to self-working
    [PunRPC]
    private void RPC_StartRoundUISetup()
    {
        // Reset Words Field
        firstTeamWordList.text = "";
        secondTeamWordList.text = "";

        bool _isCaptain = (bool)PhotonNetwork.LocalPlayer.CustomProperties["Captain"];
        int _teamNumber = (int)PhotonNetwork.LocalPlayer.CustomProperties["TeamNumber"];
        Debug.Log("RPC_StartRound");
        //wordsField.ShowColorOfWords();
        //photonView.RPC("RPC_Debug", RpcTarget.AllBuffered, "RPC_StartRound " + PhotonNetwork.LocalPlayer.NickName);
        if (_isCaptain == true && _teamNumber == 1)
        {
            firstCaptainField.gameObject.SetActive(true);
            //photonView.RPC("RPC_Debug", RpcTarget.AllBuffered, "RPC_StartRound 2" + PhotonNetwork.LocalPlayer.NickName);
        }
    }

    private void SetupPlayersCountInTeams()
    {
        int firstTeam = 0;
        int secondTeam = 0;
        ExitGames.Client.Photon.Hashtable counters = new ExitGames.Client.Photon.Hashtable();
        counters = PhotonNetwork.CurrentRoom.CustomProperties;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if ((int)player.CustomProperties["TeamNumber"] == 1)
            {
                firstTeam++;
            }
            else if ((int)player.CustomProperties["TeamNumber"] == 2)
            {
                secondTeam++;
            }
        }
        //PhotonNetwork.CurrentRoom.CustomProperties["FirstTeam"];
        //counters.Add("FirstTeam", firstTeam);
        //counters.Add("SecondTeam", secondTeam);
        counters["FirstTeam"] = firstTeam;
        counters["SecondTeam"] = secondTeam;

        PhotonNetwork.CurrentRoom.SetCustomProperties(counters);
    }

    public void OnClick_OpenSlots()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            return;

        ResetWordPressed();
        SetupPlayersCountInTeams();

        bool isSlotsOpen = !(bool)PhotonNetwork.CurrentRoom.CustomProperties[Constants.HASH_ROOM_IS_SLOTS_OPEN];

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            {Constants.HASH_ROOM_IS_SLOTS_OPEN, isSlotsOpen}
        };

        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    // pause
    public void OnClick_Pause()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            return;

        bool isPaused = !(bool)PhotonNetwork.CurrentRoom.CustomProperties[Constants.HASH_ROOM_IS_PAUSED];

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            {Constants.HASH_ROOM_IS_PAUSED, isPaused}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        //if (!isPaused)
        //{
        //    ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        //    {
        //        {Constants.HASH_ROOM_IS_PAUSED, true}
        //    };
        //    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        //}
    }

    public void OnClick_ShufflePlayers()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
            return;
        
        List<Player> playerList = new List<Player>();

        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            ExitGames.Client.Photon.Hashtable _localPlayerHash = playerInfo.Value.CustomProperties;

            if ((int)_localPlayerHash[Constants.HASH_PLAYER_TEAM_NUMBER] != 0)
            {
                playerList.Add(playerInfo.Value);
            }
        }

        // Need atleast 4 players for shuffle
        if(playerList.Count > 3)
        {
            int maxPlayerCount = playerList.Count;
            for (int i = 0; i < maxPlayerCount; i += 2)
            {
                int rnd1 = Random.Range(0, playerList.Count);
                SetPlayerTeamNumber(playerList[rnd1], 1);
                playerList.RemoveAt(rnd1);

                if(playerList.Count != 0)
                {
                    int rnd2 = Random.Range(0, playerList.Count);
                    SetPlayerTeamNumber(playerList[rnd2], 2);
                    playerList.RemoveAt(rnd2);
                }
            }

            List<Player> playerList1 = new List<Player>();
            List<Player> playerList2 = new List<Player>();

            foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
            {
                ExitGames.Client.Photon.Hashtable _localPlayerHash = playerInfo.Value.CustomProperties;

                if ((int)_localPlayerHash[Constants.HASH_PLAYER_TEAM_NUMBER] == 1)
                {
                    playerList1.Add(playerInfo.Value);
                    SetPlayerCaptain(playerInfo.Value, false);
                }
                else if ((int)_localPlayerHash[Constants.HASH_PLAYER_TEAM_NUMBER] == 2)
                {
                    playerList2.Add(playerInfo.Value);
                    SetPlayerCaptain(playerInfo.Value, false);
                }
            }
            int rndCaptain1 = Random.Range(0, playerList1.Count);
            SetPlayerCaptain(playerList1[rndCaptain1], true);
            int rndCaptain2 = Random.Range(0, playerList1.Count);
            SetPlayerCaptain(playerList2[rndCaptain2], true);
        }

    }

    // Rework
    private void SetPlayerTeamNumber(Player player, int teamNumber)
    {
        ExitGames.Client.Photon.Hashtable _localPlayerHash = player.CustomProperties;
        
        _localPlayerHash[Constants.HASH_PLAYER_TEAM_NUMBER] = teamNumber;

        player.SetCustomProperties(_localPlayerHash);
    }
    // Rework
    private void SetPlayerCaptain(Player player, bool isCaptain)
    {
        ExitGames.Client.Photon.Hashtable _localPlayerHash = player.CustomProperties;

        _localPlayerHash[Constants.HASH_PLAYER_IS_CAPTAIN] = isCaptain;

        player.SetCustomProperties(_localPlayerHash);
    }

    public void ResetGame()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //wordsField.ResetGameField();
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                ExitGames.Client.Photon.Hashtable _myCustomProperties = player.CustomProperties;

                _myCustomProperties["WordPressed"] = Constants.PICKED_WORD_EMPTY;

                player.SetCustomProperties(_myCustomProperties);
            }
            ExitGames.Client.Photon.Hashtable _myCustomRoomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

            _myCustomRoomProperties["GamePhase"] = GamePhase.Setup;
            _myCustomRoomProperties["CurrentTeam"] = 1;
            _myCustomRoomProperties["IsPaused"] = true;

            PhotonNetwork.CurrentRoom.SetCustomProperties(_myCustomRoomProperties);
        }
    }

    //public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    //{
    //    if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_IS_SLOTS_OPEN))
    //    {
    //        if((bool)propertiesThatChanged[Constants.HASH_ROOM_IS_SLOTS_OPEN] == false)
    //        {
    //            SetupPlayersCountInTeams();
    //        }
    //        else
    //        {
    //            ResetWordPressed();
    //        }
    //    }
    //}

    ////TEST
    //[PunRPC]
    //private void RPC_Debug(string myString)
    //{
    //    Debug.Log(myString);
    //}


    public void OnClick_ResetGameField()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            wordsField.ResetGameField();
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                ExitGames.Client.Photon.Hashtable _myCustomProperties = player.CustomProperties;

                _myCustomProperties["WordPressed"] = Constants.PICKED_WORD_EMPTY;

                player.SetCustomProperties(_myCustomProperties);
            }
            ExitGames.Client.Photon.Hashtable _myCustomRoomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

            _myCustomRoomProperties["GamePhase"] = GamePhase.Setup;
            _myCustomRoomProperties["CurrentTeam"] = 1;
            _myCustomRoomProperties["IsPaused"] = true;

            PhotonNetwork.CurrentRoom.SetCustomProperties(_myCustomRoomProperties);
        }


    }

}
