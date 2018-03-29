using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    //Singleton implementation
    public static GameControl instance;
    public static int numberOfInnings = 3;
    public static Inning curInning = new Inning();
    public static int strikes, balls, outs, outsThisPlay;
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
        //AudioControl.instance.PlayAudio("playball");
        instance.teamAtBat = instance.activeTeams[0].currentlyAtBat ? 0 : 1;
        instance.AddBatterToField();
        Field.SpawnFielders();
    }

    public void AddBatterToField()
    {
        //This needs to wait for the previous play to be over before being called
        if (fieldParent == null)
        {
            fieldParent = GameObject.Find("Players").transform;
        }
        GameObject go = Instantiate(runnerPrefab, GetTeamAtBat().dugout.transform.position, Quaternion.identity, fieldParent);
        go.GetComponentInChildren<Runner>().team = GetTeamAtBat();
        go.GetComponentInChildren<Runner>().player = GetCurrentBattingPlayer();
        go.name = "Runner " + runnerNumber;
        runnerNumber += 1;
        Field.runners.Add(go.GetComponentInChildren<Runner>());
    }

    public void AddFielderToField(Fielder.Position pos, GameObject obj)
    {
        if (fieldParent == null)
        {
            fieldParent = GameObject.Find("Players").transform;
        }
        GameObject go = Instantiate(fielderPrefab, GetTeamInField().dugout.transform.position, Quaternion.identity, fieldParent);
        go.name = pos.ToString();
        if (pos == Fielder.Position.pitcher)
        {
            go.transform.GetChild(0).gameObject.AddComponent<Pitcher>();
        }
        go.GetComponentInChildren<Fielder>().SetPosition(pos);
        Field.fielders.Add(go.GetComponentInChildren<Fielder>());
    }

    public void SetCameraToFollowBall(bool followBall)
    {
        if(fieldCam == null)
        {
            fieldCam = FindObjectOfType<CameraControl>();
        }
        if (followBall)
        { 
            fieldCam.followBall = true;
            fieldCam.SetParent(Field.ball.transform);
            fieldCam.ResetPosition();
        }
        else
        {
            fieldCam.followBall = false;
            fieldCam.SetParent();
            fieldCam.ResetPosition();
        }
    }

    int GetCurrentBatter()
    {
        if(GetTeamAtBat().players.Find(x => x.isAtBat == true) != null)
        {
            return GetTeamAtBat().players.FindIndex(x => x.isAtBat == true);
        }
        //By default return 0, should only occur with first batter of the game
        return 0;
    }

    public ActivePlayer GetCurrentBattingPlayer()
    {
        if (activeTeams[teamAtBat].players.Find(x => x.isAtBat == true) == null)
        {
            return activeTeams[teamAtBat].players[0];
        }
        else
        {
            return activeTeams[teamAtBat].players.Find(x => x.isAtBat == true);
        }
    }

    int GetCurrentPitcher()
    {
        if (GetTeamInField().players.Find(x => x.isPitching == true) != null)
        {
            return GetTeamInField().players.FindIndex(x => x.isPitching == true);
        }
        //By default return 0, should only occur with first batter of the game
        return 0;
    }

    public ActivePlayer GetCurrentPitchingPlayer()
    {
        int teamPitching = teamAtBat == 0 ? 1 : 0;
        if (activeTeams[teamPitching].players.Find(x => x.isPitching == true) == null)
        {
            return activeTeams[teamPitching].players[0];
        }
        else
        {
            return activeTeams[teamAtBat].players.Find(x => x.isPitching == true);
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

        if (activeTeams[0].currentlyAtBat && activeTeams[1].currentlyAtBat)
        {
            Debug.LogWarning("Both teams are batting");
        }else if(!activeTeams[0].currentlyAtBat && !activeTeams[1].currentlyAtBat)
        {
            Debug.LogWarning("Neither team is batting");
        }
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
            //Walk batter
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
            betweenInningControl.EnableBetweenInningsUI();
        }
        else
        {
            if (curInning.inningNumber >= numberOfInnings)
            {
                int teamNotBatting = teamAtBat == 0 ? 1 : 0;
                if (activeTeams[teamAtBat].score < activeTeams[teamNotBatting].score)
                {
                    GameOver();
                }
            }
            curInning.isBottom = true;
        }

        SwitchTeamAtBat();
        Field.ResetInning();
        outs = 0;
        AddBatterToField();
        SetCameraToFollowBall(false);
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
            if(activeTeams[0].score > activeTeams[1].score)
            {
                activeTeams[0].wonGame = true;
                activeTeams[1].lostGame = true;
            }
            else
            {
                activeTeams[1].wonGame = true;
                activeTeams[0].lostGame = true;
            }
        }

        Save();
        Field.runners.Clear();
        Field.fielders.Clear();
        SceneManager.LoadScene("GameOver");
    }
}
