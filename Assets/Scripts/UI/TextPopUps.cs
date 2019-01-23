using UnityEngine.UI;
using UnityEngine;

public class TextPopUps : MonoBehaviour {

    public static TextPopUps instance;
    public Sprite strike, outImg, homeRun, meh, nice, thatWasBad, foul;
    public GameObject popUpObj;
    public Image popUpImg;

	void Start () {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        popUpObj.SetActive(false);
        popUpImg = popUpObj.GetComponent<Image>();
	}

    public void ShowPopUp(string type)
    {
        switch (type)
        {
            case "strike":
                popUpImg.sprite = strike;
                break;
            case "out":
                popUpImg.sprite = outImg;
                break;
            case "homerun":
                popUpImg.sprite = homeRun;
                break;
            case "meh":
                popUpImg.sprite = meh;
                break;
            case "nice":
                popUpImg.sprite = nice;
                break;
            case "thatWasBad":
                popUpImg.sprite = thatWasBad;
                break;
            case "foul":
                popUpImg.sprite = foul;
                break;
        }
        popUpObj.SetActive(true);
    }
}
