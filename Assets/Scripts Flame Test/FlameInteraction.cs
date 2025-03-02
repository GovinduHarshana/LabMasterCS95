using UnityEngine;
using TMPro;

public class FlameInteraction : MonoBehaviour
{
    public Color defaultFlameColor = Color.white;
    private SpriteRenderer flameRenderer;
    public ChemicalTableManager tableManager;

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
        // Check if the stick tip is touching the flame
        if (other.CompareTag("StickTip"))
        {
            // Change the flame color to the selected chemical color
            if (flameRenderer != null)
            {
                flameRenderer.color = ChemicalData.Instance.SelectedChemicalColor;
            }

            // Update the table based on the selected chemical color
            if (tableManager != null)
            {
                tableManager.UpdateTable(ChemicalData.Instance.SelectedChemicalColor);
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