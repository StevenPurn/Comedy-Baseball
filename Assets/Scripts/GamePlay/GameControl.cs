using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    //Singleton implementation
    public static GameControl instance;
    public static int numberOfInnings = 5;
    public static Inning curInning = new Inning();
    public static int strikes, balls, outs, outsThisPlay;
    public static bool playHitAudio = false;
    public static bool isPlayoffGame = false;
    public static bool ballInPlay = false;
    public static bool playIsActive = false;
    public static bool isHomeRun = false;
    public static bool waitingForNextBatter = false;
    public string teamFilePath = "/Data/Teams.xml";
    public string playerFilePath = "/Data/Players.xml";
    public GameObject runnerPrefab, fielderPrefab;
    public BetweenInnings betweenInningControl;
    public Transform battersBox, fieldParent;
    public CameraControl fieldCam;
    public List<Team> teams = new List<Team>();
    public List<Player> players = new List<Player>();
    public List<ActiveTeam> activeTeams = new List<ActiveTeam>();
    public List<ActivePlayer> activePlayers = new List<ActivePlayer>();
    private int runnerNumber;
    private int teamAtBat;
    public Material homeTeamMat, awayTeamMat;

    public delegate void ChangeCount();
    public ChangeCount changeCountEvent;

    //Load player & team data from xml files
    void Awake () {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        teamFilePath = Application.dataPath + teamFilePath;
        playerFilePath = Application.dataPath + playerFilePath;

        SaveLoad.PopulateTeamList();
        SaveLoad.PopulatePlayerList();
        curInning.inningNumber = 1;
    }

    public static void SetUpGame()
    {
        numberOfInnings = isPlayoffGame ? 5 : 3;
    }

    private void Update()
    {

        if(waitingForNextBatter && ballInPlay == false)
        {
            waitingForNextBatter = false;
            NextBatter();
        }

        if (playIsActive == false)
        {
            outsThisPlay = 0;
            isHomeRun = false;
        }
        if(SceneManager.GetActiveScene().name == "SteveField")
        {
            Field.FielderAI();
        }
    }

    //Save list of teams & players to respective xml files
    void Save()
    {
        SaveLoad.Save(activeTeams, teams);
    }

    //Add team to list of participating teams
    void AddActiveTeam(ActiveTeam team)    
    {
        activeTeams.Add(team);
    }

    void NextBatter()
    {
        ActiveTeam battingTeam = GetTeamAtBat();
        int curBatter = GetCurrentBatter();
        curBatter = curBatter + 1 == battingTeam.players.Count ? 0 : curBatter + 1;

        foreach (var player in battingTeam.players)
        {
            player.isAtBat = false;
        }

        foreach (var runner in Field.runners)
        {
            runner.atBat = false;
        }
        battingTeam.players[curBatter].isAtBat = true;
        AddBatterToField();
        changeCountEvent();
    }

    public static void InitializeField()
    {
        PlayFanfare();
        instance.teamAtBat = instance.activeTeams[0].currentlyAtBat ? 0 : 1;
        instance.AddBatterToField();
        Field.SpawnFielders();
    }

    public void AddBatterToField()
    {
        if (fieldParent == null)
        {
            fieldParent = GameObject.Find("Players").transform;
        }
        Vector3 pos = GetTeamAtBat().dugout.transform.position;
        GameObject go = Instantiate(runnerPrefab, pos, Quaternion.identity, fieldParent);
        Runner runner = go.GetComponentInChildren<Runner>();
        runner.team = GetTeamAtBat();
        runner.player = GetCurrentBattingPlayer();
        Field.currentBatter = runner;
        go.name = "Runner " + runnerNumber;
        runnerNumber += 1;
        Field.runners.Add(runner);
    }

    public void AddFielderToField(Fielder.Position pos, GameObject obj)
    {
        if (fieldParent == null)
        {
            fieldParent = GameObject.Find("Players").transform;
        }
        Vector3 loc = GetTeamInField().dugout.transform.position;
        GameObject go = Instantiate(fielderPrefab, loc, Quaternion.identity, fieldParent);
        go.name = pos.ToString();
        if (pos == Fielder.Position.pitcher)
        {
            go.transform.GetChild(0).gameObject.AddComponent<Pitcher>();
        }
        Fielder fielder = go.GetComponentInChildren<Fielder>();
        fielder.SetPosition(pos);
        Field.fielders.Add(fielder);
    }

    public void SetCameraToFollowBall(bool followBall)
    {
        if(fieldCam == null)
        {
            fieldCam = FindObjectOfType<CameraControl>();
        }
        if(followBall)
        {
            fieldCam.SetParent(Field.ball.transform);
        }
        else
        {
            fieldCam.SetParent();
        }
        fieldCam.followBall = followBall;
        fieldCam.ResetPosition();
    }

    int GetCurrentBatter()
    {
        if(GetTeamAtBat().players.Find(x => x.isAtBat == true) != null)
        {
            return GetTeamAtBat().players.FindIndex(x => x.isAtBat == true);
        }
        return 0;
    }

    public ActivePlayer GetCurrentBattingPlayer()
    {
        ActivePlayer playerAtBat = GetTeamAtBat().players.Find(x => x.isAtBat);
        if (playerAtBat == null)
        {
            return activeTeams[teamAtBat].players[0];
        }
        else
        {
            return playerAtBat;
        }
    }

    int GetCurrentPitcher()
    {
        int? ind = GetTeamInField().players.FindIndex(x => x.isPitching);
        if (ind != null)
        {
            return (int)ind;
        }
        return 0;
    }

    public ActivePlayer GetCurrentPitchingPlayer()
    {
        ActivePlayer pitcher = GetTeamInField().players.Find(x => x.isPitching);
        if (pitcher == null)
        {
            return GetTeamInField().players[0];
        }
        else
        {
            return pitcher;
        }
    }

    public ActiveTeam GetTeamAtBat()
    {
        return activeTeams[teamAtBat];
    }

    public ActiveTeam GetTeamInField()
    {
        int index = teamAtBat == 1 ? 0 : 1;
        return activeTeams[index];
    }

    void SwitchTeamAtBat()
    {
        activeTeams[0].currentlyAtBat = !activeTeams[0].currentlyAtBat;
        activeTeams[1].currentlyAtBat = !activeTeams[1].currentlyAtBat;
        teamAtBat = activeTeams[0].currentlyAtBat ? 0 : 1;
        changeCountEvent();
        runnerNumber = 0;
    }

