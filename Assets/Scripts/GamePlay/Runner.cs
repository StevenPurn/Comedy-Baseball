using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Runner : MonoBehaviour {

    public bool atBat;
    public int currentBase = 0;
    public bool isOut;
    public bool hasScored = false;
    public bool isAdvancing;
    public List<GameObject> targetBase = new List<GameObject>();
    private Rigidbody2D rb;
    private float movementSpeed = 10f;
    public ActiveTeam team;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Move player towards next base if they have one
    //Use a list of bases to prevent them from moving directly to the final destination
    private void FixedUpdate()
    {
        //If we still have another base to move towards
        if(targetBase.Count > 0)
        {
            isAdvancing = true;
            //Move towards the next base
            Vector3 direction = (targetBase[0].transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
            //Clean up the state of your previous base when you leave
            //If you've reached your next destination
            if (CheckEqual(rb.position, new Vector2(targetBase[0].transform.position.x, targetBase[0].transform.position.y),0.1f))
            {
                //If you reached home plate score and then kindly remove yourself from the game
                if (currentBase == 3)
                {
                    hasScored = true;
                    GameControl.instance.ChangeTeamScore(1);
                    RemoveRunner();
                }
                else
                {
                    //Inform the runner of which base they are on
                    for (int i = Field.bases.Length - 1; i > 0; i--)
                    {
                        if (targetBase[0] == Field.bases[i].baseObj)
                        {
                            currentBase = i;
                            //Make sure the field is also aware of which base the runner is on
                            Field.bases[currentBase].isOccupied = true;
                            if (Field.bases[currentBase - 1] != null)
                            {
                                if (Field.bases[currentBase - 1].isOccupied)
                                {
                                    Field.bases[currentBase - 1].isOccupied = false;
                                }
                            }
                        }
                    }
                }
                //Remove the target as we've reached the base
                targetBase.Remove(targetBase[0]);
                //If there are no more targets remaining ensure everything is cleared out
                if (targetBase.Count == 0)
                {
                    //Totally necessary
                    targetBase.Clear();
                    isAdvancing = false;
                }

                if (hasScored)
                {

                }
            }
        }

        if(currentBase >= 4)
        {
            Debug.LogWarning("Someone went way too far. Like, good on them. But you should check it");
        }
    }

    //Approximation of position equality to prevent weird rubber banding issues
    bool CheckEqual(Vector2 v1, Vector2 v2, float tolerance)
    {
        if (Mathf.Abs(v1.x - v2.x) < tolerance && Mathf.Abs(v1.y - v2.y) < tolerance)
        {
            return true;
        }
        else return false;
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
        this.targetBase.Add(baseToTarget);
    }

    public void SetOut()
    {
        isOut = true;
        RemoveRunner();
    }

    public void RemoveRunner()
    {
        targetBase.Clear();
        currentBase = 0;
        SetBaseAsTarget(team.dugout);
        //Destroy(gameObject);
    }
}
