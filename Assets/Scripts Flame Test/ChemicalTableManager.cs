using UnityEngine;
using TMPro;

public class ChemicalTableManager : MonoBehaviour
{
   
    public TextMeshProUGUI[] chemicalNameTexts; 
    public TextMeshProUGUI[] chemicalColorTexts; 

    // Chemical names and their corresponding colors
    private string[] chemicalNames = { "Li", "Na", "K", "Rb", "Cs", "Ca", "Sr", "Ba" };
    private Color[] chemicalColors = {
        Color.red,          // Li
        Color.yellow,       // Na
        new Color(1f, 0f, 1f), // K (Purple)
        new Color(1f, 0f, 0.5f), // Rb (Purple-Red)
        Color.blue,         // Cs
        new Color(1f, 0.2f, 0f), // Ca (Orange-Red)
        new Color(0.8f, 0f, 0.2f), // Sr (Crimson Red)
        new Color(0f, 1f, 0f)  // Ba (Apple Green)
    };

    private void Start()
    {
        // Initialize the table with chemical names and "?" for colors
        for (int i = 0; i < chemicalNames.Length; i++)
        {
            chemicalNameTexts[i].text = chemicalNames[i];
            chemicalColorTexts[i].text = "?";
        }
    }

    // Update the table based on the flame color
    public void UpdateTable(Color flameColor)
    {
        for (int i = 0; i < chemicalColors.Length; i++)
        {
            if (chemicalColors[i] == flameColor)
            {
                chemicalColorTexts[i].text = ColorToName(flameColor);
                chemicalColorTexts[i].color = flameColor; // Optional: Set the text color to match the flame color
                break;
            }
        }
    }

    // Helper method to convert Color to a name
    private string ColorToName(Color color)
    {
        if (color == Color.red) return "Red";
        if (color == Color.yellow) return "Yellow";
        if (color == new Color(1f, 0f, 1f)) return "Purple";
        if (color == new Color(1f, 0f, 0.5f)) return "Purple-Red";
        if (color == Color.blue) return "Blue";
        if (color == new Color(1f, 0.2f, 0f)) return "Orange-Red";
        if (color == new Color(0.8f, 0f, 0.2f)) return "Crimson Red";
        if (color == new Color(0f, 1f, 0f)) return "Apple Green";
        return "?";
    }
}