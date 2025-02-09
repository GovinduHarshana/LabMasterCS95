using UnityEngine;

public class StickController : MonoBehaviour
{
    public float moveSpeed = 15f;

    void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse button or touch
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Ensure no depth movement
            transform.position = Vector3.MoveTowards(transform.position, mousePos, moveSpeed * Time.deltaTime);
        }
    }
}

