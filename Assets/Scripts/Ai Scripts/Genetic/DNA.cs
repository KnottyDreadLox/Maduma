using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu (fileName = "DNA", menuName = "GeneticAlgorithm/DNA")]
public class DNA : ScriptableObject
{
    public string DNAName;

    public TileBase tile;

    public int id;


}
