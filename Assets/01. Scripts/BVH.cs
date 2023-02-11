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

}