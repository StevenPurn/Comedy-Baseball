using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class Controls {

    //Read these in from an xml file once we get that setup
    public static Dictionary<string, KeyCode> controls = new Dictionary<string, KeyCode>()
    {
        { "Strike", KeyCode.S },
        { "Out", KeyCode.O },
        { "Ball", KeyCode.B },
        { "Single", KeyCode.Alpha1 },
        { "Double", KeyCode.Alpha2 },
        { "Triple", KeyCode.Alpha3 },
        { "Homerun", KeyCode.Alpha4 }
    };

    public static bool GetButtonDown(string button)
    {
        return Input.GetKeyDown(controls[button]);
    }

    public static string[] GetButtonNames()
    {
        return controls.Keys.ToArray();
    }

    public static float GetAxis(string axis)
    {
        float amount = 0.0f;
        amount = Input.GetAxis(axis);
        return amount;
    }

    public static bool GetButton(string button)
    {
        return Input.GetKey(controls[button]);
    }
}