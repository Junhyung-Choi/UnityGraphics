using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBoxDrawer : MonoBehaviour
{
    public static DebugBoxDrawer instance;
    public GameObject DebugBox;

    void Awake()
    {
        instance = this;
    }

    public void DrawDebugBox(TreeNode.Range x, TreeNode.Range y, TreeNode.Range z)
    {
        // Debug.Log("Drawing");
        GameObject tmpBox = Instantiate(this.DebugBox, null);
        
        Vector3 pos = new Vector3((float)x.min, (float)y.min, (float)z.min);
        tmpBox.transform.position = pos;
        
        Vector3 scale = new Vector3((float)(x.max - x.min), (float)(y.max - y.min), (float)(z.max - z.min));
        tmpBox.transform.localScale = scale;
    }
}