using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitches : MonoBehaviour {

    public static Pitch pitch1 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.5f, Pitcher.Pitches.popfly },
            { 0.5f, Pitcher.Pitches.groundOut }
        }, 
            new List<Vector3>() { }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch2 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.5f, Pitcher.Pitches.popfly },
            { 0.5f, Pitcher.Pitches.groundOut }
        },
            new List<Vector3>() { }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch3 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.5f, Pitcher.Pitches.popfly },
            { 0.5f, Pitcher.Pitches.groundOut }
        },
        new List<Vector3>() { }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch4 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.5f, Pitcher.Pitches.popfly },
            { 0.5f, Pitcher.Pitches.groundOut }
        },
        new List<Vector3>() { }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch5 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.5f, Pitcher.Pitches.popfly },
            { 0.5f, Pitcher.Pitches.groundOut }
        },
        new List<Vector3>() { }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch6 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.5f, Pitcher.Pitches.popfly },
            { 0.5f, Pitcher.Pitches.groundOut }
        },
        new List<Vector3>() { }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch7 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.5f, Pitcher.Pitches.popfly },
            { 0.5f, Pitcher.Pitches.groundOut }
        },
        new List<Vector3>() { }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch8 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.5f, Pitcher.Pitches.popfly },
            { 0.5f, Pitcher.Pitches.groundOut }
        },
        new List<Vector3>() { }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch9 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 0.5f, Pitcher.Pitches.hit },
            { 0.5f, Pitcher.Pitches.homerun }
        },
        new List<Vector3>() { }, 0.5f, 2.0f, 5.0f);

    public static Pitch pitch10 = new Pitch(new Dictionary<float, Pitcher.Pitches>() {
            { 1.0f, Pitcher.Pitches.homerun },
        },
        new List<Vector3>() { }, 0.5f, 2.0f, 5.0f);
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