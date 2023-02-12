using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public int depth = 0;
    public class Range
    {
        public double min = 0;
        public double max = 0;
    }

    public TreeNode leftChild,rightChild;

    public Range x = new Range();
    public Range y = new Range();
    public Range z = new Range();
    
    public string direction;

    public static int row, col;

    public DVector[,] corners;

    public void Init(DVector[,] CPMat, string direction, int depth)
    {
        this.x.min = 123456789;
        this.y.min = 123456789;
        this.z.min = 123456789;

        this.x.max = -123456789;
        this.y.max = -123456789;
        this.z.max = -123456789;
    
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                this.x.min = System.Math.Min(CPMat[i,j].x, this.x.min);
                this.y.min = System.Math.Min(CPMat[i,j].y, this.y.min);
                this.z.min = System.Math.Min(CPMat[i,j].z, this.z.min);

                this.x.max = System.Math.Max(CPMat[i,j].x, this.x.max);
                this.y.max = System.Math.Max(CPMat[i,j].y, this.y.max);
                this.z.max = System.Math.Max(CPMat[i,j].z, this.z.max);
            }
        }

        this.direction = direction;
        this.depth = depth;
    }

    public void SetLeaf(DVector[,] CPMat)
    {
        this.corners = new DVector[2,2];
        corners[0,0] = CPMat[0,0];
        corners[0,1] = CPMat[0,col - 1];
        corners[1,0] = CPMat[row - 1,0];
        corners[1,1] = CPMat[row - 1, col - 1];
    }

    public (BVH.QueueNode, BVH.QueueNode) Subdivision(DVector[,] CPMat)
    {
        BVH.QueueNode left = new BVH.QueueNode();
        BVH.QueueNode right = new BVH.QueueNode();

        TreeNode leftNode = new TreeNode();
        TreeNode rightNode = new TreeNode();

        DVector[,] leftCPMat = new DVector[row,col];
        DVector[,] rightCPMat = new DVector[row,col];

        string childDirection = "";
        if(this.direction == "u")
        {
            childDirection = "v";
            for(int i = 0; i < col; i++)
            {
                List<DVector> colVectors = new List<DVector>();
                
                for(int j = 0; j < row; j++)
                    colVectors.Add(CPMat[j,i]);

                List<DVector> triangle = Triangle(colVectors);
                List<DVector> diagonal = Diagonal(triangle, row);
                List<DVector> bottom = Bottom(triangle, row);
                
                for(int j = 0; j < row; j++)
                {
                    leftCPMat[j,i] = diagonal[j];
                    rightCPMat[j,i] = bottom[j];
                }
            }
        }
        else
        {
            childDirection = "u";
            for(int i = 0; i < row; i++)
            {
                List<DVector> rowVectors = new List<DVector>();
                
                for(int j = 0; j < col; j++)
                    rowVectors.Add(CPMat[i,j]);

                List<DVector> triangle = Triangle(rowVectors);
                List<DVector> diagonal = Diagonal(triangle, col);
                List<DVector> bottom = Bottom(triangle, col);
                
                for(int j = 0; j < col; j++)
                {
                    leftCPMat[i,j] = diagonal[j];
                    rightCPMat[i,j] = bottom[j];
                }
            }
        }

        leftNode.Init(leftCPMat,childDirection,this.depth+1);
        rightNode.Init(rightCPMat,childDirection,this.depth+1);

        leftChild = leftNode;
        rightChild = rightNode;

        left.node = leftNode;
        right.node = rightNode;

        left.controlPointsMatrix = leftCPMat;
        right.controlPointsMatrix = rightCPMat;

        return (left, right);
    }
    

    public List<DVector> Triangle(List<DVector> colVectors)
    {
        List<DVector> triangle = new List<DVector>();
        foreach(var vec in colVectors) triangle.Add(vec);

        int length = triangle.Count;
        int boundary = 0;
        while(true)
        {
            if(length == 1) break;
            else
            {
                for(int i = 0; i < length - 1; i++)
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

    public List<DVector> Diagonal(List<DVector> triangle, int size)
    {
        List<DVector> result = new List<DVector>();
        int index = 0;
        for(int i = size; i >= 1; i--)
        {
            result.Add(triangle[index]);
            index += i;
        }
        
        return result;
    }

    public List<DVector> Bottom(List<DVector> triangle, int size)
    {
        List<DVector> result = new List<DVector>();
        
        int index = triangle.Count-1;

        for(int i = 1; i <= size; i++)
        {
            result.Add(triangle[index]);
            index -= i;
        }
        
        return result;
    }

    public static void SetRowCol(int row, int col)
    {
        TreeNode.row = row;
        TreeNode.col = col;
    }
}