using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitch {

    public Dictionary<float, Pitcher.Pitches> types;
    public List<HitLocation> hitLoc = new List<HitLocation>();
    public float minSpeed, maxSpeed, hitSpeed;
    public float maxHeight;
    public Pitcher.Pitches type;

    public Pitch(Dictionary<float, Pitcher.Pitches> types, List<HitLocation> locs, float minSpeed, float maxSpeed, float height)
    {
        this.types = types;
        this.hitLoc = locs;
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