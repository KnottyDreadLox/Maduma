using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AiLevelGenBtn : MonoBehaviour
{

    GAMapGenerator mapGen;
    bool isSaved;


    public void GenerateMap()
    {

        mapGen = FindObjectOfType<GAMapGenerator>();

        string btnName = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        char shrt = btnName[btnName.Length - 1];
        int map = Convert.ToInt32(Convert.ToString(shrt));

        DNAMatrix thisMap = mapGen.DNAMatrices[map];

        mapGen.PlaceTilesOnTilemap(thisMap);
    }

    public void SaveMap()
    {
        mapGen = FindObjectOfType<GAMapGenerator>();


        string btnName = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        char shrt = btnName[btnName.Length - 1];
        int map = Convert.ToInt32(Convert.ToString(shrt));
        DNAMatrix thisMap = mapGen.DNAMatrices[map];

        //Unsave
        if (isSaved)
        {
            mapGen.SavedMatrices.Remove(thisMap);
            gameObject.GetComponent<Image>().color = Color.white;

            isSaved = false;

        }
        //Save
        else if (mapGen.SavedMatrices.Count <= 1)
        {
            mapGen.SavedMatrices.Add(thisMap);
            gameObject.GetComponent<Image>().color = Color.green;

            isSaved = true;
        }

        else
        {
            Debug.Log("Not Enough Random Maps Generated !");
        }
    }

}
