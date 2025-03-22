using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Networking;

public class SlidingMenu : MonoBehaviour
{
    public RectTransform menuPanel;      // Assign the Menu Panel in Inspector
    public Button menuButton;            // Assign the Menu Button
    public Button closeButton;           // Assign the Close Button inside the menu
    public GameObject backgroundOverlay; // Assign the invisible full-screen button

    // UI elements to update with user data
    public TextMeshProUGUI menuNameText;
    public TextMeshProUGUI menuRoleText;
   

    private Vector2 hiddenPosition = new Vector2(-600, -540); // Off-screen position
    private Vector2 visiblePosition = new Vector2(0, -540);   // Visible position
    private bool isOpen = false;
    private float slideSpeed = 0.3f; // Adjust for faster/slower slide

    void Start()
    {
        // Set initial menu position (hidden)
        menuPanel.anchoredPosition = hiddenPosition;

        // Add button event listeners
        menuButton.onClick.AddListener(OpenMenu);
        closeButton.onClick.AddListener(CloseMenu);
        backgroundOverlay.GetComponent<Button>().onClick.AddListener(CloseMenu);

        // Initially hide the background overlay
        backgroundOverlay.SetActive(false);

        // Update menu UI with user data
        UpdateMenuUI();

        // Subscribe to the event, so when the user data is updated, the menu will also update.
        if (UserDataManager.Instance != null)
        {
            UserDataManager.OnUserDataLoaded += UpdateMenuUI;
        }
        else
        {
            Debug.LogError("UserDataManager not found!");
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event, so no errors when the menu is destroyed.
        if (UserDataManager.Instance != null)
        {
            UserDataManager.OnUserDataLoaded -= UpdateMenuUI;
        }
    }

    public void OpenMenu()
    {
        if (!isOpen)
        {
            StartCoroutine(SlideMenu(visiblePosition));
            isOpen = true;
            backgroundOverlay.SetActive(true); // Show the click-outside button
        }
    }

    public void CloseMenu()
    {
        if (isOpen)
        {
            StartCoroutine(SlideMenu(hiddenPosition));
            isOpen = false;
            backgroundOverlay.SetActive(false); // Hide the click-outside button
        }
    }

    IEnumerator SlideMenu(Vector2 target)
    {
        float elapsedTime = 0;
        Vector2 startPos = menuPanel.anchoredPosition;

        while (elapsedTime < slideSpeed)
        {
            menuPanel.anchoredPosition = Vector2.Lerp(startPos, target, elapsedTime / slideSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        menuPanel.anchoredPosition = target;
    }

    void UpdateMenuUI()
    {
        if (UserDataManager.Instance != null)
        {
            // Update name and role
            menuNameText.text = UserDataManager.Instance.userName;
            menuRoleText.text = UserDataManager.Instance.userRole;

        }
    }

    
}