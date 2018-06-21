public class HRDPlayer
{
    public string name;
    public bool isAtBat =false;
    public int outs = 0;
    public int hits = 0;
    public int homeruns = 0;

    public HRDPlayer(string name)
    {
        this.name = name;
    }

    public void ChangeHits(int change)
    {
        hits += change;
    }

    public void ChangeOuts(int change)
    {
        outs += change;
    }

    public void ChangeHomeruns(int change)
    {
        homeruns += change;
    }

    public void SetAtBat(bool atBat)
    {
        isAtBat = atBat;
    }
}
