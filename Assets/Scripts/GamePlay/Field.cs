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
    public static Transform fieldParent;

    public static void AssignDugouts()
    {
        GameControl.instance.activeTeams[0].dugout = dugouts[0];
        GameControl.instance.activeTeams[1].dugout = dugouts[1];
        fieldParent = GameObject.Find("Field").transform;
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
            player.ThrowBall(GetDirectionToThrowBall(player.glove.transform.position));
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

    public static bool CanRunnerAdvance(Runner runner)
    {
        if (runner.anim.GetCurrentAnimatorStateInfo(0).IsName("runnerSwingBat"))
        {
            return false;
        }
        else if (runner.isAdvancing)
        {
            return true;
        }
        else if(runner.exitingField || runner.enteringField)
        {
            return true;
        }
        else if(fielders.Find(x => x.ballInHands))
        {
            return false;
        }
        else
        {
            return GameControl.ballInPlay;
        }
    }

    public static Vector2 GetDirectionToThrowBall(Vector3 position)
    {
        Vector2 dir = Vector2.zero;

        if (GetFurthestRunner() == null)
        {
            GameControl.ballInPlay = false;
            dir = fielders.Find(x => x.position == Fielder.Position.pitcher).glove.position - position;
        }else if (!runners.Find(x => x.isAdvancing))
        {
            GameControl.ballInPlay = false;
            dir = fielders.Find(x => x.position == Fielder.Position.pitcher).glove.position - position;
        }
        else
        {
            Fielder player = GetClosestFielderToTransform(GetFurthestRunner().targetBase[0].transform);
            dir = player.glove.position - position;
        }
        return dir;
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
