using UnityEngine;

public class CameraControl : MonoBehaviour {

    public bool followBall = false;
    public Transform camParent;
    public float smoothSpeed = 0.12f;

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
        Vector3 smoothedPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, -10), smoothSpeed);
        transform.localPosition = smoothedPosition;
    }

    public void LateUpdate()
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
