using UnityEngine;

public class HandleInput : MonoBehaviour
{

    void Update ()
    {
        if(!Field.runners.Find(x => x.isAdvancing))
        {
            if (Controls.GetButtonDown("Single"))
            {
                GameControl.instance.HandleHit(1);
            }
            else if (Controls.GetButtonDown("Double"))
            {
                GameControl.instance.HandleHit(2);
            }
            else if (Controls.GetButtonDown("Triple"))
            {
                GameControl.instance.HandleHit(3);
            }
            else if (Controls.GetButtonDown("Homerun"))
            {
                GameControl.instance.HandleHit(4);
            }
            else if (Controls.GetButtonDown("Foul"))
            {
                GameControl.instance.HandleStrike(true);
            }
            else if (Controls.GetButtonDown("Strike"))
            {
                GameControl.instance.HandleStrike();
            }
            else if (Controls.GetButtonDown("Ball"))
            {
                GameControl.instance.HandleBall();
            }
            else if (Controls.GetButtonDown("Out"))
            {
                GameControl.instance.HandleOut();
            }
        }
        else
        {
            Field.UpdateBases();
        }
        GameControl.instance.FastForward(Controls.GetButton("FastForward"));
    }
}
