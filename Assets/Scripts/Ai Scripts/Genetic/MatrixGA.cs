using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatrixGA : MonoBehaviour
{
    private const double MUTATION_RATE = 0.1;




    public DNAMatrix GeneticallyMutateMatrix(DNAMatrix parent1, DNAMatrix parent2, DNA[] DNAs)
    {

        DNAMatrix child = Crossover(parent1, parent2);

        Debug.Log("<color=yellow> =============== NEW CHILD ==============</color>");
        PrintMatrix(child);

        Mutate(child, DNAs);

        Debug.Log("Mutated Child:");
        PrintMatrix(child);

        return child;
    }

    // Crossover two parent matrices to produce a child matrix
    private DNAMatrix Crossover(DNAMatrix parent1, DNAMatrix parent2)
    {
        //Create a dnaMatrixChild, let it use any parent's rows columns to create a new matrix of the same size. 
        //Which parent is used to create the new dnaMatrixChild is irrelevent.
        DNAMatrix dnaMatrixChild = new DNAMatrix(parent1.Rows, parent1.Columns);

        //for each DNA in the matrix...
        for(int x = 0; x < parent1.Rows; x++)
        {
            for (int y = 0; y < parent1.Columns; y++)
            {
                //Make a new DNA child to replace the one here...
                DNA dnaChild;

                int r = Random.Range(0, 100);
                Debug.Log("<color=red> Random Number : " + r + " </color>");

                // Randomly choose genes from parents
                if (r <= 50)
                    dnaChild = parent1.GetDNA(x, y);
                else
                    dnaChild = parent2.GetDNA(x, y);

                Debug.Log("Random Range : " + r + " at (" + x + ","+ y + ")");
                //sets the new child in place in thew new matrix
                dnaMatrixChild.SetDNA(x, y, dnaChild);
            }
        }

        return dnaMatrixChild;
    }

    // Mutate a matrix
    private void Mutate(DNAMatrix matrix, DNA[] DNAs)
    {
        for (int x = 0; x < matrix.Rows; x++)
        {
            for (int y = 0; y < matrix.Columns; y++)
            {
                if (Random.Range(0, 1) < MUTATION_RATE)

                {
                    // Get the DNA at this position
                    DNA currentDNA = matrix.GetDNA(x, y);

                    currentDNA = DNAs[Random.Range(0,DNAs.Length)];

                    // Set the mutated DNA back into the matrix
                    matrix.SetDNA(x, y, currentDNA);
                }
            }
        }
    }


    int GetRandomExceptThis(DNA currentDNA, DNA[] DNAs)
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, DNAs.Length);
        } while (DNAs[randomIndex] == currentDNA); // Repeat until a different index is generated
        return randomIndex;
    }


    // Helper method to print a matrix
    private void PrintMatrix(DNAMatrix matrix)
    {
        //This code is for debug of the postions of tiles.
        Debug.Log("Tile Matrix:");

        for (int row = 0; row < matrix.Rows; row++)
        {
            string rowString = "";
            for (int col = 0; col < matrix.Columns; col++)
            {
                string dnaName = matrix.GetDNA(col, row).DNAName;
                char firstLetter = dnaName[0];

                rowString += firstLetter + " ";
            }

            //Debug.Log(rowString);
        }
    }
}