using System.Collections.Generic;

public class Pitch {

    public Dictionary<float, Pitcher.Pitches> types;
    public List<HitLocation> hitLoc = new List<HitLocation>();
    public float minSpeed, maxSpeed, hitSpeed;
    public float maxHeight;
    public int inputNumber;
    public Pitcher.Pitches type;

    public Pitch(Dictionary<float, Pitcher.Pitches> types, List<HitLocation> locs, float minSpeed, float maxSpeed, float height, int inputNum)
    {
        this.types = types;
        this.hitLoc = locs;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
        this.maxHeight = height;
        this.inputNumber = inputNum;
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