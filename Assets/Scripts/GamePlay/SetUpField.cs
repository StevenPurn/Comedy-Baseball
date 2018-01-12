using UnityEngine;
using System.Collections.Generic;

public class SetUpField : MonoBehaviour {

    public Base[] bases = new Base[4];
    public GameObject[] dugouts = new GameObject[2];
    public Fielder.Position[] positions = new Fielder.Position[9];
    public GameObject[] positionObjs = new GameObject[9];
    public Dictionary<Fielder.Position, GameObject> fieldPos = new Dictionary<Fielder.Position, GameObject> { };

    // Use this for initialization4
    void Awake ()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            fieldPos.Add(positions[i], positionObjs[i]);
        }
        Field.bases = bases;
        Field.dugouts = dugouts;
        Field.AssignDugouts();
        Field.fieldPositions = fieldPos;
        GameControl.InitializeField();
    }
}