#region HandleInput

    public void HandleStrike(bool wasFoul = false)
    {
        strikes += 1;
        if (strikes >= 3)
        {
            if (wasFoul)
            {
                strikes = 2;
            }
            else
            {
                if (outs < 2)
                { 
                    waitingForNextBatter = true;
                }
                Field.BatterIsOut();
                GetCurrentPitchingPlayer().ChangeStrikeoutsPitched(1);
                HandleOut();
                GetCurrentBattingPlayer().ChangeStrikeOutsAtBat(1);
            }
        }
        changeCountEvent();
    }

    public void HandleOut()
    {
        outs += 1;
        outsThisPlay += 1;
        ResetCount();
        if (outs >= 3)
        {
            ResetInning();
        }
        changeCountEvent();
    }

    public void HandleBall()
    {
        balls += 1;
        if (balls >= 4)
        {
            balls = 4;
            //Walk batter but we don't pitch any balls so...
        }
        changeCountEvent();
    }

    public void FastForward(bool isSpedUp = true)
    {
        if (isSpedUp)
        {
            Time.timeScale = 2f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void HandlePitch(int hitQuality)
    {
        Field.ball.curPitch = Pitches.pitches[hitQuality];
        Field.ball.DeterminePitchResults();
        Field.fielders.Find(x => x.position == Fielder.Position.pitcher).GetComponent<Pitcher>().ThrowPitch();
        playIsActive = true;
    }

    public void ChangeTeamScore(int change)
    {
        int curBatter = GetCurrentBatter() - 1 < 0 ? activeTeams[teamAtBat].players.Count - 1 : GetCurrentBatter() - 1;
        activeTeams[teamAtBat].players[curBatter].ChangeRBIs(change);
        activeTeams[teamAtBat].ChangeScore(change, curInning.inningNumber);
        changeCountEvent();
    }

    #endregion

    public void ResetCount()
    {
        balls = 0;
        strikes = 0;
        changeCountEvent();
    } 

    void ResetInning()
    {
        ballInPlay = false;
        waitingForNextBatter = false;
        Field.ball.transform.SetParent(GameObject.Find("Field").transform);
        if (curInning.isBottom)
        {
            if (curInning.inningNumber >= numberOfInnings)
            {
                GameOver();
            }
            curInning.inningNumber += 1;
            curInning.isBottom = false;
            curInning.pitchesThrownThisInning = 0;
            if(betweenInningControl == null)
            {
                betweenInningControl = FindObjectOfType<BetweenInnings>();
            }
            PlayFanfare();
            betweenInningControl.EnableBetweenInningsUI();
        }
        else
        {
            if (curInning.inningNumber >= numberOfInnings)
            {
                if (GetTeamAtBat().score < GetTeamInField().score)
                {
                    GameOver();
                }
            }
            SwitchTeamAtBat();
            curInning.isBottom = true;
        }
        if(curInning.inningNumber == numberOfInnings && curInning.isBottom == false)
        {
            if (GetTeamAtBat().score < GetTeamInField().score)
            {
                SwitchTeamAtBat();
            }
        }
        Field.ResetInning();
        outs = 0;
        AddBatterToField();
        SetCameraToFollowBall(false);
    }

    static void PlayFanfare()
    {
        string aud = "fanfare" + Random.Range(1, 12);
        AudioControl.instance.PlayAudio(aud);
    }

    void GameOver()
    {
        if(activeTeams[0].score == activeTeams[1].score)
        {
            activeTeams[0].tiedGame = true;
            activeTeams[1].tiedGame = true;
        }
        else
        {
            int winner = activeTeams[0].score > activeTeams[1].score ? 0 : 1;
            int loser = winner == 0 ? 1 : 0;
            activeTeams[winner].wonGame = true;
            activeTeams[loser].lostGame = true;
        }

        Save();
        Field.runners.Clear();
        Field.fielders.Clear();
        SceneManager.LoadScene("GameOver");
    }
}
