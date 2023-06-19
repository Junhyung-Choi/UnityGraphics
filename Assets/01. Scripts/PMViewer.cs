using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PMViewer : MonoBehaviour
{
    string meshPath, pmPath;
    
    public Slider slider;
    float pmValue, value;

    Mesh mesh, pmMesh;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<string> vsplitData = new List<string>();
    List<VSplit> vsplits = new List<VSplit>();

    private void Start() {
        meshPath = Application.dataPath + "/Resources/Dragon/dragon10k.m";
        pmPath = Application.dataPath + "/Resources/Dragon/dragon.pm";

        mesh = new Mesh();
        pmMesh = new Mesh();

        SetMesh();
        SetPM();   
    }

    void SetMesh()
    {
        MeshFilter meshFilter = this.transform.Find("MeshRenderer").GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        
        string meshData = System.IO.File.ReadAllText(meshPath);
        string[] meshDataSplit = meshData.Split('\n');
        List<string> vertexData = new List<string>();
        List<string> faceData = new List<string>();

        for(int i = 0; i < meshDataSplit.Length; i++)
        {
            if(meshDataSplit[i].Contains("Vertex"))
            {
                vertexData.Add(meshDataSplit[i]);
            }
            else if(meshDataSplit[i].Contains("Face"))
            {
                faceData.Add(meshDataSplit[i]);
            }
        }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        vertices.Add(new Vector3(0,0,0));

        for(int i = 0; i < vertexData.Count; i++)
        {
            string[] vertexDataSplit = vertexData[i].Split(' ');
            Vector3 vertex = new Vector3((float)double.Parse(vertexDataSplit[3]), (float)double.Parse(vertexDataSplit[4]), (float)double.Parse(vertexDataSplit[5]));
            vertices.Add(vertex);
        }

        for(int i = 0; i < faceData.Count; i++)
        {
            string[] faceDataSplit = faceData[i].Split(' ');
            triangles.Add(int.Parse(faceDataSplit[3]));
            triangles.Add(int.Parse(faceDataSplit[4]));
            triangles.Add(int.Parse(faceDataSplit[5]));
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void SetPM()
    {
        MeshFilter pmMeshFilter = this.transform.Find("PMRenderer").GetComponent<MeshFilter>();
        pmMeshFilter.mesh = pmMesh;

        string pmData = System.IO.File.ReadAllText(pmPath);
        string[] pmDataSplit = pmData.Split('\n');
        List<string> vertexData = new List<string>();
        List<string> faceData = new List<string>();
        
        Dictionary<int, int> newVertexIndexDict = new Dictionary<int, int>();

        for(int i = 0; i < pmDataSplit.Length; i++)
        {
            if(pmDataSplit[i].Contains("Vertex"))
            {
                vertexData.Add(pmDataSplit[i]);
            }
            else if(pmDataSplit[i].Contains("Face"))
            {
                faceData.Add(pmDataSplit[i]);
            }
            else if(pmDataSplit[i].Contains("Vsplit"))
            {
                vsplitData.Add(pmDataSplit[i]);
            }
        }

        for(int i = 0; i < vertexData.Count; i++)
        {
            string[] vertexDataSplit = vertexData[i].Split(' ');
            newVertexIndexDict.Add(int.Parse(vertexDataSplit[1]), i);
            Vector3 vertex = new Vector3((float)double.Parse(vertexDataSplit[2]), (float)double.Parse(vertexDataSplit[3]), (float)double.Parse(vertexDataSplit[4]));
            vertices.Add(vertex);
        }

        for(int i = 0; i < faceData.Count; i++)
        {
            string[] faceDataSplit = faceData[i].Split(' ');
            triangles.Add(newVertexIndexDict[int.Parse(faceDataSplit[2])]);
            triangles.Add(newVertexIndexDict[int.Parse(faceDataSplit[3])]);
            triangles.Add(newVertexIndexDict[int.Parse(faceDataSplit[4])]);
        }

        for(int i = 0; i < vsplitData.Count; i++)
        {
            string[] vsplitDataSplit = vsplitData[i].Split(' ');
            VSplit vsplit = new VSplit();
            Vector3 parent, child1, child2;
            child1 = new Vector3((float)double.Parse(vsplitDataSplit[2]), (float)double.Parse(vsplitDataSplit[3]), (float)double.Parse(vsplitDataSplit[4]));
            child2 = new Vector3((float)double.Parse(vsplitDataSplit[5]), (float)double.Parse(vsplitDataSplit[6]), (float)double.Parse(vsplitDataSplit[7]));
            parent = new Vector3((float)double.Parse(vsplitDataSplit[8]), (float)double.Parse(vsplitDataSplit[9]), (float)double.Parse(vsplitDataSplit[10]));
            vsplit.parent = parent;
            vsplit.child1 = child1;
            vsplit.child2 = child2;
            vsplits.Add(vsplit);
        }

        pmMesh.vertices = vertices.ToArray();
        pmMesh.triangles = triangles.ToArray();
        pmMesh.RecalculateNormals();
    }

    public void Render()
    {
        MeshFilter pmMeshFilter = this.transform.Find("PMRenderer").GetComponent<MeshFilter>();
        value = slider.value;
        value = Remap(value, 0, 1, 0, vsplits.Count);
        
        int index = (int)value;

        for(int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = vsplits[index].parent;
        }
        
        

    }

    public float Remap(float value, float minValue, float maxValue, float minTarget, float maxTarget)
    {
        return minTarget + (value - minValue) * (maxTarget - minTarget) / (maxValue - minValue);
    }
}

public class Vertex
{
    float x,y,z;
}

public class Face
{
    int v1, v2, v3;
}

public class VSplit
{
    public Vector3 parent;
    public Vector3 child1, child2;
}
