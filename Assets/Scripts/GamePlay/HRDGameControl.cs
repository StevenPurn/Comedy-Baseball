using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HRDGameControl : MonoBehaviour {

    //Singleton implementation
    public static HRDGameControl instance;
    public static int numberOfOuts = 10;
    public static bool playHitAudio = false;
    public static bool ballInPlay = false;
    public static bool playIsActive = false;
    public static bool isHomeRun = false;
    public static bool waitingForNextBatter = false;
    public static List<HRDPlayer> players = new List<HRDPlayer>();
    public GameObject batterPrefab, fielderPrefab;
    public BetweenInnings betweenInningControl;
    public Transform battersBox, fieldParent;
    public CameraControl fieldCam;
    public Material fielderMat, batterMat;

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
    }

    private void Update()
    {

 
    }

    void NextBatter()
    {
        AddBatterToField();
        changeCountEvent();
    }

    public static void InitializeField()
    {
        PlayFanfare();
        instance.AddBatterToField();
        Field.SpawnFielders();
    }

    public static void SetPlayers(string[] names)
    {
        foreach (string name in names)
        {
            players.Add(new HRDPlayer(name));
        }
    }

    public void AddBatterToField()
    {
        if (fieldParent == null)
        {
            fieldParent = GameObject.Find("Players").transform;
        }
        Vector2 pos = Field.dugouts[1].transform.position;
        GameObject go = Instantiate(batterPrefab, pos, Quaternion.identity, fieldParent);
        HRDBatter runner = go.GetComponentInChildren<HRDBatter>();
        Field.hrdCurrentBatter = runner;
        Field.hrdRunners.Add(runner);
    }

    public void AddFielderToField(Fielder.Position pos, GameObject obj)
    {
        if (fieldParent == null)
        {
            fieldParent = GameObject.Find("Players").transform;
        }
        Vector3 loc = Field.dugouts[0].transform.position;
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

#region HandleInput

    public void HandleOut()
    {
        ResetCount();
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

    public void ChangePlayerScore(int change)
    {
        changeCountEvent();
    }

    #endregion

    public void ResetCount()
    {
        changeCountEvent();
    }

    static void PlayFanfare()
    {
        string aud = "fanfare" + Random.Range(1, 12);
        AudioControl.instance.PlayAudio(aud);
    }

    void GameOver()
    {
        // Get top 3 players?
        Field.runners.Clear();
        Field.fielders.Clear();
        SceneManager.LoadScene("HRDGameOver");
    }
}
