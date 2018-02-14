using UnityEngine;
using System.Collections.Generic;

public class SetUpField : MonoBehaviour {

    public Base[] bases = new Base[4];
    public GameObject[] dugouts = new GameObject[2];
    public Fielder.Position[] positions = new Fielder.Position[9];
    public List<GameObject> runnerTargets = new List<GameObject>();
    public GameObject[] positionObjs = new GameObject[9];
    public GameObject[] playPosObjs = new GameObject[9];
    public Dictionary<Fielder.Position, GameObject> fieldPos = new Dictionary<Fielder.Position, GameObject> { };
    public Dictionary<Fielder.Position, GameObject> playPos = new Dictionary<Fielder.Position, GameObject> { };

    // Use this for initialization
    void Awake ()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            fieldPos.Add(positions[i], positionObjs[i]);
        }

        for (int i = 0; i < positions.Length; i++)
        {
            playPos.Add(positions[i], playPosObjs[i]);
        }

        Field.bases = bases;
        Field.dugouts = dugouts;
        Field.AssignDugouts();
        Field.fieldPositions = fieldPos;
        Field.playPositions = playPos;
        Field.runnerTargets = runnerTargets;
        Field.ball = GameObject.Find("Ball").GetComponent<Ball>();
        GameControl.InitializeField();
    }
}