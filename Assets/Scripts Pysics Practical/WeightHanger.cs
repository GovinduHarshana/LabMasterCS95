using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeightHanger : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI weightText;
    public Button yesButton;
    public Button noButton;

    public float snapDistance = 0.5f;
    public float totalWeight = 0f;
    public float weightSpacing = 0.5f; // Vertical spacing between weights
    public Vector3 firstWeightOffset = new Vector3(0, -2f, 0); // Initial position for the first weight

    private GameObject currentWeight; // The weight currently near the hanger

    private void Start()
    {
        // Assign the button click listeners
        yesButton.onClick.AddListener(OnYesButtonClick);
        noButton.onClick.AddListener(OnNoButtonClick);

        // Hide the popup panel initially
        popupPanel.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Weight"))
        {
            float distance = Vector2.Distance(collision.transform.position, transform.position);
            if (distance <= snapDistance)
            {
                currentWeight = collision.gameObject;
                ShowPopup();
            }
        }
    }

    private void ShowPopup()
    {
        popupPanel.SetActive(true); // Show the popup
    }

    public void OnYesButtonClick()
    {
        AddWeight(0.5f);
        AttachWeightToHanger(); // Attach the weight to the hanger
        popupPanel.SetActive(false); // Hide the popup
    }

    public void OnNoButtonClick()
    {
        popupPanel.SetActive(false); // Hide the popup
    }

    public void AddWeight(float weight)
    {
        totalWeight += weight;
        UpdateWeightText();
    }

    private void UpdateWeightText()
    {
        weightText.text = "Weight: " + totalWeight + " kg"; // Update the TextMeshPro text
    }

    private void AttachWeightToHanger()
    {
        if (currentWeight != null)
        {

            currentWeight.transform.SetParent(transform);


            Rigidbody2D weightRigidbody = currentWeight.GetComponent<Rigidbody2D>();
            if (weightRigidbody != null)
            {
                weightRigidbody.simulated = false;
            }

            // Calculate the position for the new weight
            int weightCount = transform.childCount - 1;
            Vector3 position;

            if (weightCount == 1)
            {
                position = firstWeightOffset;
            }
            else
            {
                float yOffset = -weightSpacing * (weightCount - 1); // Adjust Y position based on weight count
                position = firstWeightOffset + new Vector3(0, yOffset, 0);
            }

            currentWeight.transform.localPosition = position;
        }
    }
}