using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GroupPointsToVector3Array : ContainMassGroupOfVector3Mono
{
    public Transform [] m_points;
    public Vector3 [] m_pointsArrays;

    public override void GetVector3Ref(out Vector3[] points)
    {
        points = m_pointsArrays;
    }

    private void Update()
    {
        if (m_points.Length != m_pointsArrays.Length)
            m_pointsArrays = new Vector3[m_points.Length];
        for (int i = 0; i < m_points.Length; i++)
        {
            m_pointsArrays[i] = m_points[i].position;
        }
    }
}
