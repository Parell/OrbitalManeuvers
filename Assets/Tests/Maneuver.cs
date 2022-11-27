using UnityEngine;

public enum StearingMode
{
    Fixed, Perpendicular, Tangent
}

[System.Serializable]
public class Maneuver
{
    public StearingMode stearingMode;
    public Thruster thruster;
    public Vector3 direction;
    public float acceleration;
    public float duration;
    public float startTime;
}
