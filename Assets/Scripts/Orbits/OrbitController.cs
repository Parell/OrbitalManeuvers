using UnityEngine;

public enum IntegrationMode
{
    Euler, Leapfrog
}

public class OrbitController : MonoBehaviour
{
    public static OrbitController Instance;

    public IntegrationMode integrationMode;
    public float fixedStepSize = 0.01f;
    public bool masslessOptimization;
    public ComputeShader compute;
    public Orbit[] orbits;
    public OrbitData[] orbitData;

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
        Time.fixedDeltaTime = fixedStepSize;

        FindOrbits();

        orbitData = new OrbitData[orbits.Length];

        for (int i = 0; i < orbitData.Length; i++)
        {
            orbitData[i] = new OrbitData(i, orbits[i].mass, orbits[i].velocity, orbits[i].position);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < GameController.Instance.timeScale; i++)
        {
            orbitData = Propagation(orbitData, Time.fixedDeltaTime, integrationMode);

            for (int j = 0; j < orbits.Length; j++)
            {
                orbits[j].velocity = orbitData[j].velocity;
                orbits[j].position = orbitData[j].position;
            }
        }
    }

    public void FindOrbits()
    {
        orbits = FindObjectsOfType<Orbit>();
    }

    public OrbitData[] Propagation(OrbitData[] bodyData, float deltaTime, IntegrationMode integrationMode)
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
