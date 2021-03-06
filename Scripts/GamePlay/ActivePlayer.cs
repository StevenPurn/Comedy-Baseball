﻿/// <summary>
///  The players currently participating in the game.
/// </summary>
public class ActivePlayer
{
    public int number;
    public string name;
    public string portraitPath;
    public bool isAtBat;
    public bool isPitching;
    public int atBats, totalAtBats;
    public int strikeoutsAtBat, totalStrikeoutsAtBat;
    public int hits, totalHits;
    public int rbis, totalRbis;
    public int runs, totalRuns;
    public int pitches, totalPitches;
    public int strikeoutsPitched, totalStrikesoutsPitched;

    public void ChangeAtBats(int change)
    {
        totalAtBats += change;
        atBats += change;
    }

    public void ChangeStrikeOutsAtBat(int change)
    {
        totalStrikeoutsAtBat += change;
        strikeoutsAtBat += change;
    }

    public void ChangeHits(int change)
    {
        totalHits += change;
        hits += change;
    }

    public void ChangeRBIs(int change)
    {
        totalRbis += change;
        rbis += change;
    }

    public void ChangeRuns(int change)
    {
        totalRuns += change;
        runs += change;
    }

    public void ChangeStrikeoutsPitched(int change)
    {
        totalStrikesoutsPitched += change;
        strikeoutsPitched += change;
    }
    
    public void ChangePitches(int change)
    {
        totalPitches += change;
        pitches = change;
    }
}
