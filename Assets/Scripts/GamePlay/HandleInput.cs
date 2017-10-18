using UnityEngine;

public class HandleInput : MonoBehaviour
{
	void Update ()
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
	}
}
