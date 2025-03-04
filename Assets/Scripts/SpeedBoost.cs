using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    public float speedIncrease = 1.5f;
    public float duration = 5f;
    public float respawnTime = 5f;

    [Header("Hover & Rotate Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float hoverSpeed = 2f;
    [SerializeField] private float hoverHeight = 0.5f;

    private Vector3 startPosition;
    private ParticleSystem pickupParticles;
    private Renderer objectRenderer;
    private Collider objectCollider;

    private void Start()
    {
        startPosition = transform.position;
        pickupParticles = GetComponentInChildren<ParticleSystem>();
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
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

            if (pickupParticles != null)
            {
                GameObject particleEffect = new GameObject("SpeedBoostEffect");
                particleEffect.transform.position = transform.position;

                ParticleSystem newParticles = Instantiate(pickupParticles, particleEffect.transform);
                newParticles.Play();

                Destroy(particleEffect, newParticles.main.duration + newParticles.main.startLifetime.constantMax);
            }

            StartCoroutine(RespawnSpeedBoost());
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

    private IEnumerator RespawnSpeedBoost()
    {
        objectRenderer.enabled = false;
        objectCollider.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        transform.position = startPosition;
        objectRenderer.enabled = true;
        objectCollider.enabled = true;
    }
}
