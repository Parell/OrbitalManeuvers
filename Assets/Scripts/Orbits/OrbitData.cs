using UnityEngine;

[System.Serializable]
public struct OrbitData
{
    public int index;
    public double mass;
    public Vector3d velocity;
    public Vector3d position;

    public OrbitData(int index, double mass, Vector3d velocity, Vector3d position)
    {
        this.index = index;
        this.mass = mass;
        this.velocity = velocity;
        this.position = position;
    }

    public Vector3d Gravity(OrbitData[] bodyData)
    {
        var acceleration = Vector3d.zero;

        for (int i = 0; i < bodyData.Length; i++)
        {
            if (i == index) { continue; }

            Vector3d r = (bodyData[i].position - position);

            acceleration += (r * (Constant.G * bodyData[i].mass)) / (r.magnitude * r.magnitude * r.magnitude);
        }

        return acceleration;
    }

    public void Integration(OrbitData[] bodyData, float deltaTime, IntegrationMode integrationMode)
    {
        if (integrationMode == IntegrationMode.Euler)
        {
            position += velocity * deltaTime;
            velocity += Gravity(bodyData) * deltaTime;
        }
        else if (integrationMode == IntegrationMode.Leapfrog)
        {
            velocity += Gravity(bodyData) * 0.5 * deltaTime;
            position += velocity * deltaTime;
            velocity += Gravity(bodyData) * 0.5 * deltaTime;
        }
    }

    public void AddForce(Vector3 direction, float acceleration, float deltaTime)
    {
        velocity += (Vector3d)(acceleration * direction * deltaTime);
    }
}
