public class ActivePlayer
{
    public string name;
    public bool isAtBat;
    public int atBats;
    public int strikeoutsAtBat;
    public int hits;
    public int rbis;
    public int runs;
    public int strikeoutsPitched;

    public void ChangeAtBats(int change)
    {
        atBats += change;
    }

    public void ChangeStrikeOutsAtBat(int change)
    {
        strikeoutsAtBat += change;
    }

    public void ChangeHits(int change)
    {
        hits += change;
    }

    public void ChangeRBIs(int change)
    {
        rbis += change;
    }

    public void ChangeRuns(int change)
    {
        runs += change;
    }

    public void ChangeStrikeoutsPitched(int change)
    {
        strikeoutsPitched += change;
    }
}
