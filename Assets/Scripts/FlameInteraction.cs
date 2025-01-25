using UnityEngine;

public class FlameInteraction : MonoBehaviour
{
    public Color normalFlameColor = Color.white; // Default flame color
    public Color activeFlameColor = Color.red;   // Color when the stick tip is touching the flame
    private SpriteRenderer flameRenderer;

    private void Start()
    {
        // Get the SpriteRenderer component of the flame
        flameRenderer = GetComponent<SpriteRenderer>();
        if (flameRenderer != null)
        {
            // Set the initial flame color
            flameRenderer.color = normalFlameColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the stick tip touches the flame
        if (other.CompareTag("StickTip"))
        {
            if (flameRenderer != null)
            {
                // Change the flame's color
                flameRenderer.color = activeFlameColor;
                Debug.Log("Stick tip is touching the flame!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the stick tip stops touching the flame
        if (other.CompareTag("StickTip"))
        {
            if (flameRenderer != null)
            {
                // Revert the flame's color to normal
                flameRenderer.color = normalFlameColor;
                Debug.Log("Stick tip is no longer touching the flame!");
            }
        }
    }
}
