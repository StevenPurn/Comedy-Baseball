using UnityEngine.UI;
using UnityEngine;

public class TextPopUps : MonoBehaviour {

    public static TextPopUps instance;
    public Sprite strike, outImg, homeRun;
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
	
	// Update is called once per frame
	void Update () {
		
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
        }
        popUpObj.SetActive(true);
    }
}
