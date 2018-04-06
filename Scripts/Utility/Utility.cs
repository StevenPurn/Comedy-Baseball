using UnityEngine;

public static class Utility
{
    public static bool CheckEqual(Vector2 v1, Vector2 v2, float tolerance)
    {
        if (Mathf.Abs(v1.x - v2.x) < tolerance && Mathf.Abs(v1.y - v2.y) < tolerance)
        {
            return true;
        }
        else return false;
    }
}
