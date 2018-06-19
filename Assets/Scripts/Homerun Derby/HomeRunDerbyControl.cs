using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeRunDerbyControl : MonoBehaviour
{

    //Singleton implementation
    public static HomeRunDerbyControl instance;
    public static int numberOfInnings = 3;
    public static bool playIsActive = false;
    public static bool isHomeRun = false;
    public static bool waitingForNextBatter = false;
    public static int outs = 0;
    public GameObject runnerPrefab, fielderPrefab;
    public Transform battersBox, fieldParent;
    public CameraControl fieldCam;
    public List<ActivePlayer> activePlayers = new List<ActivePlayer>();
    private int runnerNumber;
    private int teamAtBat;
    public Material homeTeamMat, awayTeamMat;

    public delegate void ChangeCount();
    public ChangeCount changeCountEvent;

    //Load player & team data from xml files
    void Awake()
    {
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
        
    }

    public static void InitializeField()
    {
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

    public void AddFielderToField(GameObject obj)
    {
        if (fieldParent == null)
        {
            fieldParent = GameObject.Find("Players").transform;
        }
        GameObject go = Instantiate(fielderPrefab, GetTeamInField().dugout.transform.position, Quaternion.identity, fieldParent);
        go.name = "Pitcher";
        go.transform.GetChild(0).gameObject.AddComponent<Pitcher>();
        go.GetComponentInChildren<Fielder>().SetPosition(Fielder.Position.pitcher);
        Field.fielders.Add(go.GetComponentInChildren<Fielder>());
    }

    public void SetCameraToFollowBall(bool followBall)
    {
        if (fieldCam == null)
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
        if (GetTeamAtBat().players.Find(x => x.isAtBat == true) != null)
        {
            return GetTeamAtBat().players.FindIndex(x => x.isAtBat == true);
        }
        //By default return 0, should only occur with first batter of the game
        return 0;
    }

    #region HandleInput

    public void HandleOut()
    {
        outs += 1;
        if (outs >= 10)
        {
            Debug.Log("Switch to next batter");
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

    public void ChagePlayerScore(int change)
    {
        GetCurrentBatter()
        changeCountEvent();
    }

    #endregion

    public void ResetOuts()
    {
        outs = 0;
        changeCountEvent();
    }

    void GameOver()
    {
        Field.runners.Clear();
        Field.fielders.Clear();
        SceneManager.LoadScene("GameOver");
    }
}
