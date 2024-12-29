using UnityEngine;

public class ChemicalInteraction : MonoBehaviour
{
    public GameObject uiBox; // Reference to the UI popup box

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the stick is touching the chemical
        if (other.CompareTag("Stick"))
        {
            // Display the UI popup box
            uiBox.SetActive(true);
        }
    }
}
