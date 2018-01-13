using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Runner : MonoBehaviour {

    public bool atBat = true;
    public int currentBase = 0;
    public bool isOut;
    public bool exitingField = false;
    public bool isAdvancing;
    public List<GameObject> targetBase = new List<GameObject>();
    private Rigidbody2D rb;
    private float movementSpeed = 10f;
    public ActiveTeam team;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        atBat = true;
    }

    //Move player towards next base if they have one
    //Use a list of bases to prevent them from moving directly to the final destination
    private void FixedUpdate()
    {
        //If we still have another base to move towards
        if (targetBase.Count > 0)
        {
            Vector3 movementTarget = new Vector3(0, 0, 0);
            movementTarget = targetBase[0].transform.position;
            if (Utility.CheckEqual(movementTarget, transform.position, 0.1f))
            {
                isAdvancing = false;
                if (exitingField)
                {
                    targetBase.Clear();
                    Destroy(gameObject);
                }
                else if (currentBase == 3)
                {
                    exitingField = true;
                    GameControl.instance.ChangeTeamScore(1);
                    RemoveRunner();
                }
                else
                {
                    if (targetBase[0].name.Contains("Base"))
                    {
                        currentBase += 1;
                    }
                    targetBase.Remove(targetBase[0]);
                }
            }
            else
            {
                isAdvancing = true;
                MovePlayer(movementTarget);
            }
        }
        else if (targetBase.Count == 0)
        {
            //Totally necessary
            targetBase.Clear();
            isAdvancing = false;
        }
        if (currentBase >= 4)
        {
            Debug.LogWarning("Someone went way too far. Like, good on them. But you should check it");
        }
    }

    void MovePlayer(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
    }

    //Get target bases to move towards from Field class
    public void SetBaseAsTarget(List<GameObject> baseToTarget)
    {
        foreach (var targetBase in baseToTarget)
        {
            this.targetBase.Add(targetBase);
        }
    }

    public void SetBaseAsTarget(GameObject baseToTarget)
    {
        targetBase.Add(baseToTarget);
    }

    public void SetOut()
    {
        isOut = true;
        RemoveRunner();
    }

    public void RemoveRunner()
    {
        exitingField = true;
        targetBase.Clear();
        SetBaseAsTarget(team.dugout);
        currentBase = 0;
    }
}
