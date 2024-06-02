using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarHandler : MonoBehaviour
{
    Scrollbar scrollbar;  // Reference to the Scrollbar component
    public int currentValue;     // The current value corresponding to the Scrollbar's position

    private int numberOfSteps = 0;

    [SerializeField]
    private TextMeshProUGUI uiText;

    [SerializeField]
    private int offset;

    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();

        // Ensure the scrollbar starts at the first step
        scrollbar.value = 0;
        UpdateCurrentValue();

        numberOfSteps = scrollbar.numberOfSteps;

        // Add a listener to detect when the scrollbar value changes
        scrollbar.onValueChanged.AddListener(delegate { UpdateCurrentValue(); });
    }

    public void UpdateCurrentValue()
    {
        // Calculate the current step
        float stepValue = scrollbar.value * (numberOfSteps - 1);
        currentValue = Mathf.RoundToInt(stepValue) + offset;

        //Debug.Log("Current Value: " + currentValue);

        uiText.text = currentValue.ToString();
    }

    public int GetScrollbarValue()
    {
        return currentValue;
    }
}