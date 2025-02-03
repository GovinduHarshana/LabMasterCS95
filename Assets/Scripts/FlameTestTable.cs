using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class FlameTestTable : MonoBehaviour
{
    // Define colors for each element
    public Color lithiumFlameColor = Color.red;
    public Color sodiumFlameColor = Color.yellow;
    public Color potassiumFlameColor = new Color(0.67f, 0.33f, 1f); // Lilac (adjust as needed)
    public Color calciumFlameColor = new Color(1f, 0.65f, 0f); // Orange-red
    public Color rubidiumFlameColor = new Color(0.6f, 0.1f, 0.1f); // Deep red/purple
    public Color strontiumFlameColor = new Color(0.9f, 0.1f, 0.2f); // Crimson red
    public Color cesiumFlameColor = new Color(0.25f, 0.25f, 1f); // Blue-violet
    public Color bariumFlameColor = new Color(0.5f, 1f, 0f); // Yellow-green

    // Array to hold the text components of the table cells
    public Text[] elementLabels; // Assign in the Inspector in order Li, Na, K, Ca, Rb, Sr, Cs, Ba

    // Corresponding array of element names (for easy lookup)
    private string[] elementNames = { "Li", "Na", "K", "Ca", "Rb", "Sr", "Cs", "Ba" };

    public void SetFlameColor(string element)
    {
        int index = -1;

        //Find the index of the element
        for (int i = 0; i < elementNames.Length; i++)
        {
            if (elementNames[i] == element)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            Debug.LogError("Element not found in the table: " + element);
            return;
        }

        Color flameColor = Color.white;  // Default color if element not found

        switch (element)
        {
            case "Li":
                flameColor = lithiumFlameColor;
                break;
            case "Na":
                flameColor = sodiumFlameColor;
                break;
            case "K":
                flameColor = potassiumFlameColor;
                break;
            case "Ca":
                flameColor = calciumFlameColor;
                break;
            case "Rb":
                flameColor = rubidiumFlameColor;
                break;
            case "Sr":
                flameColor = strontiumFlameColor;
                break;
            case "Cs":
                flameColor = cesiumFlameColor;
                break;
            case "Ba":
                flameColor = bariumFlameColor;
                break;
            default:
                Debug.LogWarning("No color defined for element: " + element);
                break;
        }

        // Update the corresponding table cell with the color
        if (elementLabels != null && index >= 0 && index < elementLabels.Length)
        {
            elementLabels[index].color = flameColor;
        }
        else
        {
            Debug.LogError("Element label array is not set up correctly or index out of bounds.");
        }
    }
}