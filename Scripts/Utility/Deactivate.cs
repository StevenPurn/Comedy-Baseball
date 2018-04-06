using System.Collections;
using UnityEngine;

public class Deactivate : MonoBehaviour {

    public float lifeTime;

    void OnEnable()
    {
        StartCoroutine(LateCall());
    }

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}
