using System.Collections;
using System.Collections.Generic;
using UnityEngine;

     
    public class SkinnedMeshAsDrawRay : MonoBehaviour
{
    void Start()
    {
        SkinnedMeshAsVector3 mesh = GetComponent<SkinnedMeshAsVector3>();
        mesh.OnResultsReady += DrawVertices;
    }
    void DrawVertices(SkinnedMeshAsVector3 mesh)
    {
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Vector3 position = mesh.vertices[i];
            Vector3 normal = mesh.normals[i];
            Color color = Color.green;
           // Debug.DrawWireCube(position, 0.1f * Vector3.one, color);
            Debug.DrawRay(position, normal, color);
        }
    }
}


