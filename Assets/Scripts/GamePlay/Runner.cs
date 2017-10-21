using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Runner : MonoBehaviour {

    public bool atBat;
    public int currentBase = 0;
    public bool isOut;
    private List<GameObject> target = new List<GameObject>();
    private Rigidbody2D rb;
    private float movementSpeed = 20f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //Move player towards next base if they have one
    //Use a list of bases to prevent them from moving directly to the final destination
    private void FixedUpdate()
    {
        //If we still have another base to move towards
        if(target.Count > 0)
        {
            //Move towards the next base
            Vector3 direction = (target[0].transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
            //Clean up the state of your previous base when you leave
            if (Field.bases[currentBase].isOccupied)
            {
                Field.bases[currentBase].isOccupied = false;
            }
            //If you've reached your next destination
            if (CheckEqual(rb.position, new Vector2(target[0].transform.position.x, target[0].transform.position.y),0.1f))
            {
                //If you reached home plate score and then kindly remove yourself from the game
                if(currentBase == 3)
                {
                    GameControl.instance.ChangeTeamScore(1);
                    RemoveRunner();
                }
                else
                {
                    //Inform the runner of which base they are on
                    for (int i = Field.bases.Length - 1; i > 0; i--)
                    {
                        if(target[0] == Field.bases[i].baseObj)
                        {
                            currentBase = i;
                        }
                    }
                    //Make sure the field is also aware of which base the runner is on
                    Field.bases[currentBase].isOccupied = true;
                }
                //Remove the target as we've reached the base
                target.Remove(target[0]);
                //If there are no more targets remaining ensure everything is cleared out
                if (target.Count == 0)
                {
                    //Totally necessary
                    target.Clear();
                }
            }
        }

        if(currentBase >= 4)
        {
            Debug.LogWarning("Some went way too far. Like, good on them. But you should check it");
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
            target.Add(targetBase);
        }
    }

    public void SetOut()
    {
        isOut = true;
        RemoveRunner();
    }

    public void RemoveRunner()
    {
        Field.runners.Remove(this);
        Destroy(gameObject);
    }
}
