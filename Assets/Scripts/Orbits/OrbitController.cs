using UnityEngine;

public enum IntegrationMode
{
    Euler,
    Leapfrog,
}

public class OrbitController : MonoBehaviour
{
    public static OrbitController Instance;

    public IntegrationMode IntegrationMode;
    public float FixedStepSize = 0.01f;
    public Orbit[] Orbits;
    public OrbitData[] OrbitData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            Debug.Log($"Cant have more then one Instance on {gameObject.name}.");
        }
    }

    private void Start()
    {
        Time.fixedDeltaTime = FixedStepSize;

        FindOrbits();

        OrbitData = new OrbitData[Orbits.Length];

        for (int i = 0; i < OrbitData.Length; i++)
        {
            OrbitData[i] = new OrbitData(i, Orbits[i].Mass, Orbits[i].Velocity, Orbits[i].Position);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < GameController.Instance.TimeScale; i++)
        {
            OrbitData = Propagation(OrbitData, Time.fixedDeltaTime, IntegrationMode);

            for (int j = 0; j < Orbits.Length; j++)
            {
                Orbits[j].Velocity = OrbitData[j].Velocity;
                Orbits[j].Position = OrbitData[j].Position;
            }
        }
    }

    private void FindOrbits()
    {
        Orbits = FindObjectsOfType<Orbit>();
    }

    private OrbitData[] Propagation(OrbitData[] bodyData, float deltaTime, IntegrationMode integrationMode)
    {
        for (int i = 0; i < bodyData.Length; i++)
        {
            OrbitData tempBodyData = bodyData[i];

            tempBodyData.Integration(bodyData, deltaTime, integrationMode);

            bodyData[i] = tempBodyData;
        }

        return bodyData;
    }
}
