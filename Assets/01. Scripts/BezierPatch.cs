using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPatch : MonoBehaviour
{
    public int row, col;
    public List<DVector> ControlPointsArray;
    public DVector[,] ControlPointsMatrix;

    public BVH boundingVolumeTree;
    
    public int[,] bernsteinPolynomial = {
        {   1,   0,   0,   0,   0,   0,   0,   0,   0,   0},
        {   1,   1,   0,   0,   0,   0,   0,   0,   0,   0},
        {   1,   2,   1,   0,   0,   0,   0,   0,   0,   0},
        {   1,   3,   3,   1,   0,   0,   0,   0,   0,   0},
        {   1,   4,   6,   4,   1,   0,   0,   0,   0,   0},
        {   1,   5,  10,  10,   5,   1,   0,   0,   0,   0},
        {   1,   6,  15,  20,  15,   6,   1,   0,   0,   0},
        {   1,   7,  21,  35,  35,  21,   7,   1,   0,   0},
        {   1,   8,  28,  56,  70,  56,  28,   8,   1,   0},
        {   1,   9,  36,  84, 126, 126,  84,  36,   9,   1}
    };

    // Start is called before the first frame update
    void Start()
    {
        ControlPointsMatrix = new DVector[row,col];
        ControlPointsArray = new List<DVector>();

        ControlPointsArray.Add(new DVector(0,0,6));
        ControlPointsArray.Add(new DVector(3,0,0));
        ControlPointsArray.Add(new DVector(6,0,0));
        ControlPointsArray.Add(new DVector(9,0,6));
        ControlPointsArray.Add(new DVector(0,3,3));
        ControlPointsArray.Add(new DVector(3,3,0));
        ControlPointsArray.Add(new DVector(6,3,0));
        ControlPointsArray.Add(new DVector(9,3,0));
        ControlPointsArray.Add(new DVector(0,6,6));
        ControlPointsArray.Add(new DVector(3,6,0));
        ControlPointsArray.Add(new DVector(6,6,0));
        ControlPointsArray.Add(new DVector(9,6,6));

        SetControlPointsMatrix();

        SetDotMatrix();
        
        boundingVolumeTree = new BVH(ControlPointsMatrix);
        
        boundingVolumeTree.Construct();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetControlPointsMatrix()
    {
        for(int i = 0; i < row; i++)
            for(int j = 0; j < col; j++)
            {
                ControlPointsMatrix[i,j] = ControlPointsArray[i * col + j];
                Debug.DrawLine(ControlPointsMatrix[i,j], ControlPointsMatrix[i,j] * (float)1.01, Color.green, 1000);
            }
    }

    void SetDotMatrix()
    {
        for(int i = 0; i < 1024; i+= 4)
            for(int j = 0; j < 1024; j+= 4)
            {
                DVector dot = CalculateBezierPatch(i,j);
                Debug.DrawLine(dot, dot * (float)1.1, Color.red, 100);

            }
    }

    DVector CalculateBezierPatch(int u, int v)
    {
        DVector result = DVector.zero;
        
        for(int i = 0; i < row; i++)
            for(int j = 0; j < col; j++)
            {
                DVector controlPoint = ControlPointsMatrix[i,j];

                double rowPoly = CalculateBezierPolynomial(row,i,u);
                
                double colPoly = CalculateBezierPolynomial(col,j,v);

                result += (controlPoint * rowPoly * colPoly);
                // Debug.Log(result.z);
            }

        return result;
    }

    double CalculateBezierPolynomial(int size, int count, int timeslice)
    {
        double result = 0;
        if(timeslice == 0 && count != 0) return 0;
        if(timeslice == 1024 && count != size - 1) return 0;

        int first = (1024 - timeslice);
        int second = timeslice;

        double firstResult = System.Math.Pow(first ,size - 1 - count) / System.Math.Pow(1024,size - 1 - count);
        double secondResult = System.Math.Pow(second, count) / System.Math.Pow(1024, count);

        result = bernsteinPolynomial[size - 1, count] * firstResult * secondResult;
        return result;
    }
}