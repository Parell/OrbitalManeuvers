using UnityEngine;

[System.Serializable]
public struct OrbitData
{
    public int Index;
    public double Mass;
    public Vector3d Velocity;
    public Vector3d Position;

    public OrbitData(int index, double mass, Vector3d velocity, Vector3d position)
    {
        Index = index;
        Mass = mass;
        Velocity = velocity;
        Position = position;
    }

    private Vector3d Gravity(OrbitData[] bodyData)
    {
        var acceleration = Vector3d.zero;

        for (int i = 0; i < bodyData.Length; i++)
        {
            if (i == Index) { continue; }

            Vector3d r = (bodyData[i].Position - Position);

            acceleration += (r * (Constant.G * bodyData[i].Mass)) / (r.magnitude * r.magnitude * r.magnitude);
        }

        return acceleration;
    }

    public void Integration(OrbitData[] bodyData, float deltaTime, IntegrationMode integrationMode)
    {
        if (integrationMode == IntegrationMode.Euler)
        {
            Velocity += deltaTime * Gravity(bodyData);
            Position += deltaTime * Velocity;
        }
        else if (integrationMode == IntegrationMode.Leapfrog)
        {
            Velocity += deltaTime * Gravity(bodyData);
            Position += deltaTime * Velocity;
        }
    }
}
