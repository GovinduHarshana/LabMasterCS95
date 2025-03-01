using UnityEngine;
using UnityEngine.UI;

public class PasswordToggle : MonoBehaviour
{
    public Button toggleButton;     // Reference to the button with the eye icon
    public Image buttonImage;       // Reference to the button's Image component
    public Sprite openEyeSprite;    // Open eye icon (password visible)
    public Sprite closedEyeSprite;  // Closed eye icon (password hidden)
    public InputField passwordField; // Reference to the InputField for password

    private bool isPasswordVisible = false;

    void Start()
    {
        // Initialize button with the closed eye icon
        buttonImage.sprite = closedEyeSprite;

        // Add listener to the button
        toggleButton.onClick.AddListener(TogglePassword);
    }

    void TogglePassword()
    {
        isPasswordVisible = !isPasswordVisible;

        // Toggle password visibility
        if (isPasswordVisible)
        {
            passwordField.contentType = InputField.ContentType.Standard; // Show password
            buttonImage.sprite = openEyeSprite;  // Change icon to open eye
        }
        else
        {
            passwordField.contentType = InputField.ContentType.Password; // Hide password
            buttonImage.sprite = closedEyeSprite;  // Change icon to closed eye
        }

        // Force the InputField to update its display
        passwordField.ForceLabelUpdate();
    }
}
