using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Matrix2D
{
    private Vector3Int[,] matrix;

    public int Rows { get; private set; }
    public int Columns { get; private set; }


    public Matrix2D(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        matrix = new Vector3Int[rows, columns];

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


}