using UnityEngine;

public class OpenLinks : MonoBehaviour
{
    // Open Email Client
    public void OpenEmail()
    {
        Application.OpenURL("mailto:labmastersim@gmail.com");
    }

    // Open WhatsApp Chat (for Web)
    public void OpenWhatsApp()
    {
        Application.OpenURL("https://api.whatsapp.com/send?phone=94713610490");
    }

    // Open WhatsApp on Mobile Apps
    public void OpenWhatsAppMobile()
    {
        Application.OpenURL("whatsapp://send?phone=94713610490");
    }

    // Open Instagram Profile
    public void OpenInstagram()
    {
        Application.OpenURL("https://www.instagram.com/labmasterlk/?next=%2F");
    }

    // Open LinkedIn Profile
    public void OpenLinkedIn()
    {
        Application.OpenURL("https://www.linkedin.com/company/labmasterlk/");
    }

    // Open Phone Dialer
    public void OpenPhoneCall()
    {
        Application.OpenURL("tel:+94713610490");
    }
}
