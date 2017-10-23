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
                switch (runner.currentBase)
                {
                    case 2:
                        //If single, check if runner on first
                        if(numberOfBases == 1 && runners.Find(x => x.currentBase == 1) != null)
                        {
                            runnersToAdvance.Add(runner);
                        }else if(numberOfBases >= 2)
                        {
                            runnersToAdvance.Add(runner);
                        }
                        break;
                    case 3:
                        //If single, check if runner is on second
                        if (numberOfBases == 1 && (runners.Find(x => x.currentBase == 1) != null && runners.Find(x => x.currentBase == 2) != null))
                        {
                            runnersToAdvance.Add(runner);
                        }
                        //If double, check if runner on first or second
                        else if(numberOfBases == 2 && (runners.Find(x => x.currentBase == 1) != null || runners.Find(x => x.currentBase == 2) != null))
                        {
                            runnersToAdvance.Add(runner);
                        }
                        else if(numberOfBases > 2)
                        {
                            runnersToAdvance.Add(runner);
                        }
                        break;
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
