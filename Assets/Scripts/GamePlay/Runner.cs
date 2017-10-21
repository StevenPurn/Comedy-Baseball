using System.Collections;
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
    private void FixedUpdate()
    {
        if(target.Count > 0)
        {
            Vector3 direction = (target[0].transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
            if (Field.bases[currentBase].isOccupied)
            {
                Field.bases[currentBase].isOccupied = false;
            }
            if (CheckEqual(rb.position, new Vector2(target[0].transform.position.x, target[0].transform.position.y),0.1f))
            {
                if(currentBase == 3)
                {
                    GameControl.instance.ChangeTeamScore(1);
                    RemoveRunner();
                }
                else
                {
                    for (int i = Field.bases.Length - 1; i > 0; i--)
                    {
                        if(target[0] == Field.bases[i].baseObj)
                        {
                            currentBase = i;
                        }
                    }
                    Field.bases[currentBase].isOccupied = true;
                }
                target.Remove(target[0]);
                if (target.Count == 0)
                {
                    target.Clear();
                }
            }
        }

        if(currentBase >= 4)
        {
            GameControl.instance.ChangeTeamScore(1);
        }
    }

    bool CheckEqual(Vector2 v1, Vector2 v2, float tolerance)
    {
        if (Mathf.Abs(v1.x - v2.x) < tolerance && Mathf.Abs(v1.y - v2.y) < tolerance)
        {
            return true;
        }
        else return false;
    }

    public void SetBaseAsTarget(List<GameObject> baseToTarget)
    {
        foreach (var targetBase in baseToTarget)
        {
            target.Add(targetBase);
        }
        Debug.Log("Added " + target.Count + " bases to target for " + gameObject.name);
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
