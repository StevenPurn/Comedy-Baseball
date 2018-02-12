using System;
using System.Collections.Generic;
using UnityEngine;

public static class Field {

    public static Base[] bases = new Base[4];
    public static GameObject[] dugouts = new GameObject[2];
    public static Dictionary<Fielder.Position, GameObject> fieldPositions = new Dictionary<Fielder.Position, GameObject> { };
    public static List<Runner> runners = new List<Runner>();
    public static List<Fielder> fielders = new List<Fielder>();
    public static List<GameObject> runnerTargets = new List<GameObject>();
    public static Ball ball;

    public static void AssignDugouts()
    {
        GameControl.instance.activeTeams[0].dugout = dugouts[0];
        GameControl.instance.activeTeams[1].dugout = dugouts[1];
    }

    public static void BatterIsOut()
    {
        foreach (var runner in runners)
        {
            if(runner.team == GameControl.instance.GetTeamAtBat() && runner.atBat)
            {
                runner.RemoveRunner();
                return;
            }
        }
    }

    public static void UpdateBases()
    {
        for (var i = 0; i < bases.Length; i++)
        {
            bases[i].isOccupied = runners.Find(x => x.currentBase == i);
        }

        GameObject.FindObjectOfType<UIControl>().UpdateBaseIndicators();
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

    public static Vector2 GetDirectionToThrowBall(Vector3 position)
    {
        Vector2 dir = Vector2.zero;

        if(!runners.Find(x => x.isAdvancing))
        {
            GameControl.ballInPlay = false;
            dir = fielders.Find(x => x.position == Fielder.Position.pitcher).transform.position - position;
        }
        else
        {
            if(GetFurthestRunner() == null)
            {
                GameControl.ballInPlay = false;
                dir = fielders.Find(x => x.position == Fielder.Position.pitcher).transform.position - position;
            }
            else
            {
                dir = GetFurthestRunner().targetBase[0].transform.position - position;
            }
        }
        return dir;
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
            runner.SetBasesAsTargets(baseList);
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
        //Set fielders to return to dugout
        foreach (var fielder in fielders)
        {
            fielder.inningOver = true;
        }
        for (int i = runners.Count - 1; i >= 0; i--)
        {
            runners[i].RemoveRunner();
        }

        SpawnFielders();
    }

    public static void SpawnFielders()
    {
        foreach (var pos in fieldPositions)
        {
            GameControl.instance.AddFielderToField(pos.Key, pos.Value);
        }
    }
}
