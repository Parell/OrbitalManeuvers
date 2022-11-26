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

    public Maneuver(Maneuver maneuver)
    {
        this.stearingMode = maneuver.stearingMode;
        this.thruster = maneuver.thruster;
        this.acceleration = maneuver.acceleration;
        this.duration = maneuver.duration;
        this.startTime = maneuver.startTime;
        this.direction = maneuver.direction;
    }

    public Maneuver(StearingMode stearingMode, Thruster thruster, float acceleration, float duration, float startTime, Vector3 direction)
    {
        this.stearingMode = stearingMode;
        this.thruster = thruster;
        this.acceleration = acceleration;
        this.duration = duration;
        this.startTime = startTime;
        this.direction = direction;
    }
}
