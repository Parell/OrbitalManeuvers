using UnityEngine;

public enum IntegrationMode
{
    Euler,
    Leapfrog,
}

public class OrbitController : MonoBehaviour
{
    public static OrbitController Instance;

    public IntegrationMode integrationMode;
    public float fixedStepSize = 0.01f;
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
        FindOrbits();

        orbitData = new OrbitData[orbits.Length];

        for (int i = 0; i < orbitData.Length; i++)
        {
            // orbitData[i] = new OrbitData(i, orbits[i].mass, orbits[i].velocity, orbits[i].position);
        }
    }

    private void FixedUpdate()
    {

    }

    private void FindOrbits()
    {
        orbits = FindObjectsOfType<Orbit>();
    }
}
