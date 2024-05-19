using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextFileHelper : MonoBehaviour
{
    public void CreateText(string content)
    {
        //Path of the file
        string path = Application.dataPath + "/Log.txt";
        //Create File if it doesn't exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Login log \n\n");
        }
        //Content of the file
        string dateStampedContent = content + " | " + System.DateTime.Now + "\n";
        //Add some to text to it
        File.AppendAllText(path, dateStampedContent);
    }

}
