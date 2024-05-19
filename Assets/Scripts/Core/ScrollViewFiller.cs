using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollViewFiller : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonGameObject;

    [SerializeField]
    public Transform contentArea;

    private float buttonGap = -30f;

    // Call this method to add buttons dynamically
    public void AddButtons(int numberOfButtons, string buttonName)
    {
        // Clear existing buttons
        foreach (Transform child in contentArea)
        {
            buttonGap = -30f;       //reset button gap
            Destroy(child.gameObject);
        }

        // Add new buttons
        for (int i = 0; i < numberOfButtons; i++)
        {

            GameObject newButton = Instantiate(buttonGameObject, contentArea);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonName + " " + (i + 1);  // Set button text
            newButton.transform.position = new Vector3(newButton.transform.position.x, newButton.transform.position.y + buttonGap, newButton.transform.position.z);

            buttonGap += -30f;
        }
    }

    public void AddButton(string buttonName)
    {

        GameObject newButton = Instantiate(buttonGameObject, contentArea);
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonName + " " + (contentArea.childCount + 1);  // Set button text
        newButton.transform.position = new Vector3(newButton.transform.position.x, newButton.transform.position.y + buttonGap, newButton.transform.position.z);

        buttonGap += -30f;
    }
}
