using UnityEngine;

public class FlameInteraction : MonoBehaviour
{
    public Color defaultFlameColor = new Color(0f, 1f, 1f); // Default flame color
    private SpriteRenderer flameRenderer;

    private void Start()
    {
        // Get the SpriteRenderer component of the flame
        flameRenderer = GetComponent<SpriteRenderer>();
        if (flameRenderer != null)
        {
            flameRenderer.color = defaultFlameColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called with: " + other.name); // Check if the method is triggered

        if (other.CompareTag("StickTip"))
        {
            Debug.Log("StickTip detected!"); // Verify the tag check
            if (flameRenderer != null)
            {
                Debug.Log("Selected Chemical Color: " + ChemicalData.Instance.SelectedChemicalColor); // Check the color
                flameRenderer.color = ChemicalData.Instance.SelectedChemicalColor;
            }
            else
            {
                Debug.LogError("Flame Renderer is null!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Reset the flame color when the stick tip leaves
        if (other.CompareTag("StickTip"))
        {
            if (flameRenderer != null)
            {
                flameRenderer.color = defaultFlameColor;
            }
        }
    }
}