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

        DNAMatrix thisMap = mapGen.DNAMatrices[map - 1];
        mapGen.PlaceTilesOnTilemap(thisMap);

        //if (btnName.Contains("Random"))
        //{
        //    DNAMatrix thisMap = mapGen.DNAMatrices[map-1];
        //    mapGen.PlaceTilesOnTilemap(thisMap);
        //}
        //else
        //{
        //    DNAMatrix thisMap = mapGen.SavedMatrices[map-1];
        //    mapGen.PlaceTilesOnTilemap(thisMap);
        //}

    }

    public void SaveMap()
    {
        mapGen = FindObjectOfType<GAMapGenerator>();


        string btnName = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        char shrt = btnName[btnName.Length - 1];
        int map = Convert.ToInt32(Convert.ToString(shrt));
        DNAMatrix thisMap = mapGen.DNAMatrices[map - 1];

        Debug.Log(mapGen.SavedMatrices.Count);

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
            Debug.Log("Already have 2 Saved !");
        }
    }

}
