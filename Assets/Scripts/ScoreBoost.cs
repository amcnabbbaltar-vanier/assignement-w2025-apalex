using UnityEngine;

public class ScoreBoost : MonoBehaviour
{
    [Header("Hover & Rotate Settings")]
    [SerializeField] private float rotationSpeed = 50f; // Speed of rotation
    [SerializeField] private float hoverSpeed = 2f; // Speed of hovering movement
    [SerializeField] private float hoverHeight = 0.5f; // Height of hover movement

    [Header("Score Settings")]
    [SerializeField] private int scoreValue = 50; // Score to add

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
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.score += scoreValue; // Add score
            }
            Destroy(gameObject); // Destroy after pickup
        }
    }
}
