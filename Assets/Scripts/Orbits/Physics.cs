using UnityEngine;

public class Physics
{
    public static Vector3d CalculateForceOfGravity(double mass, Vector3d position, double otherMass, Vector3d otherPosition)
    {
        if (otherMass < 1e7)
        {
            return Vector3d.zero;
        }
        else
        {
            Vector3d direction = (otherPosition - position).normalized;
            double force = Constant.G * ((mass * otherMass) / (Vector3d.Distance(position, otherPosition) * Vector3d.Distance(position, otherPosition)));
            return force * direction;
        }
    }
}
