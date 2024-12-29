using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickChemical : MonoBehaviour
{
    public GameObject stick; // Reference to the stick GameObject
    public GameObject uiBox; // Reference to the popup box
    public TMP_Text messageText; // Reference to the UI Text element for the message

    public Color chemicalColor = Color.gray; // Set the desired color

    public void OnYesButtonClick()
    {
        // Change the color of the stick's front area
        SpriteRenderer stickRenderer = stick.GetComponent<SpriteRenderer>();
        stickRenderer.color = chemicalColor;


        // Hide the UI popup box
        uiBox.SetActive(false);
    }

    public void OnNoButtonClick()
    {
        // Close the popup box without doing anything
        uiBox.SetActive(false);
    }
}
