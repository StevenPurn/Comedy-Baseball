using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Field {

    public static Base[] bases = new Base[4];
    public static List<Runner> runners = new List<Runner>();

    public static void AssignBases(Base[] _bases)
    {
        bases = _bases;
    }

    public static List<Runner> CheckWhichRunnersAdvance(int numberOfBases)
    {
        List<Runner> runnersToAdvance = new List<Runner>();
        foreach (var runner in runners)
        {
            if (runner.currentBase > 1)
            {
                if ((bases[runner.currentBase - numberOfBases] != null))
                {
                    if (bases[runner.currentBase - numberOfBases].isOccupied)
                    {
                        //Need to check if the runner on that base is also advancing
                        runnersToAdvance.Add(runner);
                    }
                }
                if(runner.currentBase - numberOfBases <= 0)
                {
                    runnersToAdvance.Add(runner);
                }
            }
            else
            {
                runnersToAdvance.Add(runner);
            }
        }
        return runnersToAdvance;
    }

    public static void AdvanceRunners(int numberOfBases = 1)
    {
        foreach (var runner in CheckWhichRunnersAdvance(numberOfBases))
        {
            List<GameObject> baseList = new List<GameObject>();
            for (int i = 1; i <= numberOfBases; i++)
            {
                if(runner.currentBase + i >= 4)
                {
                    if (baseList.Contains(bases[0].baseObj))
                    {
                        continue;
                    }
                    else
                    {
                        baseList.Add(bases[0].baseObj);
                        continue;
                    }
                }
                baseList.Add(bases[runner.currentBase + i].baseObj);
            }
            runner.SetBaseAsTarget(baseList);
        }
    }

    public static Runner GetFurthestRunner()
    {
        Runner run = null;
        int highestBase = 0;
        foreach (var runner in runners)
        {
            if (runner.currentBase > highestBase)
            {
                highestBase = runner.currentBase;
                run = runner;
            }
        }

        return run;
    }

    public static void FurthestRunnerOut()
    {
        Runner runner = GetFurthestRunner();

        if(runner != null)
        {
            runner.SetOut();
        }
    }

    public static void ResetInning()
    {
        for (int i = runners.Count - 1; i >= 0; i--)
        {
            runners[i].RemoveRunner();
        }
    }
}
