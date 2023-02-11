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

}
