using UnityEngine;
using UnityEngine.UI;

public class TuningForkInteraction : MonoBehaviour
{
    public Toggle sonometerToggle; // Reference to the UI toggle
    public string sonometerTag = "Sonometer"; // Tag for the sonometer board

    private void Start()
    {
        // Ensure the toggle is unchecked at the beginning
        if (sonometerToggle != null)
        {
            sonometerToggle.isOn = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(sonometerTag))
        {
            UpdateToggle(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(sonometerTag))
        {
            UpdateToggle(false);
        }
    }

    private void UpdateToggle(bool isTouching)
    {
        if (sonometerToggle != null)
        {
            sonometerToggle.isOn = isTouching;
        }
    }
}