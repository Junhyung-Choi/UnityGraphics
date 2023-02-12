using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVH
{
    int row, col;
    GameObject dot;

    public TreeNode root;
    public int MAX_DEPTH = 16;
    public DVector[,] controlPointsMatrix;

    public class QueueNode
    {
        public TreeNode node;
        public DVector[,] controlPointsMatrix;
    }

    public Queue<QueueNode> queue;

    public BVH(DVector[,] matrix)
    {
        this.controlPointsMatrix = matrix;
        this.row = matrix.GetLength(0);
        this.col = matrix.GetLength(1);

        TreeNode.SetRowCol(row,col);
    }

    public void Construct()
    {
        root = new TreeNode();
        root.Init(this.controlPointsMatrix,"u",0);

        QueueNode qNode = new QueueNode();
        qNode.node = root;
        qNode.controlPointsMatrix = this.controlPointsMatrix;

        queue = new Queue<QueueNode>();

        queue.Enqueue(qNode);

        while(queue.Count != 0)
        {
            QueueNode tmpQueueNode = queue.Dequeue();

            TreeNode tmpNode = tmpQueueNode.node;
            DVector[,] tmpCPMat = tmpQueueNode.controlPointsMatrix;

            QueueNode left = new QueueNode();
            QueueNode right = new QueueNode();

            (left,right) = tmpNode.Subdivision(tmpCPMat);

            if(left.node.depth <= MAX_DEPTH)
            {
                if(left.node.depth == MAX_DEPTH)
                {
                    left.node.SetLeaf(left.controlPointsMatrix);
                    right.node.SetLeaf(right.controlPointsMatrix);
                }

                queue.Enqueue(left);
                queue.Enqueue(right);
            }
        }
    }

    public void ShowCollisionLeafNode(GameObject cube)
    {
        Queue<TreeNode> queue = new Queue<TreeNode>();
        float minX = cube.transform.position.x;
        float maxX = minX + cube.transform.localScale.x;
        float minY = cube.transform.position.y;
        float maxY = minY + cube.transform.localScale.y;
        float minZ = cube.transform.position.z;
        float maxZ = minZ + cube.transform.localScale.z;

        queue.Enqueue(root);

        TreeNode node;
        while(queue.Count != 0)
        {
            node = queue.Dequeue();

            if(
                node.x.min <= maxX && node.x.max >= minX &&
                node.y.min <= minY && node.y.max >= maxY &&
                node.z.min <= minZ && node.z.max >= maxZ
            )
            {
                DebugBoxDrawer.instance.DrawDebugBox(node.x,node.y,node.z);
                queue.Enqueue(node.leftChild);
                queue.Enqueue(node.rightChild);
            }
        }
        
    }

    public void LogNodes()
    {
        Queue<TreeNode> queue = new Queue<TreeNode>();
        queue.Enqueue(root);

        while(queue.Count != 0)
        {
            TreeNode node = queue.Dequeue();
            DebugBoxDrawer.instance.DrawDebugBox(node.x, node.y, node.z);
            if(node.depth < MAX_DEPTH)
            {
                queue.Enqueue(node.leftChild);
                queue.Enqueue(node.rightChild);
            }
        }
    }
}