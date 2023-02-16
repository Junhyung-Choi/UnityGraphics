using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVH
{
    int row, col;
    GameObject dot;

    public TreeNode root;
    public int MAX_DEPTH = 17;
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

    public void ShowCollisionLeafNode(GameObject cube, string mode)
    {
        Queue<TreeNode> queue = new Queue<TreeNode>();
        float minX = cube.transform.position.x;
        float maxX = minX + cube.transform.localScale.x;
        float lenX = cube.transform.localScale.x;
        float minY = cube.transform.position.y;
        float maxY = minY + cube.transform.localScale.y;
        float lenY = cube.transform.localScale.y;
        float minZ = cube.transform.position.z;
        float maxZ = minZ + cube.transform.localScale.z;
        float lenZ = cube.transform.localScale.z;

        queue.Enqueue(root);

        TreeNode node;
        // bool condition = false;

        while(queue.Count != 0)
        {
            node = queue.Dequeue();

            if(
                node.x.min <= maxX && node.x.max >= minX &&
                node.y.min <= maxY && node.y.max >= minY &&
                node.z.min <= maxZ && node.z.max >= minZ
            )
            {
                if(mode == "leaf")
                {
                    if(node.depth == MAX_DEPTH)
                        DebugBoxDrawer.instance.DrawDebugBox(node.x,node.y,node.z);
                }
                else
                {
                    DebugBoxDrawer.instance.DrawDebugBox(node.x,node.y,node.z);
                }

                if(node.depth != MAX_DEPTH)
                {
                    queue.Enqueue(node.leftChild);
                    queue.Enqueue(node.rightChild);
                }
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

    public void ShowMinDistance(GameObject obj)
    {
        float minDistance = 123456789f;
        TreeNode resultNode = null;

        Queue<TreeNode> queue = new Queue<TreeNode>();
        queue.Enqueue(root);

        while(queue.Count != 0)
        {
            TreeNode node = queue.Dequeue();
            if(node.depth < MAX_DEPTH)
            {
                Vector3 leftVec = node.leftChild.GetCenter();
                Vector3 rightVec = node.rightChild.GetCenter();

                float leftDistance = Vector3.Distance(leftVec,obj.transform.position);
                float rightDistance = Vector3.Distance(rightVec,obj.transform.position);

                if(leftDistance <= rightDistance) queue.Enqueue(node.leftChild);
                else queue.Enqueue(node.rightChild);
            }
            else if (node.depth == MAX_DEPTH)
            {
                // 이 부분 수정 필요
                if(minDistance >= Vector3.Distance(node.GetCenter(),obj.transform.position))
                {
                    minDistance = Vector3.Distance(node.GetCenter(),obj.transform.position);
                    resultNode = node; 
                }
            }
        }

        Debug.DrawLine(resultNode.corners[0,0],obj.transform.position,Color.yellow,1);
    }

}