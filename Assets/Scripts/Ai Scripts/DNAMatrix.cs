using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAMatrix : MonoBehaviour
{
    //Creates a Matrix of vector positions
    private Vector3Int[,] matrix;

    //Creates a DNA matrix
    private DNA[,] DNAs;

    public int Rows { get; private set; }
    public int Columns { get; private set; }

    public DNAMatrix(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        matrix = new Vector3Int[rows, columns];
        DNAs = new DNA[rows, columns]; // Initialize the names array
    }

    public Vector3Int this[int row, int col]
    {
        get { return matrix[row, col]; }
        set { matrix[row, col] = value; }
    }

    public static Vector3Int Translate(Vector3Int point, int dx, int dy)
    {
        return new Vector3Int(point.x + dx, point.y + dy, point.z);
    }

    public static Vector3Int Rotate(Vector3Int point, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        int x = Mathf.RoundToInt(point.x * cos - point.y * sin);
        int y = Mathf.RoundToInt(point.x * sin + point.y * cos);

        return new Vector3Int(x, y, point.z);
    }


    public DNA GetDNA(int row, int col)
    {
        return DNAs[row, col];
    }

    public void SetDNA(int row, int col, DNA dna)
    {
        this.DNAs[row, col] = dna;
    }

    public string GetDNAName(int row, int col)
    {
        return DNAs[row, col].name;
    }

    public void SetDNAName(int row, int col, string name)
    {
        DNAs[row, col].name = name;
    }
}