using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MeshAnimationToVector3Array : ContainMassGroupOfVector3Mono
{
    public Transform m_meshPoint;
    public SkinnedMeshRenderer m_meshRenderer;
    public Vector3[] m_points;

    public void Awake()
    {
        ExtractPointsOfMesh();
    }
    public void Update()
    {
        ExtractPointsOfMesh();
    }

    [ContextMenu("Refresh")]
    private void ExtractPointsOfMesh()
    {
        m_points = m_meshRenderer.sharedMesh.vertices;
        for (int i = 0; i < m_points.Length; i++)
        {

            //            Eloi.E_RelocationUtility.RotatePointAroundPivot();
            m_points[i] = m_meshPoint.rotation * m_points[i];
            m_points[i] += m_meshPoint.position;

        }

    }
  
 

    public override void GetVector3Ref(out Vector3[] points)
    {
        points = m_points;
    }
}
