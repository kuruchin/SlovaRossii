using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public const int MAX_PLAYER_IN_ROOM = 20;

    public const bool IS_TEST_MODE = false;

    public const int PICKED_WORD_EMPTY = -1;
    public const int PICKED_WORD_SKIP_TURN = -2;

    public const int WORD_COLOR_WHITE = 5;
    public const int WORD_COLOR_BLACK = 6;


    public const string HASH_PLAYER_IS_CAPTAIN      = "Captain";
    public const string HASH_PLAYER_TEAM_NUMBER     = "TeamNumber";
    public const string HASH_PLAYER_ID_NUMBER       = "IDNumber";
    public const string HASH_PLAYER_WORD_PRESSED    = "WordPressed";

    public const string HASH_ROOM_GAME_PHASE        = "GamePhase";
    public const string HASH_ROOM_CURRENT_TEAM      = "CurrentTeam";
    public const string HASH_ROOM_IS_PAUSED         = "IsPaused";
    public const string HASH_ROOM_FIRST_TEAM        = "FirstTeam";
    public const string HASH_ROOM_SECOND_TEAM       = "SecondTeam";
    public const string HASH_ROOM_FIRST_TEAM_WORDS  = "FirstTeamWords";
    public const string HASH_ROOM_SECOND_TEAM_WORDS = "SecondTeamWords";
    public const string HASH_ROOM_MAX_WORDS         = "MaxWords";
    public const string HASH_ROOM_START_TIME        = "StartTime";
    public const string HASH_ROOM_PAUSE_TIME        = "PauseTime";
    public const string HASH_ROOM_IS_SLOTS_OPEN     = "IsSlotsOpen";
}
