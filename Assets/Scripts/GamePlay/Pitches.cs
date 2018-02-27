using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitches : MonoBehaviour {
    //EXAMPLE
    /*
     * pitch1 where 1 repesents the 1-10 score given by the umpire
    public static Pitch pitch1 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.popfly }, ----- number represents the probability of the pitch type occurring, pitch type doesn't mean too much right now other than strike
            { 0.6f, Pitcher.Pitches.groundOut }
        },
    new List<Vector2>() {
            new Vector2 (1,1), ------ possibilities for which direction the ball will be hit
            new Vector2 (-1, 1)
    }, 0.5f, 2.0f, 5.0f);  --------- in order (minimum hit speed, max hit speed, max hit height)
    */

        //New object that contains a vector2 for landing location and four floats for randomly adjusting the location

    public static Pitch pitch1 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.popfly },
            { 0.6f, Pitcher.Pitches.groundOut }
        }, 
            new List<Vector2>() {
            new Vector2 (1,1),
            new Vector2 (-1, 1)
            }, 1.2f, 5.0f, 5.0f);

    public static Pitch pitch2 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.popfly },
            { 0.6f, Pitcher.Pitches.groundOut }
        },
            new List<Vector2>() {
            new Vector2 (1,1),
            new Vector2 (-1, 1)
            }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch3 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.strike },
            { 0.6f, Pitcher.Pitches.foul }
        },
        new List<Vector2>() {
            new Vector2 (0, 1),
            new Vector2 (0, -1)
        }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch4 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.popfly },
            { 0.6f, Pitcher.Pitches.groundOut }
        },
        new List<Vector2>() {
            new Vector2 (0.2f, 0.8f)
        }, 4f, 8f, 5.0f);

    public static Pitch pitch5 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.popfly },
            { 0.6f, Pitcher.Pitches.groundOut }
        },
        new List<Vector2>() {
            new Vector2 (1,1),
            new Vector2 (-1, 1)
        }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch6 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.popfly },
            { 0.6f, Pitcher.Pitches.groundOut }
        },
        new List<Vector2>() {
            new Vector2 (-0.5f,0.5f),
            new Vector2 (-0.2f, 1f)
        }, 6f, 10f, 5.0f);

    public static Pitch pitch7 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.popfly },
            { 0.6f, Pitcher.Pitches.groundOut }
        },
        new List<Vector2>() {
            new Vector2 (0.5f,0.5f),
            new Vector2 (0.2f, 1f)
        }, 6f, 10f, 5.0f);

    public static Pitch pitch8 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.popfly },
            { 0.6f, Pitcher.Pitches.groundOut }
        },
        new List<Vector2>() {
            new Vector2 (1,1),
            new Vector2 (-1, 1)
        }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch9 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.hit },
            { 0.6f, Pitcher.Pitches.homerun }
        },
        new List<Vector2>() {
            new Vector2 (1,1),
            new Vector2 (-1, 1)
        }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch10 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 1.0f, Pitcher.Pitches.homerun },
        },
        new List<Vector2>() {
            new Vector2 (1,1),
            new Vector2 (-1, 1)
        }, 10f, 20f, 5.0f);

    public static Dictionary<int, Pitch> pitches = new Dictionary<int, Pitch>()
    {
        { 1, pitch1 },
        { 2, pitch2 },
        { 3, pitch3 },
        { 4, pitch4 },
        { 5, pitch5 },
        { 6, pitch6 },
        { 7, pitch7 },
        { 8, pitch8 },
        { 9, pitch9 },
        { 10, pitch10 }
    };
}