using System.Collections.Generic;
using UnityEngine;

public class OrbitManeuver : MonoBehaviour
{
    public List<Maneuver> maneuvers;
    public Orbit orbitBody;
    public float predictionInterval = 1;
    public OrbitPredictor orbitPredictor;

    private float predictionTimer;

    private void Update()
    {
        //orbitController.maneuver = maneuver;

        // if (maneuver.deltaV == Vector3.zero && orbitBody.keplerian.SMA > 0)
        // {
        //     orbitController.steps = (int)(((orbitBody.keplerian.T) / 2) / orbitController.stepSize);
        // }
        // else if (maneuver.deltaV != Vector3.zero && orbitBody.keplerian.SMA >= 0)
        // {
        //     orbitController.steps = (int)(((maneuver.startTime + predictionLength) / 2) / orbitController.stepSize);
        // }

        //maneuver.direction = (maneuver.deltaV + (Vector3)orbitBody.velocity).normalized;

        // if (Application.isPlaying)
        // {
        //     predictionTimer = predictionTimer <= 0 ? predictionInterval : predictionTimer -= Time.deltaTime * GameController.Instance.timeScale;

        //     if (predictionTimer <= 0)
        //     {
        //         if (maneuver.startTime > 0)
        //         {
        //             maneuver.startTime -= Time.deltaTime * GameController.Instance.timeScale;
        //         }
        //         else if (maneuver.startTime <= 0)
        //         {
        //             if (maneuver.duration >= 0)
        //             {
        //                 orbitController.orbitData[1].AddConstantAcceleration(maneuver, 0);
        //                 maneuver.duration -= Time.deltaTime * GameController.Instance.timeScale;

        //                 maneuver = new Maneuver();
        //             }
        //         }
        //     }
        // }
        // else
        // {
        // }
    }
}
