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
    public static Vector2 ballLandingSpot;
    public static bool ballHasBeenThrown;
    public static Transform fieldParent;

    public static void AssignDugouts()
    {
        GameControl.instance.activeTeams[0].dugout = dugouts[0];
        GameControl.instance.activeTeams[1].dugout = dugouts[1];
        fieldParent = GameObject.Find("Field").transform;
    }

    public static void SetUpHitLocations(List<GameObject> objs)
    {
        for (int i = 0; i < objs.Count; i++)
        {
            foreach (HitLocation hitLoc in objs[i].GetComponentsInChildren<HitLocation>())
            {
                Pitches.pitches[i + 1].hitLoc.Add(hitLoc);
            }
        }
    }

    public static void BatterIsOut()
    {
        foreach (var runner in runners)
        {
            if(runner.team == GameControl.instance.GetTeamAtBat() && runner.atBat)
            {
                string aud = "out" + UnityEngine.Random.Range(1, 5);
                AudioControl.instance.PlayAudio(aud);
                runner.SetOut();
                return;
            }
        }
    }

    public static void FielderAI()
    {
        if(runners.Find(x => x.isAdvancing) == null)
        {
            GameControl.ballInPlay = false;
        }
        Fielder player = fielders.Find(x => x.ballInHands);
        if (GameControl.ballInPlay || player.position != Fielder.Position.pitcher)
        {
            MoveFieldersToPlayPosition();
            if(ballLandingSpot == Vector2.zero)
            {
                GetClosestFielderToTransform(ball.transform).movementTarget = ball.transform.position;
            }
            else
            {
                GetClosestFielderToLocation(ballLandingSpot).movementTarget = ballLandingSpot;
            }
            if(fielders.Find(x => x.ballInHands))
            {
                WhatDoIDoWithTheBall(player);
            }
        }
        else if(GameControl.ballInPlay == false)
        {
            if (player != null && player.position != Fielder.Position.pitcher)
            {
                player.ThrowBall(GetLocationToThrowBall());
            }
            MoveFieldersToStartPosition(false);
        }
    }

    public static Fielder GetClosestFielderToTransform(Transform loc, bool accountForDistFromStartPos = true)
    {
        if(loc == null)
        {
            return null;
        }
        Fielder closestPlayer = fielders[0];
        float lowestDist = float.MaxValue;
        foreach (var player in fielders)
        {
            float distance = Vector2.Distance(player.transform.position, loc.position) + Vector2.Distance(player.transform.position, player.startPosition.position);
            if (distance < lowestDist)
            {
                closestPlayer = player;
                lowestDist = distance;
            }
        }

        return closestPlayer;
    }

    public static Fielder GetClosestFielderToLocation(Vector2 loc, bool accountForDistFromStartPos = true)
    {
        Fielder closestPlayer = fielders[0];
        float lowestDist = float.MaxValue;
        foreach (var player in fielders)
        {
            float distance = Vector2.Distance(player.transform.position, loc) + Vector2.Distance(player.transform.position, player.startPosition.position);
            if (distance < lowestDist)
            {
                closestPlayer = player;
                lowestDist = distance;
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
        if (GetFurthestRunner() == null)
        {
            player.ThrowBall(GetLocationToThrowBall());
            return;
        }
        Transform baseLocation = GetFurthestRunner().targetBase[0].transform;
        GetClosestFielderToTransform(baseLocation, false).movementTarget = baseLocation.position;
        if (player == GetClosestFielderToTransform(baseLocation))
        {
            if(Utility.CheckEqual(player.transform.position, baseLocation.position, 0.1f) && !GameControl.ballInPlay)
            {
                player.ThrowBall(GetLocationToThrowBall());
            }
            return;
        }
        else
        {
            player.ThrowBall(GetLocationToThrowBall());
        }
    }

    public static void MoveFieldersToStartPosition(bool ignorePlayerWithBall = true)
    {
        if (ignorePlayerWithBall)
        {
            foreach (var player in fielders)
            {
                if (!player.ballInHands)
                {
                    player.movementTarget = player.startPosition.position;
                }
            }
        }
        else
        {
            foreach (var player in fielders)
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
        //Check if ball has been thrown
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
        }else if(Vector2.Distance(GetClosestFielderToLocation(ball.transform.position, false).transform.position, ball.transform.position) < 0.5f || ballHasBeenThrown)
        {
            return false;
        }
        else
        {
            return GameControl.ballInPlay;
        }
    }

    public static Vector2 GetLocationToThrowBall()
    {
        Vector2 loc = Vector2.zero;
        if(GameControl.ballInPlay == false)
        {
            loc = fielders.Find(x => x.position == Fielder.Position.pitcher).glove.position;
        }else if (GetFurthestRunner() == null)
        {
            GameControl.ballInPlay = false;
            loc = fielders.Find(x => x.position == Fielder.Position.pitcher).glove.position;
        }else if (!runners.Find(x => x.isAdvancing))
        {
            GameControl.ballInPlay = false;
            loc = fielders.Find(x => x.position == Fielder.Position.pitcher).glove.position;
        }
        else
        {
            Fielder player = GetClosestFielderToTransform(GetFurthestRunner().targetBase[0].transform, false);
            loc = player.glove.position;
        }
        return loc;
    }

    public static Runner GetFurthestRunner()
    {
        Runner run = null;
        int highestBase = int.MinValue;
        foreach (var runner in runners)
        {
            if (runner.currentBase > highestBase && !runner.exitingField)
            {
                highestBase = runner.currentBase;
                run = runner;
            }
        }
        return run;
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

    public static void CheckIfRunnerOut(Runner runner)
    {
        Fielder fielder = GetClosestFielderToTransform(runner.targetBase[0].transform, false);
        if (fielder.ballInHands && Utility.CheckEqual(fielder.transform.position, runner.targetBase[0].transform.position, 0.1f))
        {
            if(GetFurthestRunner() == null)
            {
                GameControl.ballInPlay = false;
            }
            string aud = "out" + UnityEngine.Random.Range(1, 5);
            AudioControl.instance.PlayAudio(aud);
            runner.SetOut();
            GameControl.instance.HandleOut();
        }
    }
}
