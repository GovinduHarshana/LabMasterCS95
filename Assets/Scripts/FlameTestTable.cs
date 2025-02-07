
using UnityEngine;
using TMPro; // For TextMeshPro
using System.Collections.Generic; // For List

public class TableManager : MonoBehaviour
{
    //private List<string> chemicalNames = new() { "Li", "Be", "Na", "Mg", "K", "Ca", "Rb", "Sr", "Cs", "Ba" };
    //private List<string> chemicalColorNames = new() { "Carmine red", "Faint white", "Yellow", "White", "Lilac", "Orange-red", "Red-violet", "Crimson red", "Blue", "Apple green" };

    public List<TextMeshProUGUI> cellTexts = new List<TextMeshProUGUI>(); // List of table cells

    private Dictionary<string, string> flameColors = new Dictionary<string, string>
    {
        {"Li - ?", "Li - Carmine red"},
        {"Be - ?", "Be - Faint white"},
        {"Na - ?", "Na - Yellow"},
        {"Mg - ?", "Mg - White"},
        {"Ca - ?", "Ca - Lilac" },
        {"Rb - ?", "Rb - Orange-red " },
        {"Sr - ?", "Sr - Red-violet" },
        {"Cs - ?", "Cs - Crimson red" },
        {"Ba - ?", "Ba - Apple green" }
    };
    private int nextAvailableCell = 0; // Track the next cell to update

    public void UpdateCellWithChemical(string chemical)
    {
        if (nextAvailableCell < cellTexts.Count && flameColors.ContainsKey(chemical))
        {
            cellTexts[nextAvailableCell].text = flameColors[chemical];
            Debug.Log("Updated Cell: " + cellTexts[nextAvailableCell].text);
            nextAvailableCell++; // Move to the next cell
        }
    }

    public void ResetTable()
    {
        nextAvailableCell = 0;
        foreach (var cell in cellTexts)
        {
            cell.text = ""; // Clear all cells
        }
    }
}
