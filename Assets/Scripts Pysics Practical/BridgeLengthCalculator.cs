using UnityEngine;
using TMPro;

public class BridgeLengthCalculator : MonoBehaviour
{
    public Transform firstBridge;
    public Transform secondBridge;
    public TextMeshProUGUI lengthText;

    public float startOffset = 5f; // Start length at 5 cm
    public float pointsPerCm = 6f; // 6 points = 1 cm

    private void Update()
    {
        // Calculate the distance between the two bridges in Unity units
        float distanceInUnits = Mathf.Abs(secondBridge.position.x - firstBridge.position.x);

        // Convert the distance to centimeters using the pointsPerCm ratio
        float distanceInCm = (distanceInUnits * pointsPerCm) + startOffset;

        // Update the UI text
        lengthText.text = "Length: " + distanceInCm.ToString("F2") + " cm";
    }
}