using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MeshAnimationToVector3Array : ContainMassGroupOfVector3Mono
{
    public Transform m_meshPoint;
    public SkinnedMeshRenderer m_meshRenderer;
    public Vector3[] m_points;
    public NativeArray<Vector3> m_nativeArrayOfPoints;

    public void Awake()
    {
        ExtractPointsOfMesh();
    }

    private void ExtractPointsOfMesh()
    {
        m_points = m_meshRenderer.sharedMesh.vertices;
        for (int i = 0; i < m_points.Length; i++)
        {
            Eloi.E_RelocationUtility.GetLocalToWorld_Point(in m_points[i], m_meshPoint, out m_points[i]);
        }


        if (m_nativeArrayOfPoints == null) { 
            m_nativeArrayOfPoints = new NativeArray<Vector3>(m_points, Allocator.Persistent);
        }
        else {
            for (int i = 0; i < m_nativeArrayOfPoints.Length; i++)
            {
                m_nativeArrayOfPoints[i] = m_points[i];

            }
        }

    }
    private void OnDestroy()
    {
        if (m_nativeArrayOfPoints != null)
            m_nativeArrayOfPoints.Dispose();
    }

 

    public override void GetVector3Ref(out Vector3[] points)
    {
        points = m_points;
    }
}
