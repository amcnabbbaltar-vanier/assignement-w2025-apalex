using UnityEngine;
using System.Collections;

public class JumpBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    [SerializeField] private float jumpMultiplier = 1.3f;
    [SerializeField] private float boostDuration = 30f;
    [SerializeField] private float respawnTime = 5f;

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
            CharacterMovement playerMovement = other.GetComponent<CharacterMovement>();

            if (playerMovement != null)
            {
                StartCoroutine(ApplyJumpBoost(playerMovement));
            }

            if (pickupParticles != null)
            {
                GameObject particleEffect = new GameObject("JumpBoostEffect");
                particleEffect.transform.position = transform.position;

                ParticleSystem newParticles = Instantiate(pickupParticles, particleEffect.transform);
                newParticles.Play();

                Destroy(particleEffect, newParticles.main.duration + newParticles.main.startLifetime.constantMax);
            }

            StartCoroutine(RespawnJumpBoost());
        }
    }

    private IEnumerator ApplyJumpBoost(CharacterMovement playerMovement)
    {
        float originalJumpForce = playerMovement.jumpForce;
        playerMovement.jumpForce *= jumpMultiplier;
        playerMovement.SetJumpBoost(true);

        yield return new WaitForSeconds(boostDuration);

        playerMovement.jumpForce = originalJumpForce;
        playerMovement.SetJumpBoost(false);
    }

    private IEnumerator RespawnJumpBoost()
    {
        objectRenderer.enabled = false;
        objectCollider.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        transform.position = startPosition;
        objectRenderer.enabled = true;
        objectCollider.enabled = true;
    }
}
