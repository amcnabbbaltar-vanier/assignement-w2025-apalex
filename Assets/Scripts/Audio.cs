using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;
    public CharacterMovement movement;

    void Start()
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Play Double Jump Sound
        //if (movement.doubleJump)
        //{
        //    source.PlayOneShot(clip);
        //}
    }
}
