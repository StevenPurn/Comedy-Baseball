using UnityEngine;

public class ChangeAudioOnLoad : MonoBehaviour {

    public string audioName;
    public bool playAudio;

	// Use this for initialization
	void Start () {
        if (playAudio)
        {
            AudioControl.instance.PlayAudio(audioName);
        } else
        {
            AudioControl.instance.Stop();
        }
    }
}
