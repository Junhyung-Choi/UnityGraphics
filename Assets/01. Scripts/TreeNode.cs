using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode : MonoBehaviour
{
    public enum NodeType
    {
        ROOT,
        BODY,
        LEAF
    }
    public List<TreeNode> childList;

    public NodeType nodeType;
    
}