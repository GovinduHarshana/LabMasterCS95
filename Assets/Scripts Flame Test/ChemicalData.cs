using UnityEngine;

public class ChemicalData : MonoBehaviour
{
    public static ChemicalData Instance;

    public Color SelectedChemicalColor { get; set; } = Color.white;
    public string SelectedChemicalName { get; set; } = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}