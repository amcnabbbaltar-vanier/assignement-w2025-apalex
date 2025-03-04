using UnityEngine;

public class ScoreBoost : MonoBehaviour
{
    [Header("Hover & Rotate Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float hoverSpeed = 2f;
    [SerializeField] private float hoverHeight = 0.5f;

    [Header("Score Settings")]
    [SerializeField] private int scoreValue = 50;

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
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.score += scoreValue;
            }

            if (pickupParticles != null)
            {
                GameObject particleEffect = new GameObject("ScoreBoostEffect");
                particleEffect.transform.position = transform.position;

                ParticleSystem newParticles = Instantiate(pickupParticles, particleEffect.transform);
                newParticles.Play();

                Destroy(particleEffect, newParticles.main.duration + newParticles.main.startLifetime.constantMax);
            }

            Destroy(gameObject);
        }
    }
}
