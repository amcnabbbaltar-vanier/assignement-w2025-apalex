using UnityEngine;

public class JumpBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    [SerializeField] private float jumpMultiplier = 1.3f; // How much to multiply jump force
    [SerializeField] private float boostDuration = 30f; // How long the boost lasts

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
        if (other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            CharacterMovement playerMovement = other.GetComponent<CharacterMovement>();

            if (playerMovement != null)
            {
                StartCoroutine(ApplyJumpBoost(playerMovement));
            }
            Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator ApplyJumpBoost(CharacterMovement playerMovement)
    {
        float originalJumpForce = playerMovement.jumpForce; // Store original jump force
        playerMovement.jumpForce *= jumpMultiplier; // Apply boost

        yield return new WaitForSeconds(boostDuration); // Wait for duration

        playerMovement.jumpForce = originalJumpForce; // Reset jump force
    }
}
