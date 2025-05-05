using UnityEngine;

public class Particle_boost : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;  // Reference to the Particle System

    private void Awake()
    {
        // If the ParticleSystem is not assigned via Inspector, try to find the child particle system
        if (particle == null)
        {
            particle = GetComponentInChildren<ParticleSystem>();  // Get the first child with a ParticleSystem component
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boost"))
        {
            // If colliding with Boost, play the particle system
            if (!particle.isPlaying)
            {
                particle.Play();
            }
        }
        else
        {
            // Stop the particle system when not colliding with Boost
            if (particle.isPlaying)
            {
                particle.Stop();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boost"))
        {
            // Stop particle system when exiting Boost collision
            if (particle.isPlaying)
            {
                particle.Stop();
            }
        }
    }
}
