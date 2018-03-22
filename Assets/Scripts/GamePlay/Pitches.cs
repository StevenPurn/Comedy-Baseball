using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitches : MonoBehaviour {
    //EXAMPLE
    /*
     * pitch1 where 1 repesents the 1-10 score given by the umpire
    public static Pitch pitch1 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.popfly }, ----- number represents the probability of the pitch type occurring, pitch type possibilities are strike, ball, hit, homerun, grandslam (grandslam should never be selected as we will determine that at run time)
            { 0.6f, Pitcher.Pitches.groundOut }
        },
    new List<HitLocation>(), 0.5f, 2.0f, 5.0f);  --------- in order (locations on the field the ball might land, minimum hit speed, max hit speed, max hit height)
    */
    //Hit Location is ---v
    //New object that contains a vector2 for landing location and four floats for randomly adjusting the location

    public static Pitch pitch1 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 1f, Pitcher.Pitches.strike },
        }, 
        new List<HitLocation>(), 4f, 6.0f, 5.0f);

    public static Pitch pitch2 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.hit },
            { 0.6f, Pitcher.Pitches.strike }
        },
        new List<HitLocation>(), 4f, 5.0f, 5.0f);

    public static Pitch pitch3 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.strike },
            { 0.6f, Pitcher.Pitches.hit }
        },
        new List<HitLocation>(), 4f, 5.0f, 5.0f);

    public static Pitch pitch4 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 1f, Pitcher.Pitches.hit }
        },
        new List<HitLocation>(), 4f, 8f, 5.0f);

    public static Pitch pitch5 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 1f, Pitcher.Pitches.hit }
        },
        new List<HitLocation>(), 4f, 5.0f, 5.0f);

    public static Pitch pitch6 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 1f, Pitcher.Pitches.hit }
        },
        new List<HitLocation>(), 6f, 10f, 5.0f);

    public static Pitch pitch7 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 1f, Pitcher.Pitches.hit }
        },
        new List<HitLocation>(), 6f, 10f, 7.0f);

    public static Pitch pitch8 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 1f, Pitcher.Pitches.hit }
        },
        new List<HitLocation>(), 6f, 10f, 7.0f);

    public static Pitch pitch9 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.4f, Pitcher.Pitches.hit }
        },
        new List<HitLocation>(), 8f, 18f, 8.0f);

    public static Pitch pitch10 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 1.0f, Pitcher.Pitches.homerun },
        },
        new List<HitLocation>(), 10f, 20f, 10.0f);

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