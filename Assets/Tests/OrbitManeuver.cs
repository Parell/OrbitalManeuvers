using UnityEngine;

public class OrbitManeuver : MonoBehaviour
{
    public Maneuver[] maneuvers;
    public Orbit orbitBody;
    // public float predictionLength;
    //public OrbitController orbitController;

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
        //     // if (maneuver.startTime > 0)
        //     // {
        //     //     maneuver.startTime -= Time.deltaTime * GameController.Instance.timeScale;
        //     // }
        //     // else if (maneuver.startTime <= 0)
        //     // {
        //     //     if (maneuver.duration >= 0)
        //     //     {
        //     //         // orbitController.orbitData[1].AddConstantAcceleration(maneuver, 0);
        //     //         maneuver.duration -= Time.deltaTime * GameController.Instance.timeScale;

        //     //         maneuver = new Maneuver();
        //     //     }
        //     // }
        // }
    }
}
