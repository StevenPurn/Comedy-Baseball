using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Runner : MonoBehaviour {

    public Base firstBase, secondBase, thirdBase, homePlate, currentBase;
    private GameObject target;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void AdvanceBase()
    {
        if(currentBase == null)
        {
            target = firstBase.gameObject;
        }
        else if(currentBase == firstBase)
        {
            target = secondBase.gameObject;
        }else if(currentBase == secondBase)
        {
            target = thirdBase.gameObject;
        }else if(currentBase == thirdBase)
        {
            target = homePlate.gameObject;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            rb.MovePosition(target.transform.position);
        }
    }
}
