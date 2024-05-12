using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MotherNature : MonoBehaviour
{

    int dnaLenght;
    public Dictionary<bool, int> genetics;

    // Start is called before the first frame update
    void Start()
    {
        genetics = new Dictionary<bool, int>();

    }

    //Set random genetics
    public void SetRandom(DNA[] DNAs)
    {
        genetics.Clear();
        genetics.Add(false, Random.Range(0, DNAs.Length));
        genetics.Add(true, Random.Range(0, DNAs.Length));

        dnaLenght = genetics.Count;
    }

    //set if the genetic is active or inactive. 
    //Also has the DNA Array, IE the AiTiles, to
    //set a random value to represent the colour.
    public void SetGenetics(bool isActive, DNA[] DNAs)
    {
        genetics.Clear();
        genetics.Add(isActive, Random.Range(0, DNAs.Length));

        dnaLenght = genetics.Count;
    }


    //public void Combine(DNA d1, DNA d2)
    //{
    //    int i = 0;

    //    Dictionary<bool, int> newGenes = new Dictionary<bool, int>();

    //    foreach (KeyValuePair<bool, int> gene in genetics)
    //    {
    //        if (i < dnaLenght / 2)
    //        {
    //            newGenes.Add(gene.Key, d1.genetics[gene.Key]);
    //        }
    //        else
    //        {
    //            newGenes.Add(gene.Key, d2.genetics[gene.Key]);
    //        }

    //        i++;
    //    }

    //    genetics = newGenes;
    //}

    public float GetGene(bool active)
    {
        return genetics[active];
    }
}
