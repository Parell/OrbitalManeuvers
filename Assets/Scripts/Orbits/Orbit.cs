using UnityEngine;

[ExecuteAlways]
public class Orbit : MonoBehaviour
{
    public double mass = 1; // put into data
    public Vector3d velocity;
    public Vector3d position;
    public Vector3d nextForce;

    private void Update()
    {
        if (Application.isPlaying)
        {
            transform.position = (Vector3)(position/*  - FloatingOrigin.Instance.originPosition */);
            // scaledTransform.position = (Vector3)(Position / Constant.SCALE/*  - FloatingOrigin.Instance.originPositionScaled */);
        }
        else
        {
            position = (Vector3d)transform.position;
            // transform.position = scaledTransform.position * Constant.SCALE;
        }
    }
 // put into data
    public void CalculateForces(Orbit[] orbits)
    {
        Vector3d force = Vector3d.zero;

        foreach (Orbit orbit in orbits)
        {
            if (orbit == this) { continue; }

            force += Physics.CalculateForceOfGravity(mass, position, orbit.mass, orbit.position);
        }

        nextForce = force;
    }
 // put into data
    public void ApplyForces(float stepSize)
    {
        double acceleration = nextForce.magnitude / mass;

        velocity = acceleration * stepSize * nextForce.normalized + velocity;
        position += velocity * stepSize;
    }
 // put into data
    public void AddForce(Vector3 direction, float acceleration, float deltaTime)
    {
        velocity += (Vector3d)(acceleration * direction * deltaTime);
    }
}
