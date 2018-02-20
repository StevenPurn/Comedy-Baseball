﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitch {

    public Dictionary<float, Pitcher.Pitches> types;
    public List<Vector3> hitAngles = new List<Vector3>();
    public float minSpeed, maxSpeed, hitSpeed;
    public float maxHeight;
    public Pitcher.Pitches type;

    public Pitch(Dictionary<float, Pitcher.Pitches> types, List<Vector3> angles, float minSpeed, float maxSpeed, float height)
    {
        this.types = types;
        this.hitAngles = angles;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
        this.maxHeight = height;
    }

    public void SetType(Pitcher.Pitches pitchType)
    {
        type = pitchType;
    }

    public void SetSpeed(float speed)
    {
        hitSpeed = speed;
    }
}
