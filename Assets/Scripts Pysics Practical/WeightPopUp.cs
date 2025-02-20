using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeightPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text popupMessage;
    public Button addButton;
    public Button cancelButton;

    private GameObject selectedWeight;

    void Start()
    {

        popupPanel.SetActive(false);


        addButton.onClick.AddListener(OnAddButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
    }


    public void ShowAddPopup(GameObject weight)
    {
        selectedWeight = weight;
        popupMessage.text = "Do you want to add 0.5kg to the hanger?";
        popupPanel.SetActive(true);
    }


    public void ShowRemovePopup(GameObject weight)
    {
        selectedWeight = weight;
        popupMessage.text = "Do you want to remove 0.5kg from the hanger?";
        popupPanel.SetActive(true);
    }


    void OnAddButtonClicked()
    {

        AddWeightToHanger(selectedWeight);
        popupPanel.SetActive(false);
    }


    void OnCancelButtonClicked()
    {

        Destroy(selectedWeight);
        popupPanel.SetActive(false);
    }


    void AddWeightToHanger(GameObject weight)
    {

        weight.transform.SetParent(WeightHangerManager.Instance.hangerArea.transform, false);
        weight.transform.localPosition = Vector3.zero;


        WeightHangerManager.Instance.AddWeight(0.5f);
    }
}