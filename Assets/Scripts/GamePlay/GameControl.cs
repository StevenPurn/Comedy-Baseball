using System;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    //Singleton implementation
    public static GameControl instance;
    //Static variables
    public static int numberOfInnings = 3;
    public static Inning curInning = new Inning();
    public static int strikes, balls, outs;
    //Save location for team & player files
    public string teamFilePath = "/Data/Teams.xml";
    public string playerFilePath = "/Data/Players.xml";
    public GameObject runnerPrefab, fielderPrefab;
    public Transform battersBox, fieldParent;
    //Teams & players that exist in the xml files
    public List<Team> teams = new List<Team>();
    public List<Player> players = new List<Player>();
    //Teams & players participating in this game
    public List<ActiveTeam> activeTeams = new List<ActiveTeam>();
    public List<ActivePlayer> activePlayers = new List<ActivePlayer>();
    private int runnerNumber;
    private int teamAtBat;
    public static bool ballInPlay = false;
    public static bool waitingForNextBatter = false;
    public Material homeTeamMat, awayTeamMat;
    public Dictionary<TeamColor, Color> teamColors = new Dictionary<TeamColor, Color>()
    {
        {TeamColor.orange, Color.green }, //This makes no sense and will need to be changed to orange at some point
        {TeamColor.blue, Color.blue }
    };

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
        if(waitingForNextBatter && !ballInPlay)
        {
            waitingForNextBatter = false;
            NextBatter();
        }

        Field.FielderAI();
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
        //ballInPlay = false;

        foreach (var player in battingTeam.players)
        {
            player.isAtBat = false;
        }

        foreach (var runner in Field.runners)
        {
            runner.atBat = false;
        }
        battingTeam.players[curBatter].isAtBat = true;
        battingTeam.players[curBatter].atBats += 1;
        AddBatterToField();
    }

    public static void InitializeField()
    {
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
        go.name = "Runner " + runnerNumber;
        runnerNumber += 1;
        Field.runners.Add(go.GetComponentInChildren<Runner>());
    }

    private Pitch EvaluatePitch(int hitQuality)
    {
        Pitch pitch = new Pitch();
        switch (hitQuality)
        {
            case 1:
                //Pop fly/ground out
                pitch.type = Pitcher.Pitches.popfly;
                pitch.maxHeight = 6.0f;
                pitch.hitSpeed = 3.0f;
                pitch.hitAngles.Add(new Vector2(1, 0));
                break;
            case 2:
                //fly out/ground out or foul
                pitch.type = Pitcher.Pitches.hit;
                pitch.maxHeight = 6.0f;
                pitch.hitSpeed = 3f;
                pitch.hitAngles.Add(new Vector2(0, 1));
                break;
            case 3:
                //strike or foul
                pitch.type = Pitcher.Pitches.hit;
                pitch.maxHeight = 6.0f;
                pitch.hitSpeed = 3f;
                pitch.hitAngles.Add(new Vector2(-1, 0));
                break;
            case 4:
                //single
                pitch.type = Pitcher.Pitches.hit;
                pitch.maxHeight = 6.0f;
                pitch.hitSpeed = 3f;
                pitch.hitAngles.Add(new Vector2(0, -1));
                break;
            case 5:
                //single with better on base chance + speed boost
                pitch.type = Pitcher.Pitches.hit;
                pitch.maxHeight = 6.0f;
                pitch.hitSpeed = 0.2f;
                pitch.hitAngles.Add(new Vector3(0, 0, 1));
                break;
            case 6:
                //shallow outfield at best
                pitch.type = Pitcher.Pitches.hit;
                pitch.maxHeight = 6.0f;
                pitch.hitSpeed = 0.2f;
                pitch.hitAngles.Add(new Vector3(8, 8, 1));
                break;
            case 7:
                //shallow outfield
                pitch.type = Pitcher.Pitches.hit;
                pitch.maxHeight = 6.0f;
                pitch.hitSpeed = 0.2f;
                pitch.hitAngles.Add(new Vector3(0, 0, 1));
                break;
            case 8:
                //past infield
                pitch.type = Pitcher.Pitches.hit;
                pitch.maxHeight = 6.0f;
                pitch.hitSpeed = 0.2f;
                pitch.hitAngles.Add(new Vector3(0, 0, 1));
                break;
            case 9:
                //Homerun + error on next play less likely than 10
                pitch.type = Pitcher.Pitches.hit;
                pitch.maxHeight = 6.0f;
                pitch.hitSpeed = 0.2f;
                pitch.hitAngles.Add(new Vector3(0, 0, 1));
                break;
            case 10:
                //Homerun + error on next play
                pitch.type = Pitcher.Pitches.hit;
                pitch.maxHeight = 6.0f;
                pitch.hitSpeed = 0.2f;
                pitch.hitAngles.Add(new Vector3(0, 0, 1));
                break;
            default:
                Debug.LogWarning("Hit quality wasn't generated correctly");
                break;
        }

        return pitch;
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
        GetCurrentPitchingPlayer().ChangePitches(1);
        Field.fielders.Find(x => x.position == Fielder.Position.pitcher).GetComponent<Pitcher>().ThrowPitch(Pitcher.Pitches.strike);
        strikes += 1;
        if (strikes >= 3)
        {
            if (wasFoul)
            {
                strikes = 2;
            }
            else
            {
                Field.BatterIsOut();
                GetCurrentPitchingPlayer().ChangeStrikeoutsPitched(1);
                HandleOut();
                GetCurrentBattingPlayer().ChangeStrikeOutsAtBat(1);
            }
        }
        changeCountEvent();
    }

    //Need to add potential for advancing runners on base during an out
    public void HandleOut()
    {
        outs += 1;
        ResetCount();
        waitingForNextBatter = true;
        if (outs >= 3)
        {
            ResetInning();
        }
        changeCountEvent();
    }

    public void HandleBall()
    {
        GetCurrentPitchingPlayer().ChangePitches(1);
        balls += 1;
        if (balls >= 4)
        {
            balls = 4;
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
        Pitch curPitch = EvaluatePitch(hitQuality);
        Field.ball.curPitch = curPitch;
        Field.fielders.Find(x => x.position == Fielder.Position.pitcher).GetComponent<Pitcher>().ThrowPitch(curPitch.type);
    }

    public void ChangeTeamScore(int change)
    {
        int curBatter = GetCurrentBatter() - 1 < 0 ? activeTeams[teamAtBat].players.Count - 1 : GetCurrentBatter() - 1;
        activeTeams[teamAtBat].players[curBatter].ChangeRBIs(change);
        activeTeams[teamAtBat].score += change;
        changeCountEvent();
    }

    #endregion

    void ResetCount()
    {
        balls = 0;
        strikes = 0;
        changeCountEvent();
    } 

    void ResetInning()
    {
        if (curInning.isBottom)
        {
            if(curInning.inningNumber >= numberOfInnings)
            {
                GameOver();
            }
            curInning.inningNumber += 1;
            curInning.isBottom = false;
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
    }

    void GameOver()
    {
        ActiveTeam winner = activeTeams[0].score > activeTeams[1].score ? activeTeams[0] : activeTeams[1];
        winner.wonGame = true;
        Save();
    }
}
