using UnityEngine;

public class MovableBridge : MonoBehaviour
{
    public Transform leftBoundary; // First bridge position
    public Transform rightBoundary; // Sonometer end corner position

    public float startOffset = 5f; // Start at 5 cm from the left boundary
    public float pointsPerCm = 6f; // 6 points = 1 cm

    private bool isDragging = false;
    private Vector3 offset;

    private void Start()
    {
        // Set the initial position of the second bridge at 5 cm from the left boundary
        float startPositionX = leftBoundary.position.x + (startOffset / pointsPerCm);
        transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
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
            float newX = Mathf.Clamp(mousePosition.x + offset.x, leftBoundary.position.x, rightBoundary.position.x);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }
}