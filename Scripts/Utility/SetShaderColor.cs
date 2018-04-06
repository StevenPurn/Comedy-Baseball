using UnityEngine;
using UnityEngine.UI;

public class SetShaderColor : MonoBehaviour {

    public Color col;
    public Material mat;

	// Use this for initialization
	void Awake ()
    {
        mat = GetComponent<Image>().material;
        mat.SetColor("_Color", col);
    }

    public void UpdateShaderColor()
    {
        mat.SetColor("_Color", col);
    }
}
