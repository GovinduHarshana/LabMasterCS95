using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelNavigation : MonoBehaviour
{
    private const string PreviousSceneKey = "PreviousScene";

    public void OpenNoteSaverFromFlameTestNew()
    {
        PlayerPrefs.SetString(PreviousSceneKey, "FlameTestNew"); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("NoteSaver");
    }

    public void OpenNoteSaverFromPhysicsPractical()
    {
        PlayerPrefs.SetString(PreviousSceneKey, "PhysicsPractical"); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("NoteSaver");
    }

    public void GoBackToPreviousScene()
    {
        string previousScene = PlayerPrefs.GetString(PreviousSceneKey, "FlameTestNew"); 
        SceneManager.LoadScene(previousScene);
    }
}