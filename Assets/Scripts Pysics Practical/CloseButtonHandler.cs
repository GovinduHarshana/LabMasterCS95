using UnityEngine;
using UnityEngine.UI;

public class CloseButtonHandler : MonoBehaviour
{
    
    public GameObject fullGuidancePanel;

    // This method will be called when the Close Button is clicked
    public void OnCloseButtonClicked()
    {
        // Disable the FullGuidancePanel
        if (fullGuidancePanel != null)
        {
            fullGuidancePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("FullGuidancePanel reference is not set in the Inspector.");
        }
    }
}