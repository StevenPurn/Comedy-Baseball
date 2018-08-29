using UnityEngine;

public class HandleInput : MonoBehaviour
{
    public static bool listenForHits;

    void Update ()
    {
        bool result = Field.AreRunnersAdvancing();
        result = !result || listenForHits;
        if (result)
        {
            if (Controls.GetButtonDown("hit1"))
            {
                GameControl.instance.HandlePitch(1);
            }
            else if (Controls.GetButtonDown("hit2"))
            {
                GameControl.instance.HandlePitch(2);
            }
            else if (Controls.GetButtonDown("hit3"))
            {
                GameControl.instance.HandlePitch(3);
            }
            else if (Controls.GetButtonDown("hit4"))
            {
                GameControl.instance.HandlePitch(4);
            }
            else if (Controls.GetButtonDown("hit5"))
            {
                GameControl.instance.HandlePitch(5);
            }
            else if (Controls.GetButtonDown("hit6"))
            {
                GameControl.instance.HandlePitch(6);
            }
            else if (Controls.GetButtonDown("hit7"))
            {
                GameControl.instance.HandlePitch(7);
            }
            else if (Controls.GetButtonDown("hit8"))
            {
                GameControl.instance.HandlePitch(8);
            }
            else if (Controls.GetButtonDown("hit9"))
            {
                GameControl.instance.HandlePitch(9);
            }
            else if (Controls.GetButtonDown("hit10"))
            {
                GameControl.instance.HandlePitch(10);
            }
            Field.UpdateBases();
        }
        if (listenForHits)
        {
            GameControl.instance.FastForward(Controls.GetButton("FastForward"));
        }
    }
}
