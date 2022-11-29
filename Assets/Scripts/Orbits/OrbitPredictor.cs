using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways, RequireComponent(typeof(OrbitController))]
public class OrbitPredictor : MonoBehaviour
{
    public OrbitController orbitController;
    public float predictionLength;
    public float stepSize = 1;
    public int steps = 100;
    public float predictionInterval = 1;
    public Orbit referenceFrame;
    public OrbitData[] virtualOrbitData;

    private float predictionTimer;
    public List<Maneuver> maneuvers;

    private void Update()
    {
        if (Application.isPlaying)
        {
            predictionTimer = predictionTimer <= 0 ? predictionInterval : predictionTimer -= Time.deltaTime;

            if (predictionTimer <= 0)
            {
                UpdateOrbit();
            }
        }
        else
        {
            orbitController.FindOrbits();
            UpdateOrbit();
        }
    }

    private void UpdateOrbit()
    {
        steps = (int)(predictionLength / (stepSize * virtualOrbitData.Length));

        virtualOrbitData = new OrbitData[orbitController.orbits.Length];
        Vector3[][] drawPoints = new Vector3[orbitController.orbits.Length][];

        int referenceFrameIndex = 0;
        Vector3d referenceBodyInitialPosition = Vector3d.zero;

        for (int i = 0; i < orbitController.orbits.Length; i++)
        {
            virtualOrbitData[i] = new OrbitData(i, orbitController.orbits[i].mass, orbitController.orbits[i].velocity, orbitController.orbits[i].position);
            drawPoints[i] = new Vector3[steps];

            if (referenceFrame != null && orbitController.orbits[i] == referenceFrame)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualOrbitData[i].position;
            }

            if (orbitController.orbits[i].GetComponent<OrbitManeuver>())
            {
                maneuvers = new List<Maneuver>(orbitController.orbits[i].GetComponent<OrbitManeuver>().maneuvers.Count);

                orbitController.orbits[i].GetComponent<OrbitManeuver>().maneuvers.ForEach((item) =>
                {
                    maneuvers.Add(new Maneuver(item));
                });
            }
        }

        for (int step = 0; step < steps; step++)
        {
            Vector3d referenceBodyPosition = (referenceFrame != null) ? virtualOrbitData[referenceFrameIndex].position : Vector3d.zero;

            for (int i = 0; i < virtualOrbitData.Length; i++)
            {
                if (orbitController.orbits[i].GetComponent<OrbitManeuver>())
                {
                    for (int j = 0; j < maneuvers.Count; j++)
                    {
                        if ((step * stepSize * virtualOrbitData.Length) >= maneuvers[j].startTime)
                        {
                            if (maneuvers[j].duration >= 0)
                            {
                                virtualOrbitData[i].AddForce(maneuvers[j].direction.normalized, maneuvers[j].acceleration, stepSize);

                                maneuvers[j].duration -= stepSize;
                            }
                        }
                    }
                }

                virtualOrbitData = orbitController.Propagation(virtualOrbitData, stepSize, orbitController.integrationMode);

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
            var lineRenderer = orbitController.orbits[bodyIndex].transform.GetComponent<LineRenderer>();
            lineRenderer.positionCount = drawPoints[bodyIndex].Length;
            lineRenderer.SetPositions(drawPoints[bodyIndex]);
        }
    }
}
