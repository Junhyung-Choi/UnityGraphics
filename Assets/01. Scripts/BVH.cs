using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVH
{
    GameObject dot;

    public DVector[,] controlPointsMatrix;

    public BVH(DVector[,] matrix)
    {
        this.controlPointsMatrix = matrix;
    }

    public void Construct()
    {

    }

    public (DVector[,], DVector[,]) Subdivision(DVector[,] parentMatrix)
    {
        int row = parentMatrix.GetLength(0);
        int col = parentMatrix.GetLength(1);

        DVector[,] leftChildMatrix = new DVector[row,col];
        DVector[,] rightChildMatrix = new DVector[row,col];



        return (leftChildMatrix, rightChildMatrix);
    }

    public List<DVector> Subdivision(List<DVector> col)
    {
        List<DVector> triangle = new List<DVector>();
        foreach(var vec in col) triangle.Add(vec);

        int length = triangle.Count;
        int boundary = 0;
        while(true)
        {
            if(length == 1) break;
            else
            {
                for(int i = 0; i < length; i++)
                {
                    DVector tmp = (triangle[boundary + i] * 0.5) + (triangle[boundary + i + 1] * 0.5);
                    triangle.Add(tmp);
                }
                boundary += length;
                length -= 1;
            }
        }

        return triangle; 
    }

}