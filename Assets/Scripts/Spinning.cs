using UnityEngine;

public class Spinning : MonoBehaviour
{
    public enum SpinAxis { X_Axis, Y_Axis, Z_Axis } // Choose rotation axis
    public SpinAxis spinAxis = SpinAxis.X_Axis; 

    public float spinSpeed = 125f; // Speed of rotation

    void Update()
    {
        // Rotate based on the selected axis
        switch (spinAxis)
        {
            case SpinAxis.X_Axis:
                transform.Rotate(Vector3.right * spinSpeed * Time.deltaTime, Space.Self);
                break;
            case SpinAxis.Y_Axis:
                transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.Self);
                break;
            case SpinAxis.Z_Axis:
                transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime, Space.Self);
                break;
        }
    }
}
