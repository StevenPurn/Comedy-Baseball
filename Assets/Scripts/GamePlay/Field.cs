using System.Collections.Generic;
using UnityEngine;

public static class Field {

    public static Base[] bases = new Base[4];
    public static GameObject[] dugouts = new GameObject[2];
    public static Dictionary<Fielder.Position, GameObject> fieldPositions = new Dictionary<Fielder.Position, GameObject> { };
    public static Dictionary<Fielder.Position, GameObject> playPositions = new Dictionary<Fielder.Position, GameObject> { };
    public static List<Runner> runners = new List<Runner>();
    public static List<Fielder> fielders = new List<Fielder>();
    public static Runner mostRecentBatter, currentBatter;
    public static List<GameObject> runnerTargets = new List<GameObject>();
    public static Ball ball;
    public static Vector2 ballLandingSpot;
    public static bool ballHasBeenThrown;
    public static Transform fieldParent;
    public static bool fieldersCanReact;
    private static Vector3 ballOffset = new Vector3(0.09f, -0.05f, 0f);

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
        string aud = "";
        if(GameControl.outsThisPlay == 2)
        {
            aud = "doubleplay";
        }
        else
        {
            TextPopUps.instance.ShowPopUp("out");
            aud = "out" + Random.Range(1, 5);
        }
        AudioControl.instance.PlayAudio(aud);
        currentBatter.SetOut();
    }

    public static void FielderAI()
    {
        if (runners.Find(x => x.isAdvancing) == null && ballHasBeenThrown)
        {
            GameControl.ballInPlay = false;
        }
        Fielder fielderWithBall = fielders.Find(x => x.ballInHands);
        bool pitcherHasBall = false;

        if(fielderWithBall != null)
        {
            ball.transform.parent = fielderWithBall.glove.gameObject.transform;
            ball.transform.localPosition = Vector3.zero;
            ball.curHeight = 1f;
            pitcherHasBall = fielderWithBall.position == Fielder.Position.pitcher;
        }
        else
        {
            if(ball != null)
            {
                ball.transform.parent = fieldParent;
            }
        }

        if (fieldersCanReact)
        {
            if (GameControl.ballInPlay || pitcherHasBall == false)
            {
                MoveFieldersToPlayPosition();
                if (ball.hasHitGround && ballHasBeenThrown == false)
                {
                    GetClosestFielderToTransform(ball.transform, false).movementTarget = ball.transform.position + ballOffset;
                }
                else
                {
                    GetClosestFielderToLocation(ballLandingSpot).movementTarget = ballLandingSpot;
                }
                if (fielderWithBall != null)
                {
                    WhatDoIDoWithTheBall(fielderWithBall);
                }
            }
        }

        if (GameControl.ballInPlay == false)
        {
            if (fielderWithBall != null && fielderWithBall.position != Fielder.Position.pitcher)
            {
                fielderWithBall.ThrowBall(GetPlayerToThrowBallTo());
            }
            else if (fielderWithBall != null && fielderWithBall.position == Fielder.Position.pitcher)
            {
                GameControl.instance.SetCameraToFollowBall(false);
                GameControl.playIsActive = false;
                fieldersCanReact = false;
            }
            MoveFieldersToStartPosition(false);
        }

        if (ball != null && ball.targetFielder != null)
        {
            ball.targetFielder.movementTarget = ball.endPoint;
        }
    }

    public static Fielder GetClosestFielderToTransform(Transform loc, bool accountForDistFromStartPos = true)
    {
        if(loc == null)
        {
            return null;
        }
        Fielder closestPlayer = null;
        float lowestDist = float.MaxValue;
        foreach (Fielder player in fielders)
        {
            float distance = Vector2.Distance(player.transform.position, loc.position);
            distance += accountForDistFromStartPos ? Vector2.Distance(player.transform.position, player.startPosition.position) : 0f;
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
        Fielder closestPlayer = null;
        float lowestDist = float.MaxValue;
        foreach (Fielder player in fielders)
        {
            float distance = Vector2.Distance(player.transform.position, loc);
            distance += accountForDistFromStartPos ? Vector2.Distance(player.transform.position, player.startPosition.position) : 0f;
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
        foreach(var player in fielders)
        {
            if(ignorePlayerWithBall && player.ballInHands)
            {
                continue;
            }
            player.movementTarget = player.playPosition.position;
        }
    }

    //do all of these just lead to the ball being thrown to
    //whoever the target player is?
    //There should be a case where the player runs to the base with the ball
    public static void WhatDoIDoWithTheBall(Fielder player)
    {
        Runner furthestRunner = GetFurthestRunner();
        if (furthestRunner == null)
        {
            Fielder targetPlayer = GetPlayerToThrowBallTo();
            if (targetPlayer != player)
            {
                player.ThrowBall(targetPlayer);
            }
            return;
        }
        Transform baseLocation = furthestRunner.targetBase[0].transform;
        GetClosestFielderToTransform(baseLocation, false).movementTarget = baseLocation.position;
        if (player == GetClosestFielderToTransform(baseLocation))
        {
            if(Utility.CheckEqual(player.transform.position, baseLocation.position, 0.1f) && !GameControl.ballInPlay)
            {
                var targetPlayer = GetPlayerToThrowBallTo();
                if (targetPlayer != player)
                {
                    player.ThrowBall(targetPlayer);
                }
            }
            return;
        }
        else
        {
            var targetPlayer = GetPlayerToThrowBallTo();
            if (targetPlayer != player)
            {
                player.ThrowBall(targetPlayer);
            }
        }
    }

    public static void MoveFieldersToStartPosition(bool ignorePlayerWithBall = true)
    {
        if(fielders.Count > 0)
        {
            foreach (var player in fielders)
            {
                if (ignorePlayerWithBall && player.ballInHands)
                {
                    continue;
                }
                player.movementTarget = player.startPosition.position;
            }
        }
    }

    //Add public variable for UI Control Obj
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
        else if (GameControl.isHomeRun && !runner.atBat)
        {
            return true;
        }
        else if (runners.Find(x => x.isAdvancing && x.currentBase + 1 == runner.currentBase))
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

    public static Fielder GetPlayerToThrowBallTo()
    {
        Fielder playerToThrowTo;
        if(GameControl.ballInPlay == false)
        {
            playerToThrowTo = fielders.Find(x => x.position == Fielder.Position.pitcher && !x.inningOver);
        }else if (GetFurthestRunner() == null)
        {
            GameControl.ballInPlay = false;
            playerToThrowTo = fielders.Find(x => x.position == Fielder.Position.pitcher && !x.inningOver);
        }else
        {
            playerToThrowTo = GetClosestFielderToTransform(GetFurthestRunner().targetBase[0].transform, false); ;
        }
        return playerToThrowTo;
    }

    public static Runner GetFurthestRunner(bool onlyConsiderAdvancingRunner = true)
    {
        Runner run = null;
        int highestBase = int.MinValue;
        foreach (var runner in runners)
        {
            if(onlyConsiderAdvancingRunner && runner.isAdvancing == false)
            {
                continue;
            }
            else if (runner.currentBase > highestBase && !runner.exitingField)
            {
                highestBase = runner.currentBase;
                run = runner;
            }
        }
        return run;
    }

    public static void ResetInning()
    {
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
                Debug.LogWarning("This should never show up");
                GameControl.ballInPlay = false;
            }
            string aud;
            if (GameControl.outsThisPlay == 1)
            {
                aud = "doubleplay";
            }else if (GameControl.outsThisPlay == 2)
            {
                aud = "tripleplay";
            }
            else
            {
                TextPopUps.instance.ShowPopUp("out");
                aud = "out" + Random.Range(1, 5);
            }
            AudioControl.instance.PlayAudio(aud);
            runner.SetOut();
            GameControl.instance.HandleOut();
        }
    }

}
