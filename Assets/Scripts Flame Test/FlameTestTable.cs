using UnityEngine;
using TMPro;

public class FlameTestTable : MonoBehaviour
{

    public TMP_Text LiText;
    public TMP_Text NaText;
    public TMP_Text KText;
    public TMP_Text RbText;
    public TMP_Text CsText;
    public TMP_Text CaText;
    public TMP_Text SrText;
    public TMP_Text BaText;


    private System.Collections.Generic.Dictionary<string, TMP_Text> chemicalTextMap;


    private System.Collections.Generic.Dictionary<string, string> chemicalToElementMap;

    private void Start()
    {
        // Initialize the chemical-to-element map
        chemicalToElementMap = new System.Collections.Generic.Dictionary<string, string>
        {
            { "Chemical1", "Li" },
            { "Chemical2", "Na" },
            { "Chemical3", "K" },
            { "Chemical4", "Rb" },
            { "Chemical5", "Cs" },
            { "Chemical6", "Ca" },
            { "Chemical7", "Sr" },
            { "Chemical8", "Ba" }
        };

        // Initialize the chemical-to-Text map
        chemicalTextMap = new System.Collections.Generic.Dictionary<string, TMP_Text>
        {
            { "Chemical1", LiText },
            { "Chemical2", NaText },
            { "Chemical3", KText },
            { "Chemical4", RbText },
            { "Chemical5", CsText },
            { "Chemical6", CaText },
            { "Chemical7", SrText },
            { "Chemical8", BaText }
        };

        // Initialize the table with placeholders
        foreach (var entry in chemicalTextMap)
        {
            string elementName = chemicalToElementMap[entry.Key];
            entry.Value.text = elementName + " - ?";
        }
    }

    // Method to update the table when a chemical is identified
    public void UpdateTable(string chemicalName, Color flameColor)
    {
        if (chemicalTextMap.ContainsKey(chemicalName))
        {
            TMP_Text textElement = chemicalTextMap[chemicalName];
            string elementName = chemicalToElementMap[chemicalName];
            textElement.text = elementName + " - " + ColorToString(flameColor);
        }
    }

    // Helper method to convert Color to a string
    private string ColorToString(Color color)
    {
        if (color == Color.red) return "Red";
        if (color == Color.yellow) return "Yellow";
        if (color == new Color(1f, 0f, 1f)) return "Purple";
        if (color == new Color(1f, 0f, 0.5f)) return "Purple-Red";
        if (color == Color.blue) return "Blue";
        if (color == new Color(1f, 0.2f, 0f)) return "Orange-Red";
        if (color == new Color(0.8f, 0f, 0.2f)) return "Crimson Red";
        if (color == new Color(0f, 1f, 0f)) return "Apple Green";
        return "No Color";
    }
}
