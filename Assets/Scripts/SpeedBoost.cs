using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    public float speedIncrease = 1.5f;
    public float duration = 5f;

    [Header("Hover & Rotate Settings")]
    [SerializeField] private float rotationSpeed = 50f; // Rotation speed
    [SerializeField] private float hoverSpeed = 2f; // Speed of hovering movement
    [SerializeField] private float hoverHeight = 0.5f; // Height of hover movement

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position; // Store initial position
    }

    private void Update()
    {
        RotateObject();
        HoverObject();
    }

    private void RotateObject()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void HoverObject()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterMovement movement = other.GetComponent<CharacterMovement>();
            if (movement != null)
            {
                StartCoroutine(ApplySpeedBoost(movement));
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplySpeedBoost(CharacterMovement movement)
    {
        float originalMultiplier = movement.speedMultiplier;
        movement.speedMultiplier = originalMultiplier * speedIncrease;

        yield return new WaitForSeconds(duration);

        if (movement.speedMultiplier == originalMultiplier * speedIncrease)
        {
            movement.speedMultiplier = originalMultiplier;
        }
    }
}
