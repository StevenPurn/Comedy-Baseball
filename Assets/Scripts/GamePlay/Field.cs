using System;
using System.Collections.Generic;
using UnityEngine;

public static class Field {

    public static Base[] bases = new Base[4];
    public static GameObject[] dugouts = new GameObject[2];
    public static Dictionary<Fielder.Position, GameObject> fieldPositions = new Dictionary<Fielder.Position, GameObject> { };
    public static Dictionary<Fielder.Position, GameObject> playPositions = new Dictionary<Fielder.Position, GameObject> { };
    public static List<Runner> runners = new List<Runner>();
    public static List<Fielder> fielders = new List<Fielder>();
    public static List<GameObject> runnerTargets = new List<GameObject>();
    public static Ball ball;
    public static bool ballHasBeenThrown;

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
                runner.SetOut();
                return;
            }
        }
    }

    public static void FielderAI()
    {
        if (GameControl.ballInPlay && !ballHasBeenThrown)
        {
            MoveFieldersToPlayPosition();
            GetClosestFielderToTransform(ball.transform).movementTarget = ball.transform.position;
            if(fielders.Find(x => x.ballInHands))
            {
                Fielder player = fielders.Find(x => x.ballInHands);
                WhatDoIDoWithTheBall(player);
            }
        }
        else if(GameControl.ballInPlay == false)
        {
            MoveFieldersToStartPosition();
        }
    }

    public static Fielder GetClosestFielderToTransform(Transform loc)
    {
        Fielder closestPlayer = fielders[0];
        float dis = float.MaxValue;
        foreach (var player in fielders)
        {
            if (Vector2.Distance(player.transform.position, loc.position) < dis)
            {
                closestPlayer = player;
                dis = Vector2.Distance(player.transform.position, loc.position);
            }
        }

        return closestPlayer;
    }

    public static void MoveFieldersToPlayPosition(bool ignorePlayerWithBall = true)
    {
        if (ignorePlayerWithBall)
        {
            foreach (var player in fielders)
            {
                if (!player.ballInHands)
                {
                    player.movementTarget = player.playPosition.position;
                }
            }
        }
        else
        {
            foreach (var player in fielders)
            {
                player.movementTarget = player.playPosition.position;
            }
        }
    }

    public static void WhatDoIDoWithTheBall(Fielder player)
    {
        Transform baseLocation = GetFurthestRunner().targetBase[0].transform;
        GetClosestFielderToTransform(baseLocation).movementTarget = baseLocation.position;
        if (player == GetClosestFielderToTransform(baseLocation))
        {
            return;
        }
        else
        {
            player.ThrowBall(GetDirectionToThrowBall(player.transform.position));
        }
    }

    public static void MoveFieldersToStartPosition()
    {
        foreach (var player in fielders)
        {
            if (!player.ballInHands)
            {
                player.movementTarget = player.startPosition.position;
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
            dir = fielders.Find(x => x.position == Fielder.Position.pitcher).glove.position - position;
        }
        else
        {
            if(GetFurthestRunner() == null)
            {
                GameControl.ballInPlay = false;
                dir = fielders.Find(x => x.position == Fielder.Position.pitcher).glove.position - position;
            }
            else
            {
                Fielder player = GetClosestFielderToTransform(GetFurthestRunner().targetBase[0].transform);
                dir = player.glove.position - position;
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
        int highestBase = int.MinValue;
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
            runners[i].SetOut();
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
