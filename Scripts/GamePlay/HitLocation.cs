using UnityEngine;

public class HitLocation : MonoBehaviour {
    public GameObject center;
    public Vector2 minOffset;
    public Vector2 maxOffset;

    private void Awake()
    {
        center = this.gameObject;
        if(minOffset == Vector2.zero)
        {
            minOffset = new Vector2(-1, -1);
        }

        if (maxOffset == Vector2.zero)
        {
            maxOffset = new Vector2(1, 1);
        }
    }
}
