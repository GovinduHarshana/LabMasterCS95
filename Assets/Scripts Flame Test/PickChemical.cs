using UnityEngine;

public class PickChemical : MonoBehaviour
{
    public GameObject stick;
    public Color defaultStickColor = Color.gray;
    public UIController uiController;
    public FlameTestTable flameTestTable;

    public void OnPickButtonClick()
    {
        if (stick != null)
        {
            SpriteRenderer stickRenderer = stick.GetComponent<SpriteRenderer>();
            if (stickRenderer != null)
            {
                // Change the stick's color to gray
                stickRenderer.color = Color.gray;
                Debug.Log("Stick color changed to: " + Color.gray);
            }
            else
            {
                Debug.LogError("SpriteRenderer not found on the stick GameObject!");
            }
        }
        else
        {
            Debug.LogError("Stick reference not set in the Inspector!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Chemical1"))
        {
            Debug.Log("Chemical1 detected!");
            ChemicalData.Instance.SelectedChemicalColor = Color.red;
            uiController.ShowUIBox();
        }
        else if (other.CompareTag("Chemical2"))
        {
            Debug.Log("Chemical2 detected!");
            ChemicalData.Instance.SelectedChemicalColor = Color.yellow;
            uiController.ShowUIBox();
        }
        else if (other.CompareTag("Chemical3"))
        {
            Debug.Log("Chemical3 detected!");
            ChemicalData.Instance.SelectedChemicalColor = new Color(1f, 0f, 1f); // Purple
            uiController.ShowUIBox();
        }
        else if (other.CompareTag("Chemical4"))
        {
            Debug.Log("Chemical4 detected!");
            ChemicalData.Instance.SelectedChemicalColor = new Color(1f, 0f, 0.5f); // Red Purple
            uiController.ShowUIBox();
        }
        else if (other.CompareTag("Chemical5"))
        {
            Debug.Log("Chemical5 detected!");
            ChemicalData.Instance.SelectedChemicalColor = Color.blue; // Blue
            uiController.ShowUIBox();
        }
        else if (other.CompareTag("Chemical6"))
        {
            Debug.Log("Chemical6 detected!");
            ChemicalData.Instance.SelectedChemicalColor = new Color(1f, 0.2f, 0f); // Orange Red
            uiController.ShowUIBox();
        }
        else if (other.CompareTag("Chemical7"))
        {
            Debug.Log("Chemical7 detected!");
            ChemicalData.Instance.SelectedChemicalColor = new Color(0.8f, 0f, 0.2f); // Crimson Red
            uiController.ShowUIBox();
        }
        else if (other.CompareTag("Chemical8"))
        {
            Debug.Log("Chemical8 detected!");
            ChemicalData.Instance.SelectedChemicalColor = new Color(0f, 1f, 0f); // Green
            uiController.ShowUIBox();
        }

    }
}