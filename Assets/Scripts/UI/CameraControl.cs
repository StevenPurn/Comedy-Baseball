using UnityEngine;

public class CameraControl : MonoBehaviour {

    public bool followBall = false;
    public Transform camParent;

    private void Awake()
    {
        camParent = GameObject.Find("CameraParent").transform;
        SetParent(camParent);
    }

    public void SetParent(Transform par)
    {
        this.transform.parent = par;
    }

    public void SetParent()
    {
        this.transform.parent = camParent;
    }

    public void ResetPosition()
    {
        this.transform.localPosition = new Vector3(0,0,-10);
    }

    public void Update()
    {
        if (followBall)
        {
            SetParent(Field.ball.transform);
            ResetPosition();
        }
        else
        {
            SetParent(camParent);
            ResetPosition();
        }
    }
}
