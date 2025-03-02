using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordToggle : MonoBehaviour
{
    public TMP_InputField passwordField;
    public Button eyeButton;
    public Image eyeIcon;
    public Sprite openEyeSprite;
    public Sprite closedEyeSprite;

    private bool isPasswordVisible = false;

    public void TogglePasswordVisibility()
    {
        isPasswordVisible = !isPasswordVisible;

        // Toggle password field content type
        passwordField.contentType = isPasswordVisible ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        passwordField.ForceLabelUpdate(); // Refresh text field

        // Debugging message
        Debug.Log("Password Visibility: " + isPasswordVisible);

        // Update the eye icon
        if (eyeIcon != null)
        {
            eyeIcon.sprite = isPasswordVisible ? openEyeSprite : closedEyeSprite;
            eyeIcon.rectTransform.sizeDelta = new Vector2(20f, 10f); // Set fixed size
        }

    }
}
