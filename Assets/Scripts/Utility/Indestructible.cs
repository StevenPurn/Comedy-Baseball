﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indestructible : MonoBehaviour {

	void Start () {
        DontDestroyOnLoad(gameObject);
	}
}
