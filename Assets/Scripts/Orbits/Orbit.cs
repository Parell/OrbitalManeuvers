using UnityEngine;

[ExecuteAlways]
public class Orbit : MonoBehaviour
{
    public double Mass;
    public Vector3d Velocity;
    public Vector3d Position;

    private void Update()
    {
        if (Application.isPlaying)
        {
            transform.position = (Vector3)(Position/*  - FloatingOrigin.Instance.originPosition */);
            // scaledTransform.position = (Vector3)(Position / Constant.SCALE/*  - FloatingOrigin.Instance.originPositionScaled */);
        }
        else
        {
            Position = (Vector3d)transform.position;
            // transform.position = scaledTransform.position * Constant.SCALE;
        }
    }
}
