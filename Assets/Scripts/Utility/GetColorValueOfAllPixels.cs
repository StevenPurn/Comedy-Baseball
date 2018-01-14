using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetColorValueOfAllPixels : MonoBehaviour {

    public SpriteRenderer sr;
    public Sprite sprite;
    public List<Color> colors;

	// Use this for initialization
	void Start() {
        sr = GetComponent<SpriteRenderer>();
        sprite = sr.sprite;

        foreach (var col in sprite.texture.GetPixels())
        {
            Debug.Log(col);
        }
	}
}
