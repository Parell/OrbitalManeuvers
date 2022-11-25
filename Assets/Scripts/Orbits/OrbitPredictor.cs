using UnityEngine;


[ExecuteAlways, RequireComponent(typeof(OrbitController))]
public class OrbitPredictor : MonoBehaviour
{
    public OrbitController orbitController;
    public float stepSize = 1;
    public int steps = 100;
    public float predictionInterval = 1;
    public Orbit referenceFrame;
    public OrbitData[] virtualOrbitData;

    private float predictionTimer;

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
        }

        for (int step = 0; step < steps; step++)
        {
            Vector3d referenceBodyPosition = (referenceFrame != null) ? virtualOrbitData[referenceFrameIndex].position : Vector3d.zero;

            for (int i = 0; i < virtualOrbitData.Length; i++)
            {
                virtualOrbitData = orbitController.Propagation(virtualOrbitData, stepSize, orbitController.integrationMode);

                // if ((step * stepSize) * 2 >= maneuver.startTime)
                // {
                //     if (maneuver.duration >= 0)
                //     {
                //         virtualBodyData[1].AddConstantAcceleration(maneuver, stepSize);
                //         maneuver.duration -= step * stepSize;
                //     }
                //     else if (step * stepSize > (maneuver.startTime + maneuver.duration))
                //     {
                //     }
                // }
                // plotLength = (step * stepSize) * 2;

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
            var pathColour = Color.white;

            // var lineRenderer = orbits[bodyIndex].scaledTransform.GetComponent<LineRenderer>();
            var lineRenderer = orbitController.orbits[bodyIndex].transform.GetComponent<LineRenderer>();
            lineRenderer.positionCount = drawPoints[bodyIndex].Length;
            lineRenderer.SetPositions(drawPoints[bodyIndex]);
            lineRenderer.startColor = pathColour;
            lineRenderer.endColor = pathColour;
            lineRenderer.widthMultiplier = 1;
        }
    }
}
