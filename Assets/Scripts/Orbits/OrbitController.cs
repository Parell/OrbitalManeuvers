using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class OrbitController : MonoBehaviour
{
    public static OrbitController Instance;

    public double universalTime;
    public int timeScale;
    // public float errorTimeScale = 1f;
    [Space]
    public float fixedStepSize = 0.01f;
    [Space]
    public double plotLength;
    public double tolerance = 1E-08;
    [ReadOnly]
    public float relitiveError = 1e-10f;
    [ReadOnly]
    public float absoluteError = 1e-10f;
    public float stepSize = 1f;
    [ReadOnly]
    public int steps = 100;
    public Orbit referenceFrame;
    public Orbit[] orbits;
    public OrbitData[] virtualOrbitData;
    [ReadOnly]
    public List<Maneuver> maneuvers;
    [Space]
    public float plotUpdateInterval = 1f;

    private float plotUpdateTimer;

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

        Time.fixedDeltaTime = fixedStepSize;

        FindOrbits();

        // orbitData = new OrbitData[orbits.Length];

        // for (int i = 0; i < orbitData.Length; i++)
        // {
        //     orbitData[i] = new OrbitData(i, orbits[i].mass, orbits[i].velocity, orbits[i].position);
        // }
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            plotUpdateTimer = plotUpdateTimer <= 0 ? plotUpdateInterval : plotUpdateTimer -= Time.deltaTime;

            if (plotUpdateTimer <= 0)
            {
                UpdateOrbitPlot();
            }
        }
        else
        {
            FindOrbits();
            UpdateOrbitPlot();
        }
    }

    private void UpdateOrbitPlot()
    {
        steps = (int)(plotLength / stepSize);

        virtualOrbitData = new OrbitData[orbits.Length];
        Vector3[][] drawPoints = new Vector3[orbits.Length][];

        int referenceFrameIndex = 0;
        Vector3d referenceBodyInitialPosition = Vector3d.zero;

        for (int i = 0; i < orbits.Length; i++)
        {
            virtualOrbitData[i] = new OrbitData(i, orbits[i].mass, orbits[i].velocity, orbits[i].position); // try constructor for mono
            drawPoints[i] = new Vector3[steps];

            if (referenceFrame != null && orbits[i] == referenceFrame)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualOrbitData[i].position;
            }

            if (orbits[i].GetComponent<OrbitManeuver>())
            {
                maneuvers = new List<Maneuver>(orbits[i].GetComponent<OrbitManeuver>().maneuvers.Count);

                orbits[i].GetComponent<OrbitManeuver>().maneuvers.ForEach((item) =>
                {
                    maneuvers.Add(new Maneuver(item));
                });
            }
        }

        for (int step = 0; step < steps; step++)
        {
            Vector3d referenceBodyPosition = (referenceFrame != null) ? virtualOrbitData[referenceFrameIndex].position : Vector3d.zero;

            virtualOrbitData = Propagation(virtualOrbitData, stepSize);

            for (int i = 0; i < virtualOrbitData.Length; i++)
            {
                if (orbits[i].GetComponent<OrbitManeuver>())
                {
                    for (int j = 0; j < maneuvers.Count; j++)
                    {
                        if ((step * stepSize) >= maneuvers[j].startTime)
                        {
                            if (maneuvers[j].duration >= 0)
                            {
                                virtualOrbitData[i].AddForce(maneuvers[j].direction.normalized, maneuvers[j].acceleration, stepSize);

                                maneuvers[j].duration -= stepSize;
                            }
                        }
                    }
                }

                Vector3d nextPosition = virtualOrbitData[i].position;
                if (referenceFrame != null)
                {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    nextPosition -= referenceFrameOffset;
                }
                if (referenceFrame != null && i == referenceFrameIndex)
                {
                    nextPosition = referenceBodyInitialPosition;
                }

                drawPoints[i][step] = (Vector3)(nextPosition)/*  / Constant.SCALE - (Vector3)floatingOrigin.originPositionScaled */;
            }
        }

        for (int bodyIndex = 0; bodyIndex < virtualOrbitData.Length; bodyIndex++)
        {
            var lineRenderer = orbits[bodyIndex].transform.GetComponent<LineRenderer>();
            lineRenderer.positionCount = drawPoints[bodyIndex].Length;
            lineRenderer.SetPositions(drawPoints[bodyIndex]);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < timeScale; i++)
        {
            universalTime += Time.fixedDeltaTime;

            foreach (Orbit orbit in orbits)
            {
                orbit.CalculateForces(orbits);
                orbit.ApplyForces(Time.fixedDeltaTime);
            }
        }
    }

    public void FindOrbits()
    {
        orbits = FindObjectsOfType<Orbit>();
    }

    public OrbitData[] Propagation(OrbitData[] bodyData, float deltaTime)
    {
        for (int i = 0; i < bodyData.Length; i++)
        {
            OrbitData tempBodyData = bodyData[i];

            tempBodyData.Integration(bodyData, deltaTime);

            bodyData[i] = tempBodyData;
        }

        return bodyData;
    }
}
