using UnityEngine;
using TMPro;

public class WeightHangerManager : MonoBehaviour
{
    public static WeightHangerManager Instance;

    public Transform hangerArea;
    public TMP_Text weightText;

    private float totalWeight = 0f;

    void Awake()
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


    public void AddWeight(float weight)
    {
        totalWeight += weight;
        UpdateWeightDisplay();
    }


    public void RemoveWeight(float weight)
    {
        totalWeight -= weight;
        UpdateWeightDisplay();
    }


    void UpdateWeightDisplay()
    {
        weightText.text = "Weight: " + totalWeight + " kg";
    }
}