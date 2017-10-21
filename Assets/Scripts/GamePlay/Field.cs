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

    public static List<Runner> CheckWhichRunnersAdvance()
    {
        List<Runner> runnersToAdvance = new List<Runner>();
        foreach (var runner in runners)
        {
            if (runner.currentBase > 0)
            {
                if(runner.currentBase == 1)
                {
                    runnersToAdvance.Add(runner);
                }else if (bases[runner.currentBase - 1] != null && bases[runner.currentBase - 1].isOccupied)
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
        foreach (var runner in CheckWhichRunnersAdvance())
        {
            int targetBase = runner.currentBase + numberOfBases;
            if(targetBase > 3)
            {
                targetBase = 0;
            }
            runner.SetBaseAsTarget(targetBase);
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
