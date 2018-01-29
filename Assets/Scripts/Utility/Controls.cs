using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class Controls {

    //Read these in from an xml file once we get that setup
    public static Dictionary<string, KeyCode> controls = new Dictionary<string, KeyCode>()
    {
        { "hit1",     KeyCode.Alpha1  },
        { "hit2",     KeyCode.Alpha2  },
        { "hit3",     KeyCode.Alpha3  },
        { "hit4",     KeyCode.Alpha4  },
        { "hit5",     KeyCode.Alpha5  },
        { "hit6",     KeyCode.Alpha6  },
        { "hit7",     KeyCode.Alpha7  },
        { "hit8",     KeyCode.Alpha8  },
        { "hit9",     KeyCode.Alpha9  },
        { "hit10",    KeyCode.Alpha0  },
        { "Foul",       KeyCode.F       },
        { "Strike",     KeyCode.S       },
        { "Ball",       KeyCode.B       },
        { "Out",        KeyCode.O       },
        { "Steal",      KeyCode.T       },
        { "FastForward",KeyCode.Space   }
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