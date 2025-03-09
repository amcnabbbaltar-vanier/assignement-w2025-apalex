using UnityEngine;

public class TranslateObjects : MonoBehaviour
{
    public enum MoveAxis { X_Axis, Y_Axis, Z_Axis } // Dropdown for movement axis
    public MoveAxis moveAxis = MoveAxis.X_Axis; 

    public float moveSpeed = 2f; // Speed of movement (units per second)
    public float moveDistance = 5f; // Total movement distance in units

    private Vector3 startPosition;
    private int direction = 1; // Moving forward (1) or backward (-1)

    void Start()
    {
        startPosition = transform.position; // Store the starting position
    }

    void Update()
    {
        MoveObject();
    }

    void MoveObject()
    {
        // Calculate the movement vector based on the selected axis
        Vector3 moveVector = Vector3.zero;

        switch (moveAxis)
        {
            case MoveAxis.X_Axis:
                moveVector = Vector3.right;
                break;
            case MoveAxis.Y_Axis:
                moveVector = Vector3.up;
                break;
            case MoveAxis.Z_Axis:
                moveVector = Vector3.forward;
                break;
        }

        // Move the object
        transform.Translate(moveVector * moveSpeed * direction * Time.deltaTime);

        // Reverse direction if the object moves beyond the set distance
        if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
        {
            direction *= -1; // Reverse movement direction
        }
    }
}
