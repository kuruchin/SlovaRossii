// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountdownTimer.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities,
// </copyright>
// <summary>
// This is a basic CountdownTimer. In order to start the timer, the MasterClient can add a certain entry to the Custom Room Properties,
// which contains the property's name 'StartTime' and the actual start time describing the moment, the timer has been started.
// To have a synchronized timer, the best practice is to use PhotonNetwork.Time.
// In order to subscribe to the CountdownTimerHasExpired event you can call CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
// from Unity's OnEnable function for example. For unsubscribing simply call CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;.
// You can do this from Unity's OnDisable function for example.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.UtilityScripts
{
    /// <summary>This is a basic, network-synced CountdownTimer based on properties.</summary>
    /// <remarks>
    /// In order to start the timer, the MasterClient can call SetStartTime() to set the timestamp for the start.
    /// The property 'StartTime' then contains the server timestamp when the timer has been started.
    /// 
    /// In order to subscribe to the CountdownTimerHasExpired event you can call CountdownTimer.OnCountdownTimerHasExpired
    /// += OnCountdownTimerIsExpired;
    /// from Unity's OnEnable function for example. For unsubscribing simply call CountdownTimer.OnCountdownTimerHasExpired
    /// -= OnCountdownTimerIsExpired;.
    /// 
    /// You can do this from Unity's OnEnable and OnDisable functions.
    /// </remarks>
    public class TestTimer : MonoBehaviourPunCallbacks
    {
        /// <summary>
        ///     OnCountdownTimerHasExpired delegate.
        /// </summary>
        public delegate void CountdownTimerHasExpired();

        public const string CountdownStartTime = "StartTime";

        [Header("Countdown time in seconds")]
        public float Countdown = 60.0f;

        private bool isTimerRunning;

        private bool isGameNotStarted = true; //isGameEnded

        private int startTime;
        private int pausetTime = 0;

        [SerializeField]
        private int testTimer;

        [Header("Reference to a Text component for visualizing the countdown")]
        public Text Text;

        [SerializeField]
        private MainManager mainManager;

        /// <summary>
        ///     Called when the timer has expired.
        /// </summary>
        public static event CountdownTimerHasExpired OnCountdownTimerHasExpired;


        public void Start()
        {
            if (this.Text == null) Debug.LogError("Reference to 'Text' is not set. Please set a valid reference.", this);
        }

        public override void OnEnable()
        {
            Debug.Log("OnEnable CountdownTimer");
            base.OnEnable();

            // the starttime may already be in the props. look it up.
            Initialize();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Debug.Log("OnDisable CountdownTimer");

        }

        public void Update()
        {
            if (!this.isTimerRunning) return;

            float countdown = TimeRemaining();
            this.Text.text = string.Format("Таймер хода {0}", countdown.ToString("n0"));

            //if (countdown > 0.0f) return;

            if(isGameNotStarted || countdown < 0.0f)
                OnTimerEnds();
        }

        private void OnTimerRuns()
        {
            this.isTimerRunning = true;
            this.enabled = true;
        }

        private void OnTimerEnds()
        {
            this.isTimerRunning = false;
            //this.enabled = false;

            Debug.Log("Emptying info text.", this.Text);
            this.Text.text = string.Empty;

            //OnCountdownTimerHasExpired = mainManager.TestRun;
            OnCountdownTimerHasExpired = mainManager.ResetTimer;
            if (OnCountdownTimerHasExpired != null && isGameNotStarted == false)
            {
                Debug.Log("OnCountdownTimerHasExpired new run");
                OnCountdownTimerHasExpired();
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_IS_PAUSED) && !isGameNotStarted)
            {
                if ((bool)propertiesThatChanged[Constants.HASH_ROOM_IS_PAUSED] == true)
                {
                    Debug.Log("------------PAUSE-------------");
                    isTimerRunning = false;
                    pausetTime = PhotonNetwork.ServerTimestamp;
                    float countdown = TimeRemaining();
                    this.Text.text = string.Format("Таймер хода {0} [Пауза]", countdown.ToString("n0"));
                }
                else
                {
                    Debug.Log("------------UNPAUSE-------------");
                    ResumeTimer();
                    isTimerRunning = true;
                }
            }

            if (propertiesThatChanged.ContainsKey(Constants.HASH_ROOM_GAME_PHASE))
            {
                if ((MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE] == MainManager.GamePhase.Ending ||
                    (MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE] == MainManager.GamePhase.Setup)
                {
                    isGameNotStarted = true;
                    OnTimerEnds();

                    if (!PhotonNetwork.LocalPlayer.IsMasterClient)
                        return;

                    // Delete timer from hashtable
                    PhotonNetwork.CurrentRoom.CustomProperties.Remove(Constants.HASH_ROOM_START_TIME);

                }
                else if((MainManager.GamePhase)propertiesThatChanged[Constants.HASH_ROOM_GAME_PHASE] == MainManager.GamePhase.Opening)
                {
                    isGameNotStarted = false;
                }
            }

            if (propertiesThatChanged.ContainsKey(CountdownStartTime))
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            int propStartTime;
            if (TryGetStartTime(out propStartTime))
            {
                this.startTime = propStartTime;
                Debug.Log("Initialize sets StartTime " + this.startTime + " server time now: " + PhotonNetwork.ServerTimestamp + " remain: " + TimeRemaining());


                this.isTimerRunning = TimeRemaining() > 0;

                if (this.isTimerRunning)
                    OnTimerRuns();
                else
                    OnTimerEnds();
            }
            else
            {
                Debug.Log("Initialize: False");
            }
        }

        private float TimeRemaining()
        {
            int timer = PhotonNetwork.ServerTimestamp - startTime;//this.startTime;
            return this.Countdown - timer / 1000f;
        }

        public static bool TryGetStartTime(out int startTimestamp)
        {
            startTimestamp = PhotonNetwork.ServerTimestamp;

            object startTimeFromProps;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CountdownStartTime, out startTimeFromProps))
            {
                startTimestamp = (int)startTimeFromProps;
                Debug.Log("TryGetStartTime current time: " + startTimestamp);
                return true;
            }
            Debug.Log("TryGetStartTime: False");
            return false;
        }

        public static void SetStartTime()
        {
            int startTime = 0;
            bool wasSet = TryGetStartTime(out startTime);

            Hashtable props = new Hashtable
            {
                {TestTimer.CountdownStartTime, (int)PhotonNetwork.ServerTimestamp}
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);

            Debug.Log("Set Custom Props for Time: " + props.ToStringFull() + " wasSet: " + wasSet);
        }

        public void ResumeTimer()
        {
            startTime = startTime + (PhotonNetwork.ServerTimestamp - pausetTime);
        }

        public void AddTimeToTimer(int time)
        {
            int _startTime = 0;
            TryGetStartTime(out _startTime);

            Hashtable props = new Hashtable
            {
                {TestTimer.CountdownStartTime, _startTime + time * 1000}
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }

        public void RunTimer()
        {
            //Countdown = timer;
            SetStartTime();
        }
    }
}