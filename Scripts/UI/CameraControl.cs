using UnityEngine;

public class CameraControl : MonoBehaviour {

    public bool followBall = false;
    public Transform camParent;
    private float smoothSpeed = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        camParent = GameObject.Find("CameraParent").transform;
        SetParent(camParent);
    }

    public void SetParent(Transform par)
    {
        transform.parent = par;
    }

    public void SetParent()
    {
        transform.parent = camParent;
    }

    public void ResetPosition()
    {
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(0, 0, -10), ref velocity, smoothSpeed);
        transform.localPosition = smoothedPosition;
    }

    public void LateUpdate()
    {
        if (followBall)
        {
            SetParent(Field.ball.transform);
        }
        else
        {
            SetParent(camParent);
        }
        ResetPosition();
    }
}
