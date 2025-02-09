using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject uiBox; // Reference to the UI popup box
    public PickChemical pickChemical; // Reference to the PickChemical script

    // Called when the "Pick" button is clicked
    public void OnPickButtonClick()
    {
        if (pickChemical != null)
        {
            pickChemical.OnPickButtonClick();
        }
        HideUIBox();
    }

    // Called when the "Cancel" button is clicked
    public void OnCancelButtonClick()
    {
        HideUIBox();
    }

    // Show the UI box
    public void ShowUIBox()
    {
        if (uiBox != null)
        {
            uiBox.SetActive(true);
        }
    }

    // Hide the UI box
    private void HideUIBox()
    {
        if (uiBox != null)
        {
            uiBox.SetActive(false);
        }
    }
}