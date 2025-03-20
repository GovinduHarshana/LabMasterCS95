using UnityEngine;

public class TuningFork : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public float soundDuration = 2f; // Duration of the sound

    private bool isSoundPlaying = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name); // Debug log

        if (other.gameObject.CompareTag("RubberHammer") && !isSoundPlaying)
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            isSoundPlaying = true;
            Invoke(nameof(StopSound), soundDuration);
        }
    }

    private void StopSound()
    {
        isSoundPlaying = false;
    }
}