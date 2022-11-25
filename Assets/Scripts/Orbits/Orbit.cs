using UnityEngine;

[ExecuteAlways]
public class Orbit : MonoBehaviour
{
    public double mass;
    public Vector3d velocity;
    public Vector3d position;

    private void Update()
    {
        if (Application.isPlaying)
        {
            transform.position = (Vector3)(position/*  - FloatingOrigin.Instance.originPosition */);
            // scaledTransform.position = (Vector3)(Position / Constant.SCALE/*  - FloatingOrigin.Instance.originPositionScaled */);
        }
        else
        {
            position = (Vector3d)transform.position;
            // transform.position = scaledTransform.position * Constant.SCALE;
        }
    }
}
