using UnityEngine;
using UnityEngine.UI;

public class DraggablePaperRider : MonoBehaviour
{
    public Transform wire; 
    public Transform firstBridge; 
    public Transform secondBridge; 
    public Toggle midCheckbox; 

    private bool isDragging = false;
    private Vector3 offset;
    private bool isSnapped = false;

    private void Start()
    {
        // Ensure the checkbox is unchecked at the start
        if (midCheckbox != null)
        {
            midCheckbox.isOn = false;
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x + offset.x, mousePosition.y + offset.y, transform.position.z);

            // Check if the paper rider is near the wire
            if (Mathf.Abs(transform.position.y - wire.position.y) < 0.1f) // Adjust the threshold as needed
            {
                SnapToWire();
            }
            else
            {
                isSnapped = false;
                UpdateCheckbox(false); // Uncheck the checkbox when not snapped
            }
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        // Snap to the wire if close enough
        if (Mathf.Abs(transform.position.y - wire.position.y) < 0.1f) 
        {
            SnapToWire();
        }
        else
        {
            isSnapped = false;
            UpdateCheckbox(false); // Uncheck the checkbox when not snapped
        }
    }

    private void SnapToWire()
    {
        // Snap the paper rider to the wire's Y position
        transform.position = new Vector3(transform.position.x, wire.position.y, transform.position.z);
        isSnapped = true;

        // Ensure the paper rider stays between the bridges
        float clampedX = Mathf.Clamp(transform.position.x, firstBridge.position.x, secondBridge.position.x);
        transform.position = new Vector3(clampedX, wire.position.y, transform.position.z);

        // Check if the paper rider is nearly in the middle of the bridges
        CheckIfNearlyMid();
    }

    private void Update()
    {
        // If the paper rider is snapped, keep it between the bridges
        if (isSnapped)
        {
            float clampedX = Mathf.Clamp(transform.position.x, firstBridge.position.x, secondBridge.position.x);
            transform.position = new Vector3(clampedX, wire.position.y, transform.position.z);

            // Check if the paper rider is nearly in the middle of the bridges
            CheckIfNearlyMid();
        }
    }

    private void CheckIfNearlyMid()
    {
        // Calculate the middle position between the bridges
        float middleX = (firstBridge.position.x + secondBridge.position.x) / 2f;

        // Define a threshold for "nearly middle"
        float threshold = 0.1f;

        // Check if the paper rider is within the threshold
        bool isNearlyMid = Mathf.Abs(transform.position.x - middleX) < threshold;

        // Update the checkbox
        UpdateCheckbox(isNearlyMid);
    }

    private void UpdateCheckbox(bool isChecked)
    {
        if (midCheckbox != null)
        {
            midCheckbox.isOn = isChecked;
        }
    }
}