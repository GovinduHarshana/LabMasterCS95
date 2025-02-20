using UnityEngine;

public class WeightInteraction : MonoBehaviour
{
    public WeightPopup weightPopup;

    private bool isDragging = false;

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;


        if (IsInHangerArea())
        {
            weightPopup.ShowAddPopup(gameObject);
        }
    }

    void Update()
    {
        if (isDragging)
        {

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }


    bool IsInHangerArea()
    {
        Collider2D hangerCollider = WeightHangerManager.Instance.hangerArea.GetComponent<Collider2D>();
        return hangerCollider.OverlapPoint(transform.position);
    }
}