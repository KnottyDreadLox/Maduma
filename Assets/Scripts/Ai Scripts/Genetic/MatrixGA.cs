using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatrixGA : MonoBehaviour
{
    private const float MUTATION_RATE = 0.1f;

    public DNAMatrix GeneticallyMutateMatrix(DNAMatrix parent1, DNAMatrix parent2, DNA[] DNAs)
    {

        DNAMatrix child = Crossover(parent1, parent2);

        Debug.Log("<color=yellow> =============== NEW CHILD ==============</color>");
        PrintMatrix(child);

        //child = Mutate(child, DNAs);

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
    private DNAMatrix Mutate(DNAMatrix matrix, DNA[] DNAs)
    {
        DNAMatrix mutatedMatrix = matrix;

        for (int x = 0; x < mutatedMatrix.Rows; x++)
        {
            for (int y = 0; y < mutatedMatrix.Columns; y++)
            {
                if (Random.Range(0, 1f) < MUTATION_RATE)
                {
                    // Get the DNA at this position
                    DNA currentDNA = DNAs[GetRandomExceptThis(mutatedMatrix.GetDNA(x, y), DNAs)];

                    // Set the mutated DNA back into the matrix
                    matrix.SetDNA(x, y, currentDNA);
                }
            }
        }

        return mutatedMatrix;
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